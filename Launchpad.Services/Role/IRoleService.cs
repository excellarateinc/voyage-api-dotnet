using System.Collections.Generic;
using System.Threading.Tasks;
using Voyage.Models;
using Microsoft.AspNet.Identity;

namespace Voyage.Services.Role
{
    public interface IRoleService
    {
        IEnumerable<ClaimModel> GetRoleClaimsByRoleId(string id);

        RoleModel GetRoleByName(string name);

        RoleModel GetRoleById(string id);

        Task<RoleModel> CreateRoleAsync(RoleModel model);

        IEnumerable<RoleModel> GetRoles();

        IEnumerable<ClaimModel> GetRoleClaims(string name);

        Task<ClaimModel> AddClaimAsync(string roleId, ClaimModel claim);

        Task<IdentityResult> RemoveRoleAsync(string roleId);

        void RemoveClaim(string roleId, int claimId);

        ClaimModel GetClaimById(string roleId, int claimId);
    }
}
