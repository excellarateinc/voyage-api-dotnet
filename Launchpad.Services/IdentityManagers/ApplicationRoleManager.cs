using Voyage.Models.Entities;
using Microsoft.AspNet.Identity;

namespace Voyage.Services.IdentityManagers
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole, string> store)
            : base(store)
        {
        }
    }
}
