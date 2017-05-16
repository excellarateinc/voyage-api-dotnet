using System;
using System.Configuration;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Voyage.Core;

namespace Voyage.Security.BasicToken
{
    public static class VoyageBasicToken
    {
        public static IAppBuilder UseVoyageOauth2(this IAppBuilder app, VoyageBasicTokenConfiguration configuration)
        {
            // Setup Authorization Server
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                // Token path
                TokenEndpointPath = new PathString(Paths.TokenPath),

                // Show error
                ApplicationCanDisplayErrors = true,

                // If the config is wrong, let the application crash
                AccessTokenExpireTimeSpan = TimeSpan.FromSeconds(int.Parse(ConfigurationManager.AppSettings["oAuth:TokenExpireSeconds"])),

                // In production mode set AllowInsecureHttp = false
                AllowInsecureHttp = bool.Parse(ConfigurationManager.AppSettings["oAuth:AllowInsecureHttp"]),

                // Authorization server provider which controls the lifecycle of Authorization Server
                Provider = new VoyageBasicTokenProvider()
            });

            return app;
        }
    }
}
