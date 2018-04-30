using System.Threading.Tasks;
using Autofac;
using Autofac.Integration.Owin;
using Microsoft.Owin.Security.OAuth;
using Voyage.Services.Client;
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

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string clientSecret;
            if (context.TryGetBasicCredentials(out clientId, out clientSecret) ||
                context.TryGetFormCredentials(out clientId, out clientSecret))
            {
                var scope = context.OwinContext.GetAutofacLifetimeScope();
                var clientService = scope.Resolve<IClientService>();

                if (await clientService.IsValidClientAsync(clientId, clientSecret))
                {
                    context.Validated();
                }
            }
        }
    }
}
