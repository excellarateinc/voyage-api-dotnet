using System.Collections.Generic;
using System.Threading.Tasks;
using Voyage.Models;
using Microsoft.AspNet.Identity;

namespace Voyage.Services.Role
{
    public interface IRoleService
    {
        IEnumerable<PermissionModel> GetRolePermissionsByRoleId(string id);

        RoleModel GetRoleByName(string name);

        RoleModel GetRoleById(string id);

        Task<RoleModel> CreateRoleAsync(RoleModel model);

        IEnumerable<RoleModel> GetRoles();

        IEnumerable<PermissionModel> GetRolePermissions(string name);

        Task<PermissionModel> AddPermissionAsync(string roleId, PermissionModel permission);

        Task<IdentityResult> RemoveRoleAsync(string roleId);

        void RemovePermission(string roleId, int permissionId);

        PermissionModel GetPermissionById(string roleId, int permissionId);
    }
}
