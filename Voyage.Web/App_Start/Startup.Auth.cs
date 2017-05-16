using System.Configuration;
using FluentValidation.WebApi;
using Voyage.Web.Middleware;
using Microsoft.Owin.Cors;
using Owin;
using System.Web.Http;
using Voyage.Security.Oauth2;

namespace Voyage.Web
{
    public partial class Startup
    {
        public void Configure(IAppBuilder app)
        {
            var httpConfig = new HttpConfiguration();

            // Build the container
            ContainerConfig.Register(httpConfig);

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

            app.UseVoyageOauth2(new VoyageOauth2Configuration
            {
                Container = ContainerConfig.Container,
                TokenExpireSeconds = ConfigurationManager.AppSettings["oAuth:TokenExpireSeconds"],
                AllowInsecureHttp = bool.Parse(ConfigurationManager.AppSettings["oAuth:AllowInsecureHttp"])
            });
        }
    }
}