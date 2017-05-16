using System;
using System.Configuration;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Voyage.Core;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security;

namespace Voyage.Security.Oauth2
{
    public static class VoyageOauth2
    {
        /// <summary>
        /// Enable Voyage Oauth2
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IAppBuilder UseVoyageOauth2(this IAppBuilder app, VoyageOauth2Configuration configuration)
        {
            // Enable Application Sign In Cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Application",
                AuthenticationMode = AuthenticationMode.Passive,
                LoginPath = new PathString(Paths.LoginPath),
                LogoutPath = new PathString(Paths.LogoutPath),
            });

            // Enable External Sign In Cookie
            app.SetDefaultSignInAsAuthenticationType("External");
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "External",
                AuthenticationMode = AuthenticationMode.Passive,
                CookieName = CookieAuthenticationDefaults.CookiePrefix + "External",
                ExpireTimeSpan = TimeSpan.FromMinutes(5),
            });

            // Check custom authorize endpoint provided by consummer
            var authorizePath = Paths.AuthorizePath;
            if (!string.IsNullOrWhiteSpace(configuration.AuthorizeEndpointPath))
                authorizePath = configuration.AuthorizeEndpointPath;

            // Check custom token endpoint provided by consummer
            var tokenPath = Paths.TokenPath;
            if (!string.IsNullOrWhiteSpace(configuration.TokenEndpointPath))
                tokenPath = configuration.TokenEndpointPath;

            // Setup Authorization Server
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                // Authorize path
                AuthorizeEndpointPath = new PathString(authorizePath),

                // Token path
                TokenEndpointPath = new PathString(tokenPath),

                // Show error
                ApplicationCanDisplayErrors = true,

                // If the config is wrong, let the application crash
                AccessTokenExpireTimeSpan = TimeSpan.FromSeconds(int.Parse(configuration.TokenExpireSeconds)),

                // In production mode set AllowInsecureHttp = false
                AllowInsecureHttp = configuration.AllowInsecureHttp,

                // Authorization server provider which controls the lifecycle of Authorization Server
                Provider = new VoyageJwtProvider(),

                // Jwt custom format
                AccessTokenFormat = new VoyageJwtFormat(configuration.Container),

            });

            return app;
        }
    }
}