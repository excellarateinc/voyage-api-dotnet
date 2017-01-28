using FluentValidation.WebApi;
using Launchpad.Web.AuthProviders;
using Launchpad.Web.Middleware;
using Microsoft.Owin.Cors;
using Owin;
using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using IdentityServer3.AccessTokenValidation;
using IdentityServer3.Core.Configuration;
using IUserService = IdentityServer3.Core.Services.IUserService;

namespace Launchpad.Web
{
    public partial class Startup
    {
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

            // 5. Configure OAuth (IdentityServer3)
            var factory = new IdentityServerServiceFactory()
                .UseInMemoryClients(IdentityServerHelpers.GetClients())
                .UseInMemoryScopes(IdentityServerHelpers.GetScopes());

            factory.UserService = new Registration<IUserService>(typeof(IdentityServerUserService));

            app.Map("/OAuth", idsrvApp =>
            {
                idsrvApp.UseIdentityServer(new IdentityServerOptions
                {
                    SiteName = "Voyage IdentityServer",
                    RequireSsl = !bool.Parse(ConfigurationManager.AppSettings["oAuth:AllowInsecureHttp"]),
                    SigningCertificate = LoadCertificate(),
                    Factory = factory
                });
            });

            // Accept access tokens from IdentityServer.
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = ConfigurationManager.AppSettings["IdentityServerAuthority"],
                ValidationMode = ValidationMode.ValidationEndpoint
            });

            // 6. Add web api to pipeline
            app.UseWebApi(httpConfig);
        }

        private static X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["IdentityServerCertificatePathEnding"]), "idsrv3test");
        }
    }
}