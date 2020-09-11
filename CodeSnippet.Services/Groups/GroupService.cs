using CodeSnippet.Data;
using CodeSnippet.Data.Models;
using CodeSnippet.Models;
using CodeSnippet.Services.Base;
using CodeSnippet.Services.Extensions;
using CodeSnippet.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeSnippet.Services.Groups
{
    public class GroupService : ServiceBase, IGroupService
    {
        public GroupService(DataContext dataContext) : base(dataContext)
        { }

        public GroupViewModel GetById(Guid contactId, Guid groupId)
        {
            return QueryGroups(contactId, groupId).FirstOrDefault();
        }

        private List<GroupViewModel> QueryGroups(Guid contactId, Guid? groupId = null)
        {
            var contact = db.Contacts.Single(x => x.Id == contactId);

            //load models from sql
            var groups = db.ChatGroups
                .Where(g => g.Id == groupId)
                .AsNoTracking();
            var chatParticipants = db.ChatGroupParticipants
                .Where(p => p.ChatGroupId == groupId)
                .AsNoTracking();

            var chatParticipantList = chatParticipants.ToLookup(_ => _.ChatGroupId);

            return groups
                .AsEnumerable()
                .Select(g =>
                {
                    return Map(g, chatParticipantList[g.Id], contact);
                })
                .ToList();
        }

        private static GroupViewModel Map(
            ChatGroup group,
            IEnumerable<ChatGroupParticipant> chatParticipants,
            Contact contact)
        {
            var isTemporaryRoom = group.Type == GroupType.Temporary;
            var isInitiator = group.InitiatorContactId == contact.Id;

            return new GroupViewModel
            {
                Id = group.Id,
                Title = group.FormatGroupTitle(chatParticipants, contact.Id),
                AvatarImageId = group.FormatGroupAvatar(chatParticipants, contact.Id),
                Participants = chatParticipants
                    .Where(x => group.Type == GroupType.Regular || x.ContactId == contact.Id)
                    .Select(x => x.Map())
                    .ToList(),
                IsOneToOne = group.IsOneToOne,
                Type = group.Type,
                AccessRight = isTemporaryRoom ?
                    new AccessRights(!isInitiator, isInitiator, isInitiator, isInitiator, isInitiator, isInitiator) :
                    new AccessRights(!isInitiator && !group.IsOneToOne, isInitiator || group.IsOneToOne),
                InitiatorContactId = group.InitiatorContactId,
                LastUpdatedDate = group.UpdatedOn
            };
        }
    }
}
