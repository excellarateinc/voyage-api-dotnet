using Microsoft.AspNet.Identity.EntityFramework;

namespace Launchpad.Models.EntityFramework
{
    /// <summary>
    /// Represents an application user. Properties placed on this class will be persisted to the user table
    /// </summary>
    public class ApplicationUser : IdentityUser 
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }

    }
}
