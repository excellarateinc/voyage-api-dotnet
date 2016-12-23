using System.Collections.Generic;
using System.Threading.Tasks;
using Launchpad.Models;

namespace Launchpad.Services.Role
{
    public interface IRoleService
    {
        EntityResult<IEnumerable<ClaimModel>> GetRoleClaimsByRoleId(string id);

        EntityResult<RoleModel> GetRoleByName(string name);

        EntityResult<RoleModel> GetRoleById(string id);

        Task<EntityResult<RoleModel>> CreateRoleAsync(RoleModel model);

        EntityResult<IEnumerable<RoleModel>> GetRoles();

        EntityResult<IEnumerable<ClaimModel>> GetRoleClaims(string name);

        Task<EntityResult<ClaimModel>> AddClaimAsync(string roleId, ClaimModel claim);

        Task<EntityResult> RemoveRoleAsync(string roleId);

        EntityResult RemoveClaim(string roleId, int claimId);

        EntityResult<ClaimModel> GetClaimById(string roleId, int claimId);
    }
}
