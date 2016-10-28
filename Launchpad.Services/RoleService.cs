using Launchpad.Services.Interfaces;
using System.Threading.Tasks;
using Launchpad.Models;
using Launchpad.Services.IdentityManagers;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Launchpad.Core;
using Launchpad.Models.EntityFramework;

namespace Launchpad.Services
{
    public class RoleService : IRoleService
    {
        private ApplicationRoleManager _roleManager;

        public RoleService(ApplicationRoleManager roleManager)
        {
            _roleManager = roleManager.ThrowIfNull(nameof(roleManager));
        }

        public async Task<IdentityResult> CreateRoleAsync(RoleModel model)
        {
            var role = new ApplicationRole();
            role.Name = model.Name;

            var result = await _roleManager.CreateAsync(role);

            return result;
        }
    }
}
