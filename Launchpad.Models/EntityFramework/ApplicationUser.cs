using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Launchpad.Models.EntityFramework
{
    /// <summary>
    /// Represents an application user. Properties placed on this class will be persisted to the user table
    /// </summary>
    public class ApplicationUser : IdentityUser, ISoftDeleteable
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual ICollection<UserPhone> Phones { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }
    }
}
