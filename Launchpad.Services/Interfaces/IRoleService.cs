using Launchpad.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Launchpad.Services.Interfaces
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
