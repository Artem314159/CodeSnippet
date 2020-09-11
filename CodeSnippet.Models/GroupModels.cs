using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CodeSnippet.Models
{
    public class GroupViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsOneToOne { get; set; }
        public Guid? AvatarImageId { get; set; }
        public List<GroupParticipantViewModel> Participants { get; set; } = new List<GroupParticipantViewModel>();
        public AccessRights AccessRight { get; set; }
        public Guid InitiatorContactId { get; set; }
        public GroupType Type { get; set; }
        public bool IsLoading { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }

    public class GroupParticipantViewModel
    {
        public Guid Id { get; set; }
        public Guid ContactId { get; set; }
        public string Name { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsActive { get; set; }
        public Guid? AvatarImageId { get; set; }
        public Guid ChatGroupId { get; set; }
        public string Email { get; set; }
        public bool IsAllowVideo { get; set; }
    }

    public enum GroupType
    {
        Regular,
        Temporary
    }

    public class GroupListViewModel
    {
        public IEnumerable<GroupViewModel> Groups { get; set; }
    }

    public class SafeGroupModel
    {
        public GroupViewModel Group { get; set; }
        public CreateGroupViewModel FailedGroup { get; set; }
    }

    public class CreateGroupViewModel
    {
        [MaxLength(256, ErrorMessage = "Maximum field length exceeded")]
        public string Title { get; set; }

        public Guid? AvatarImageId { get; set; }
        public bool IsOneToOne { get; set; }

        [MinLength(1, ErrorMessage = "You should add at least one participant.")]
        public List<GroupParticipantEmailViewModel> Participants { get; set; } = new List<GroupParticipantEmailViewModel>();
    }

    public class GroupParticipantEmailViewModel
    {
        [EmailAddress]
        [MaxLength(256, ErrorMessage = "Maximum field length exceeded")]
        public string Email { get; set; }
    }
}
