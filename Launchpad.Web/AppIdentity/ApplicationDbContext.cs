using Microsoft.AspNet.Identity.EntityFramework;

namespace Launchpad.Web.AppIdentity
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(string connectionString) 
            : base(connectionString, throwIfV1Schema: true)
        {

        }
    }
}