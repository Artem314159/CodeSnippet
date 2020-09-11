using CodeSnippet.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CodeSnippet.Data.Models
{
    public class ChatGroup
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(256)]
        public string Title { get; set; }

        public Guid InitiatorContactId { get; set; }

        public bool IsOneToOne { get; set; }

        public int SyncStamp { get; set; }

        public Guid? AvatarImageId { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// Avatar Id to be supplied to client until content for the referenced AvatarImage will be loaded. 
        /// Primarally, keeps previously uploaded/synced = available avatar Id
        /// </summary>
        public Guid? BackupAvatarImageId { get; set; }

        public DateTime? DateMarkedForDeletion { get; set; }

        public Guid? MarkedForDeletionContactId { get; set; }

        public DateTime? ChatDumpSentOn { get; set; }

        public GroupType Type { get; set; }

        [ForeignKey(nameof(InitiatorContactId))]
        public virtual Contact InitiatorContact { get; set; }

        [ForeignKey(nameof(MarkedForDeletionContactId))]
        public virtual Contact MarkedForDeletionContact { get; set; }

        public virtual ICollection<ChatGroupParticipant> Participants { get; set; }
    }
}
