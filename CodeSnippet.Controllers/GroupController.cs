using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Net;
using CodeSnippet.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CodeSnippet.Models;
using Autofac;

namespace CodeSnippet.Controllers
{
    [Authorize]
    [ApiController, Route("groups")]
    public class GroupsController : ApiControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IGroupCreateService _groupCreateService;

        public GroupsController(ILifetimeScope scope,
            IGroupService groupService,
            IGroupCreateService groupCreateService) : base(scope)
        {
            _groupService = groupService;
            _groupCreateService = groupCreateService;
        }

        /// <summary>
        /// Get group info by id
        /// </summary>
        /// <param name="id">Group id</param>
        /// <response code="200">OK</response>
        [HttpGet("{contactId}/{id}")]
        public ActionResult<GroupViewModel> Get(Guid id, Guid contactId)
        {
            return _groupService.GetById(contactId, id);
        }

        /// <summary>
        /// Create group (regular) or contact (one-by-one). For regular group (IsOneToOne = false) field 'Title' is required, also Group participants should containe at least one participant
        /// </summary>
        /// <param name="group">created fields</param>
        /// <returns></returns>
        [HttpPost("{contactId}")]
        public async Task<ActionResult<GroupViewModel>> Create(CreateGroupViewModel group, Guid contactId)
        {

            return await _groupCreateService.Create(contactId, new List<CreateGroupViewModel>() { group });
        }

        /// <summary>
        /// Create list of contacts (one-by-one).
        /// </summary>
        /// <param name="groupList">Created fields</param>
        /// <returns></returns>
        [HttpPost("{contactId}/list")]
        public async Task<ActionResult<GroupViewModel>> CreateContactList(CreateContactListViewModel groupList, Guid contactId)
        {
            return await _groupCreateService.Create(contactId, groupList.Groups);
        }

        public class CreateContactListViewModel
        {
            [MinLength(1, ErrorMessage = "You should add at least one group.")]
            public List<CreateGroupViewModel> Groups { get; set; } = new List<CreateGroupViewModel>();
        }
    }
}
