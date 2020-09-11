using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CodeSnippet.Data.Models
{
    public class Contact
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(256)]
        [Required]
        public string Email { get; set; }

        [StringLength(256)]
        public string DisplayName { get; set; }

        public Guid? AvatarImageId { get; set; }

        public Guid? BackupAvatarImageId { get; set; }

        public Guid? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        public DateTime LastActivity { get; set; }

        public int LastActivitySyncStamp { get; set; }

        public int SyncStamp { get; set; }

        public ContactType Type { get; set; }

        public virtual ICollection<ChatGroup> InitiatedChatGroups { get; set; }
        public virtual ICollection<ChatGroup> MarkedForDeletionChatGroups { get; set; }
        public virtual ICollection<ChatGroupParticipant> ChatGroupParticipants { get; set; }
    }

    public enum ContactType
    {
        Regular,
        Temporary
    }
}
