namespace Voyage.Web.Auth_Stuff
{
    public static class Paths
    {
        /// <summary>
        /// AuthorizationServer.
        /// </summary>
        public const string AuthorizationServerBaseAddress = "http://localhost:52431";

        /// <summary>
        /// ResourceServer.
        /// </summary>
        public const string ResourceServerBaseAddress = "http://localhost:52431";

        /// <summary>
        /// ImplicitGrant.
        /// </summary>
        public const string ImplicitGrantCallBackPath = "http://localhost:52431/Home/Index";

        /// <summary>
        /// AuthorizationCodeGrant.
        /// </summary>
        public const string AuthorizeCodeCallBackPath = "http://localhost:52431/Home/Index";

        public const string AuthorizePath = "/OAuth/Authorize";
        public const string TokenPath = "/OAuth/Token";
        public const string LoginPath = "/Account/Login";
        public const string LogoutPath = "/Account/Logout";
        public const string MePath = "/api/Me";
    }
}