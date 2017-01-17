using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Launchpad.Models.Entities
{
    /// <summary>
    /// Represents an application user. Properties placed on this class will be persisted to the user table
    /// </summary>
    [Table("User")]
    public class ApplicationUser : IdentityUser, ISoftDeleteable
    {
        [MaxLength(128)]
        [Required]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(128)]
        public string LastName { get; set; }

        public virtual ICollection<UserPhone> Phones { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public bool Deleted { get; set; }
    }
}
