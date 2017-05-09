namespace Voyage.Models
{
    public class AppClaims
    {
        public const string Type = "authorities";

        public const string CreateRole = "api.roles.create";
        public const string DeleteRole = "api.roles.delete";
        public const string ListRoles = "api.roles.list";
        public const string ViewRole = "api.roles.get";
        public const string UpdateRole = "api.roles.update";

        public const string AddRolePermission = "api.roles.permission.add";
        public const string DeleteRolePermission = "api.roles.permission.delete";
        public const string ListRolePermission = "api.roles.permission.list";
        public const string GetRolePermission = "api.roles.permission.get";

        public const string CreatePermission = "api.permission.create";
        public const string DeletePermission = "api.permission.delete";
        public const string ViewPermission = "api.permission.get";
        public const string ListPermission = "api.permission.list";

        public const string ListUserPermissions = "api.users.permissions.list";

        public const string AssignUserRole = "api.users.roles.assign";
        public const string CreateUserRole = "api.users.roles.create";
        public const string DeleteUserRole = "api.users.roles.delete";
        public const string ViewUserRole = "api.users.roles.get";
        public const string ListUserRole = "api.users.roles.list";

        public const string ListUsers = "api.users.list";
        public const string ViewUser = "api.users.get";
        public const string UpdateUser = "api.users.update";
        public const string DeleteUser = "api.users.delete";
        public const string CreateUser = "api.users.create";
    }
}
