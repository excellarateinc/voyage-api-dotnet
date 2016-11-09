using Launchpad.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Launchpad.Services.Interfaces
{
    public interface IRoleService
    {
        RoleModel GetRoleById(string id);

        Task<IdentityResult<RoleModel>> CreateRoleAsync(RoleModel model);

        IEnumerable<RoleModel> GetRoles();

        IEnumerable<ClaimModel> GetRoleClaims(string name);

        Task<ClaimModel> AddClaimAsync(string roleId, ClaimModel claim);

        Task<IdentityResult> RemoveRoleAsync(string roleId);

         void RemoveClaim(string roleId, int claimId);

        ClaimModel GetClaimById(string roleId, int claimId);
    }
}
