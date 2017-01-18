namespace Launchpad.Web
{
    public static class Constants
    {
#pragma warning disable SA1401 // Fields must be private
        public static string ApplicationName = "Launchpad .Net API";
#pragma warning restore SA1401 // Fields must be private

        public static class RoutePrefixes
        {
            public const string V1 = "api/v1";
        }

        public static class LssClaims
        {
            public const string Type = "lss.permission";

            public const string AssignRole = "assign.role";
            public const string CreateRole = "create.role";
            public const string DeleteRole = "delete.role";
            public const string ListRoles = "list.roles";
            public const string RevokeRole = "revoke.role";
            public const string ViewRole = "view.role";

            public const string ListUsers = "list.users";
            public const string ListUserClaims = "list.user-claims";
            public const string ViewUser = "view.user";
            public const string UpdateUser = "update.user";
            public const string DeleteUser = "delete.user";
            public const string CreateUser = "create.user";

            public const string DeleteRoleClaim = "delete.role-claim";
            public const string CreateClaim = "create.claim";
            public const string ViewClaim = "view.claim";
            public const string ListRoleClaims = "list.role-claims";
        }
    }
}
