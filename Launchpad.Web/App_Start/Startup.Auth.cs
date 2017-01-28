using Autofac;
using FluentValidation.WebApi;
using Launchpad.Web.AuthProviders;
using Launchpad.Web.Middleware;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using IdentityServer3.AccessTokenValidation;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Resources;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.InMemory;

namespace Launchpad.Web
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId => "self"; // Configure the application for OAuth based flow

        public static void Configure(IAppBuilder app)
        {
            var httpConfig = new HttpConfiguration();

            // Build the container
            ContainerConfig.Register(httpConfig);

            // Configure API
            WebApiConfig.Register(httpConfig);

            // configure FluentValidation model validator provider
            FluentValidationModelValidatorProvider.Configure(httpConfig);

            // 1. Use the autofac scope for owin
            app.UseAutofacLifetimeScopeInjector(ContainerConfig.Container);

            // 2. Allow cors requests
            app.UseCors(CorsOptions.AllowAll);

            // 3. Use the readable response middleware
            app.Use<RewindResponseMiddleware>();

            // 4. Register the activty auditing here so that anonymous activity is captured
            app.UseMiddlewareFromContainer<ActivityAuditMiddleware>();

            var factory = new IdentityServerServiceFactory()
                .UseInMemoryClients(IdentityServerHelpers.GetClients())
                .UseInMemoryScopes(IdentityServerHelpers.GetScopes());
            factory.UserService = new Registration<IUserService>(typeof(IdentityServerUserService));

            // 5. Configure oAuth
            app.Map("/OAuth", idsrvApp =>
            {
                idsrvApp.UseIdentityServer(new IdentityServerOptions
                {
                    SiteName = "Embedded IdentityServer",
                    RequireSsl = false,
                    SigningCertificate = LoadCertificate(),
                    Factory = factory
                });
            });

            // Accept access tokens from IdentityServer.
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "http://localhost:52431/OAuth",
                ValidationMode = ValidationMode.ValidationEndpoint
            });

            // 6. Add web api to pipeline
            app.UseWebApi(httpConfig);
        }

        private static X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\idsrv3test.pfx"), "idsrv3test");
        }
    }

#pragma warning disable SA1402 // File may only contain a single class
    public class MiddlewareUrlRewriter : OwinMiddleware
#pragma warning restore SA1402 // File may only contain a single class
    {
        public MiddlewareUrlRewriter(OwinMiddleware next)
            : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            if (context.Request.Path.ToString().Contains("/token"))
            {
                context.Request.Path = new PathString(Regex.Replace(context.Request.Path.ToString(), "/token", "/connect/token"));
            }

            await Next.Invoke(context);
        }
    }
}