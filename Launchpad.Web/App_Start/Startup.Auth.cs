using Autofac;
using FluentValidation.WebApi;
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
            #region Container and Route Configuration
            var httpConfig = new HttpConfiguration();

            // Build the container
            ContainerConfig.Register(httpConfig);

            // Configure API 
            WebApiConfig.Register(httpConfig);

            // configure FluentValidation model validator provider
            FluentValidationModelValidatorProvider.Configure(httpConfig);

            #endregion

            #region Arrange Pipeline
            // 1. Use the autofac scope for owin 
            app.UseAutofacLifetimeScopeInjector(ContainerConfig.Container);

            // 2. Allow cors requests
            app.UseCors(CorsOptions.AllowAll);

            // 3. Use the readable response middleware
            app.Use<RewindResponseMiddleware>();

            // 4. Register the activty auditing here so that anonymous activity is captured
            app.UseMiddlewareFromContainer<ActivityAuditMiddleware>();

            // 5. Configure oAuth
            var oauthProvider = ContainerConfig.Container.Resolve<ApplicationOAuthProvider>();
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/api/v1/login"),
                Provider = oauthProvider,
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),

                // If the config is wrong, let the application crash
                AccessTokenExpireTimeSpan = TimeSpan.FromSeconds(int.Parse(ConfigurationManager.AppSettings["oAuth:TokenExpireSeconds"])),
                // In production mode set AllowInsecureHttp = false
                AllowInsecureHttp = bool.Parse(ConfigurationManager.AppSettings["oAuth:AllowInsecureHttp"]),
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            // 6. Add web api to pipeline
            app.UseWebApi(httpConfig);

            #endregion 
        }
    }
}