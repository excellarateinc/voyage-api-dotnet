using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Voyage.Models.Entities
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }

        public virtual ICollection<RoleClaim> Claims { get; set; }
    }
}
