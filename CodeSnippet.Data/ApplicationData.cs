using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSnippet.Data
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string DisplayName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int SyncStamp { get; set; }
    }

    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole() { }
        public ApplicationRole(string name) { Name = name; }
    }
}
