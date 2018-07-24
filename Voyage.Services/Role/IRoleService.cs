using System.Collections.Generic;
using System.Threading.Tasks;
using Voyage.Models;
using Microsoft.AspNet.Identity;

namespace Voyage.Services.Role
{
    public interface IRoleService
    {
        Task<IEnumerable<ClaimModel>> GetRoleClaimsByRoleIdAsync(string id);

        Task<RoleModel> GetRoleByNameAsync(string name);

        RoleModel GetRoleById(string id);

        Task<RoleModel> CreateRoleAsync(RoleModel model);

        Task<IEnumerable<RoleModel>> GetRolesAsync();

        Task<IEnumerable<ClaimModel>> GetRoleClaimsAsync(string name);

        Task<ClaimModel> AddClaimAsync(string roleId, ClaimModel claim);

        Task<IdentityResult> RemoveRoleAsync(string roleId);

        Task RemoveClaimAsync(string roleId, int claimId);

        Task<ClaimModel> GetClaimByIdAsync(string roleId, int claimId);
    }
}
