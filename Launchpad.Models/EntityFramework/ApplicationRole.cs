using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Launchpad.Models.EntityFramework
{
    public class ApplicationRole : IdentityRole
    {
        public virtual ICollection<RoleClaim> Claims { get; set; }
    }
}
