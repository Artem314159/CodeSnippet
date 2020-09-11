using CodeSnippet.Data;
using CodeSnippet.Data.Models;
using CodeSnippet.Models;
using CodeSnippet.Services.Base;
using CodeSnippet.Services.Extensions;
using CodeSnippet.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSnippet.Services.Contacts
{
    public class ContactService : ServiceBase, IContactService
    {
        public ContactService(DataContext dataContext) : base(dataContext)
        { }

        public List<ContactViewModel> EnsureContactsInvitedByEmails(Guid currentContactId, Guid groupId,
            List<string> emails, bool sendEmailInvitation = true)
        {
            var currentContact = db.Contacts
                .FirstOrDefault(x => x.Id == currentContactId);

            if (currentContact == null)
                return null;

            var dbContacts = db.Contacts
                .Where(x => emails.Contains(x.Email))
                .Distinct()
                .ToList();

            List<(string mail, bool isExist)> missingEmailsTuple = emails
                .Where(e => dbContacts.All(x =>
                    (string.Compare(x.Email, e, StringComparison.OrdinalIgnoreCase) != 0 ||
                     x.LastActivity == DateTime.MinValue) && x.Type == ContactType.Regular))
                .Select(x =>
                (
                    x,
                    dbContacts.Any(c => c.Email.Equals(x, StringComparison.OrdinalIgnoreCase))
                ))
                .Distinct()
                .ToList();

            var result = dbContacts
                .Select(x => x.MapToViewModel(groupId))
                .ToList();
            foreach (var missingContact in missingEmailsTuple)
            {
                //code for email invitation of missing contacts
            }

            return result.Distinct().ToList();
        }
    }
}
