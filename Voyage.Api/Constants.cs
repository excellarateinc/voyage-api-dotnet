namespace Voyage.Api
{
    public static class Constants
    {
        public static string ApplicationName = "Voyage .Net API";

        public static class RoutePrefixes
        {
            public const string V1 = "api/v1";
        }

        public static class AppPermissions
        {
            public const string Type = "app.permission";

            public const string AssignRole = "assign.role";
            public const string CreateRole = "create.role";
            public const string DeleteRole = "delete.role";
            public const string ListRoles = "list.roles";
            public const string RevokeRole = "revoke.role";
            public const string ViewRole = "view.role";

            public const string ListUsers = "list.users";
            public const string ListUserPermissions = "list.user-permissions";
            public const string ViewUser = "view.user";
            public const string UpdateUser = "update.user";
            public const string DeleteUser = "delete.user";
            public const string CreateUser = "create.user";

            public const string DeleteRolePermission = "delete.role-permission";
            public const string CreatePermission = "create.permission";
            public const string ViewPermission = "view.permission";
            public const string ListRolePermissions = "list.role-permissions";
        }
    }
}
