using CodeSnippet.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace CodeSnippet.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DataContext()
        {

        }
        
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ChatGroup> ChatGroups { get; set; }
        public DbSet<ChatGroupParticipant> ChatGroupParticipants { get; set; }
    }
}
