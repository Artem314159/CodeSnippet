using CodeSnippet.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSnippet.Services.Interfaces
{
    public interface IGroupService
    {
        GroupViewModel GetById(Guid contactId, Guid groupId);
    }
}
