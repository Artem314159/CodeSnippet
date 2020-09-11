using CodeSnippet.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeSnippet.Services.Interfaces
{
    public interface IGroupCreateService
    {
        Task<GroupViewModel> Create(Guid contactId, List<CreateGroupViewModel> groups);
    }
}
