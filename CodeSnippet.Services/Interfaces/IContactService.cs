using CodeSnippet.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeSnippet.Services.Interfaces
{
    public interface IContactService
    {
        List<ContactViewModel> EnsureContactsInvitedByEmails(Guid currentContactId, Guid groupId, List<string> emails, bool sendEmailInvitation = true);
    }
}
