using Autofac;
using Launchpad.Web.App_Start;
using Launchpad.Web.AuthProviders;
using Launchpad.Web.Middleware;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Configuration;
using System.Web.Http;

namespace Launchpad.Web
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }
        public static string PublicClientId => "self"; // Configure the application for OAuth based flow
            
        
        public static void Configure(IAppBuilder app)
        {
            //Add autofac to the pipeline

            var httpConfig = new HttpConfiguration();
            

            ContainerConfig.Register(httpConfig);

            WebApiConfig.Register(httpConfig);

            app.UseAutofacLifetimeScopeInjector(ContainerConfig.Container);

            app.UseCors(CorsOptions.AllowAll);
            

             // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // app.UseCookieAuthentication(new CookieAuthenticationOptions());
            //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

           

            var oauthProvider = ContainerConfig.Container.Resolve<ApplicationOAuthProvider>();
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/api/v1/login"),
                Provider = oauthProvider,
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),

                //If the config is wrong, let the application crash
                AccessTokenExpireTimeSpan = TimeSpan.FromSeconds(int.Parse(ConfigurationManager.AppSettings["oAuth:TokenExpireSeconds"])),
                // In production mode set AllowInsecureHttp = false
                AllowInsecureHttp = bool.Parse(ConfigurationManager.AppSettings["oAuth:AllowInsecureHttp"]),
            };


            //Register the activty auditing here so that anonymous activity is captured
            app.UseMiddlewareFromContainer<ActivityAuditMiddleware>();


            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            app.UseWebApi(httpConfig);
        }
    }
}