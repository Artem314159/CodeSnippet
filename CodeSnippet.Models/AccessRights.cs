namespace CodeSnippet.Models
{
    public class AccessRights
    {
        public AccessRights() { }

        public AccessRights(bool canLeave, bool canDelete)
            : this(canLeave, canDelete, false, false, false, false)
        {
        }

        public AccessRights(bool canLeave, bool canDelete, bool canExtendTime, bool canDeleteParticipants, bool canDisableVideo, bool canConfirm)
        {
            CanLeaveChat = canLeave;
            CanDeleteChat = canDelete;
            CanExtendTime = canExtendTime;
            CanDeleteParticipants = canDeleteParticipants;
            CanDisableVideo = canDisableVideo;
            CanConfirmToJoin = canConfirm;
        }

        public bool CanLeaveChat { get; set; }
        public bool CanDeleteChat { get; set; }
        public bool CanExtendTime { get; set; }
        public bool CanDeleteParticipants { get; set; }
        public bool CanDisableVideo { get; set; }
        public bool CanConfirmToJoin { get; set; }
    }
}
