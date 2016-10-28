using Launchpad.Services.Interfaces;
using System.Threading.Tasks;
using Launchpad.Models;
using Launchpad.Services.IdentityManagers;
using Microsoft.AspNet.Identity;
using Launchpad.Core;
using Launchpad.Models.EntityFramework;
using Launchpad.Data.Interfaces;

namespace Launchpad.Services
{
    public class RoleService : IRoleService
    {
        private ApplicationRoleManager _roleManager;
        private IRoleClaimRepository _roleClaimRepository;


        public RoleService(ApplicationRoleManager roleManager, IRoleClaimRepository roleClaimRepository)
        {
            _roleManager = roleManager.ThrowIfNull(nameof(roleManager));
            _roleClaimRepository = roleClaimRepository.ThrowIfNull(nameof(roleClaimRepository));
        }

        public async Task AddClaimAsync(RoleModel role, ClaimModel claim)
        {
            var roleEntity = await _roleManager.FindByNameAsync(role.Name);
            if(roleEntity != null)
            {
                _roleClaimRepository.Add(new RoleClaim { RoleId = roleEntity.Id, ClaimValue = claim.ClaimValue, ClaimType = claim.ClaimType });   
            }
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
