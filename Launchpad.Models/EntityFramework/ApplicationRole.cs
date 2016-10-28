using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace Launchpad.Models.EntityFramework
{
    public class ApplicationRole : IdentityRole
    {
        public virtual ICollection<RoleClaim> Claims { get; set; }
    }
}
