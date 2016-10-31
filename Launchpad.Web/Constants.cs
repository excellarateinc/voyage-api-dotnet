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
        }
    }
}