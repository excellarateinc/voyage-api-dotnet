using Autofac;
using FluentValidation.WebApi;
using Microsoft.Owin.Cors;
using Owin;
using System.Configuration;
using System.Web.Http;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Voyage.Api.Middleware;
using Voyage.Services.KeyContainer;

namespace Voyage.Api
{
    public partial class Startup
    {
        public void Configure(IAppBuilder app)
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
            
            // Allow Web API to consume bearer JWT tokens.
            var rsaProvider = ContainerConfig.Container.Resolve<IRsaKeyContainerService>();
            var tokenParam = new System.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidIssuer = ConfigurationManager.AppSettings["oAuth:Issuer"],
                ValidAudience = ConfigurationManager.AppSettings["oAuth:Audience"],
                IssuerSigningKey = new System.IdentityModel.Tokens.RsaSecurityKey(rsaProvider.GetRsaCryptoServiceProviderFromKeyContainer())
            };
            var jwtTokenOptions = new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                TokenValidationParameters = tokenParam
            };
            app.UseJwtBearerAuthentication(jwtTokenOptions);

            // 6. Add web api to pipeline
            app.UseWebApi(httpConfig);
        }
    }
}