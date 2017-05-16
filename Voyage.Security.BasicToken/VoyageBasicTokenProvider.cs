using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Integration.Owin;
using Microsoft.Owin.Security.OAuth;
using Voyage.Services.User;

namespace Voyage.Security.BasicToken
{
    public class VoyageBasicTokenProvider : OAuthAuthorizationServerProvider
    {
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var scope = context.OwinContext.GetAutofacLifetimeScope();
            var userService = scope.Resolve<IUserService>();

            if (await userService.IsValidCredential(context.UserName, context.Password))
            {
                context.Validated(await userService.CreateClaimsIdentityAsync(context.UserName, "OAuth"));
            }
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            var grantType = context.Request.Headers.Get("grant_type");

            if (!string.IsNullOrWhiteSpace(grantType) && string.Equals(grantType, "password", StringComparison.InvariantCultureIgnoreCase))
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }
    }
}
