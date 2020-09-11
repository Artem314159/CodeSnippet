using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CodeSnippet.Data.Models
{
    public class ChatGroupParticipant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public Guid ChatGroupId { get; set; }
        public Guid ContactId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeniedVideo { get; set; }
        public ConfirmationState ConfirmationStateByInitiator { get; set; }
        [ForeignKey(nameof(ChatGroupId))]
        public virtual ChatGroup ChatGroup { get; set; }
        [ForeignKey(nameof(ContactId))]
        public virtual Contact Contact { get; set; }
    }

    public enum ConfirmationState
    {
        WaitingForConfirmation = 0,
        Confirmed = 1,
        Declined = 2
    }
}
