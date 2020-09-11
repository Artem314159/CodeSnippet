using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CodeSnippet.Data;
using CodeSnippet.Data.Models;
using CodeSnippet.Models;
using CodeSnippet.Services.Interfaces;
using CodeSnippet.Services.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CodeSnippet.Services.Base;

namespace CodeSnippet.Services.Groups
{
    public class GroupCreateService : ServiceBase, IGroupCreateService
    {
        private readonly IGroupService _groupsService;
        private readonly IContactService _contactsService;
        private readonly ILogger<IGroupCreateService> _logger;

        public GroupCreateService(
            DataContext dataContext,
            IGroupService groupsService,
            IContactService contactsService,
            ILogger<IGroupCreateService> logger
        ) : base(dataContext)
        {
            _groupsService = groupsService;
            _contactsService = contactsService;
            _logger = logger;
        }

        public async Task<GroupViewModel> Create(Guid contactId, List<CreateGroupViewModel> groups)
        {
            ValidateGroups(contactId, groups);

            if (groups.Count == 1)
                return await CreateGroup(contactId, groups.Single());

            var safeGroups = new List<SafeGroupModel>();
            foreach (var group in groups)
            {
                safeGroups.Add(await TryCreateGroup(contactId, group));
            }

            if (safeGroups.Any(_ => _.FailedGroup != null))
            {
                //
                var failedEmails = safeGroups
                    .Select(_ => _.FailedGroup)
                    .Where(g => g != null)
                    .SelectMany(g => g.Participants)
                    .Select(p => p.Email);
                throw new Exception($"Unable create groups for emails: {String.Join(", ", failedEmails)}");
            }

            return safeGroups.LastOrDefault(g => g.Group != null)?.Group;
        }

        private async Task<SafeGroupModel> TryCreateGroup(Guid contactId, CreateGroupViewModel group)
        {
            var result = new SafeGroupModel();
            try
            {
                result.Group = await CreateGroup(contactId, group);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Group create failed.");
                result.FailedGroup = group;
            }

            return result;
        }

        private async Task<GroupViewModel> CreateGroup(Guid contactId, CreateGroupViewModel group)
        {
            var result = TryFindExistingOneToOneGroup(contactId, group);
            if (result != null && !result.DateMarkedForDeletion.HasValue)
                return _groupsService.GetById(contactId, result.Id);

            result ??= await CreateDbGroup(contactId, group);
            if (result.DateMarkedForDeletion.HasValue && result.IsOneToOne)
                await RecreateOneToOneGroup(result);

            return _groupsService.GetById(contactId, result.Id);
        }

        private async Task<ChatGroup> CreateDbGroup(Guid contactId, CreateGroupViewModel group)
        {
            var dbGroup = new ChatGroup
            {
                Id = Guid.NewGuid(),
                Title = !group.IsOneToOne ? group.Title : null,
                InitiatorContactId = contactId,
                IsOneToOne = group.IsOneToOne,
                AvatarImageId = !group.IsOneToOne ? group.AvatarImageId : null,
                Participants = new List<ChatGroupParticipant>()
            };
            await db.ChatGroups.AddAsync(dbGroup);

            // participants emails
            var rawEmails = group.Participants.Select(x => x.Email).Distinct().ToList();
            var participantContactIds =
                _contactsService.EnsureContactsInvitedByEmails(contactId, dbGroup.Id, rawEmails)
                .Select(x => x.Id).ToList();

            // Add currentContact to participants, if it didn't done early
            if (!participantContactIds.Contains(contactId))
                participantContactIds.Add(contactId);

            var participants = participantContactIds.Select(id => new ChatGroupParticipant
            {
                Id = Guid.NewGuid(),
                IsActive = true,
                IsDeniedVideo = false,
                ContactId = id,
                ChatGroupId = dbGroup.Id,
                ConfirmationStateByInitiator = ConfirmationState.Confirmed
            });

            await db.ChatGroupParticipants.AddRangeAsync(participants);
            await db.SaveChangesAsync();

            return dbGroup;
        }

        private void ValidateGroups(Guid currentContactId, List<CreateGroupViewModel> groups)
        {
            var contact = db.Contacts
                .AsNoTracking()
                .Single(x => x.Id == currentContactId);

            var contactEmails = groups.SelectMany(g => g.Participants)
                .Select(x => x.Email);
            var temporaryContactEmails = db.Contacts
                .Where(x => contactEmails.Contains(x.Email) && x.Type == ContactType.Temporary)
                .Select(x => x.Email)
                .AsNoTracking();

            if (temporaryContactEmails.Any())
                throw new Exception($"With temporary contacts {String.Join(", ", temporaryContactEmails)} it is not possible to create ChatGroups");

            foreach (var group in groups)
            {
                VerifyIfGroupIsNotWithUserItself(contact, group);
            }
        }

        private void VerifyIfGroupIsNotWithUserItself(Contact currentContact, CreateGroupViewModel group)
        {
            if (group.IsOneToOne && group.Participants.Count == 1
                && string.Equals(group.Participants[0].Email, currentContact.Email, StringComparison.OrdinalIgnoreCase))
                throw new Exception("Creating chat with yourself is not supported");
        }

        private ChatGroup TryFindExistingOneToOneGroup(Guid contactId, CreateGroupViewModel group)
        {
            // check if this is group 1-1 try find existing
            if (group.IsOneToOne && group.Participants.Count == 1)
            {
                var participant = group.Participants.Single();
                var participantEmail = participant.Email;
                var participantUser = db.Contacts
                    .FirstOrDefault(x => x.Email == participantEmail);
                return participantUser == null
                    ? null
                    : FindPrivateGroup(contactId, participantUser.Id);
            }

            // one to one group can be created with just single other participant
            group.IsOneToOne = false;

            return null;
        }

        private ChatGroup FindPrivateGroup(Guid contact1, Guid contact2)
        {
            return (
                from g in db.QueryGroup()
                join gp1 in db.ChatGroupParticipants on g.Id equals gp1.ChatGroupId
                join gp2 in db.ChatGroupParticipants on g.Id equals gp2.ChatGroupId
                where g.IsOneToOne
                      && gp1.ContactId == contact1
                      && gp2.ContactId == contact2
                      && g.Participants.Count == 2
                select g
            ).FirstOrDefault();
        }

        private async Task RecreateOneToOneGroup(ChatGroup group)
        {
            group.DateMarkedForDeletion = null;
            group.MarkedForDeletionContactId = null;

            await db.SaveChangesAsync();
        }
    }
}
