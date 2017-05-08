using System.Linq;

namespace Voyage.Data.Repositories.RolePermission
{
    public interface IRolePermissionRepository : IRepository<Models.Entities.RolePermission>
    {
        IQueryable<Models.Entities.RolePermission> GetPermissionsByRole(string roleName);

        Models.Entities.RolePermission GetByRoleAndPermission(string roleName, string permissionType, string permissionValue);
    }
}
