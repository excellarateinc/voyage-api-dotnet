using System.Linq;

namespace Voyage.Data.Repositories.RolePermission
{
    public class RolePermissionRepository : BaseRepository<Models.Entities.RolePermission>, IRolePermissionRepository
    {
        public RolePermissionRepository(IVoyageDataContext context)
            : base(context)
        {
        }

        public override Models.Entities.RolePermission Add(Models.Entities.RolePermission model)
        {
            Context.RolePermissions.Add(model);
            Context.SaveChanges();
            return model;
        }

        public override void Delete(object id)
        {
            var entity = Get(id);
            if (entity != null)
            {
                Context.RolePermissions.Remove(entity);
                Context.SaveChanges();
            }
        }

        public override Models.Entities.RolePermission Get(object id)
        {
            return Context.RolePermissions.Find(id);
        }

        public override IQueryable<Models.Entities.RolePermission> GetAll()
        {
            return Context.RolePermissions;
        }

        public Models.Entities.RolePermission GetByRoleAndPermission(string roleName, string permissionType, string permissionValue)
        {
            return Context.RolePermissions
                     .FirstOrDefault(_ => _.Role.Name == roleName && _.PermissionType == permissionType && _.PermissionValue == permissionValue);
        }

        public IQueryable<Models.Entities.RolePermission> GetPermissionsByRole(string roleName)
        {
            return Context.RolePermissions
                     .Where(_ => _.Role.Name == roleName);
        }

        public override Models.Entities.RolePermission Update(Models.Entities.RolePermission model)
        {
            Context.SaveChanges();
            return model;
        }
    }
}
