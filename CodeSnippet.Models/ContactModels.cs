using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSnippet.Models
{
    public class ContactViewModel
    {
        public Guid Id { get; set; }
        public Guid? PrivateChatGroupId { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public bool HasAccount { get; set; }
        public ContactActivityStatus Status { get; set; }
        public Guid? AvatarImageId { get; set; }
        public DateTime LastActivity { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsTemporaryRoomVisitor { get; set; }
    }

    public enum ContactActivityStatus
    {
        Offline = 0,
        Online = 1,
        Away = 2
    }
}
