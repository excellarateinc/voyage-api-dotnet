namespace Launchpad.Web
{
    public static class Constants
    {
        public static string ApplicationName = "Launchpad .Net API";
        public static class RoutePrefixes
        {
            public const string V1 = "api/v1";
            public const string V2 = "api/v2";
            public const string Account = "api/account";
            public const string Role = "api";
            public const string User = "api";
        }

        public static class LssClaims
        {
            public const string Type = "lss.permission";

            public const string AssignRole = "assign.role";
            public const string CreateRole = "create.role";
            public const string ListRoles = "list.roles";

            public const string ListUsers = "list.users";
            public const string ListUserClaims = "list.user-claims";

            public const string CreateClaim = "create.claim";

            public const string ListWidgets = "list.widgets";

           
        }
    }
}