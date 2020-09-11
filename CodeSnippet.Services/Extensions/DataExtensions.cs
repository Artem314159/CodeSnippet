using CodeSnippet.Data;
using CodeSnippet.Data.Models;
using CodeSnippet.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeSnippet.Services.Extensions
{
    public static partial class DataExtensions
    {
        public static IQueryable<ChatGroup> QueryGroup(this DataContext db) =>
            db.ChatGroups
                .Include(x => x.Participants)
                    .ThenInclude(x => x.Contact);

        public static GroupParticipantViewModel Map(this ChatGroupParticipant p)
        {
            return new GroupParticipantViewModel()
            {
                Id = p.Id,
                ContactId = p.ContactId,
                Name = GetDisplayName(p.Contact.DisplayName, p.Contact.Email),
                IsActive = p.IsActive,
                AvatarImageId = p.Contact.AvatarImageId,
                ChatGroupId = p.ChatGroupId,
                Email = p.Contact.Email,
                IsAllowVideo = !p.IsDeniedVideo,
            };
        }

        public static Guid? FormatGroupAvatar(this ChatGroup group, IEnumerable<ChatGroupParticipant> chatParticipants, Guid contactId)
        {
            if (group.IsOneToOne)
                return chatParticipants.FirstOrDefault(x => x?.ContactId != contactId)?.Contact.AvatarImageId;

            return group.AvatarImageId;
        }

        public static string FormatGroupTitle(this ChatGroup group, IEnumerable<ChatGroupParticipant> chatParticipants, Guid contactId)
        {
            if (string.IsNullOrEmpty(group.Title) || group.IsOneToOne)
                return string.Join(", ", chatParticipants.Where(x => x.IsActive && x.ContactId != contactId)
                    .Select(x => GetDisplayName(x.Contact.DisplayName, x.Contact.Email)));

            return group.Title;
        }

        public static string GetDisplayName(string displayName, string email) => string.IsNullOrWhiteSpace(displayName) ? email : displayName;

        public static ContactViewModel MapToViewModel(this Contact contact, Guid? oneToOneGroupId = null, bool isFavorite = false)
        {
            return new ContactViewModel
            {
                Id = contact.Id,
                PrivateChatGroupId = oneToOneGroupId,
                Email = contact.Email,
                DisplayName = GetDisplayName(contact.DisplayName, contact.Email),
                HasAccount = contact.UserId != null,
                IsFavorite = isFavorite,
                AvatarImageId = contact.AvatarImageId,
                LastActivity = contact.LastActivity,
                IsTemporaryRoomVisitor = contact.Type == ContactType.Temporary
            };
        }
    }
}
