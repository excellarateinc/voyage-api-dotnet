using System.Security.Claims;
using System.Threading.Tasks;
using Autofac;
using Autofac.Integration.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Voyage.Services.Client;
using Voyage.Services.User;
using Voyage.Core.Exceptions;

namespace Voyage.Security.Oauth2
{
    public class VoyageJwtProvider : OAuthAuthorizationServerProvider
    {
        /// <summary>
        /// Client credentials is used primary server to server authentication
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            var scope = context.OwinContext.GetAutofacLifetimeScope();
            var userService = scope.Resolve<IUserService>();

            ClaimsIdentity addionaloAuthIdentity = await userService.CreateClientClaimsIdentityAsync(context.ClientId);
            var oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
            oAuthIdentity.AddClaims(addionaloAuthIdentity.Claims);
            var ticket = new AuthenticationTicket(oAuthIdentity, new AuthenticationProperties());
            context.Validated(ticket);
        }

        /// <summary>
        /// Validate client that is trying to get access token
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // TODO Your application will need to verify client id and client secret from you database
            string clientId;
            string clientSecret;
            if (context.TryGetBasicCredentials(out clientId, out clientSecret) ||
                context.TryGetFormCredentials(out clientId, out clientSecret))
            {
                var scope = context.OwinContext.GetAutofacLifetimeScope();
                var clientService = scope.Resolve<IClientService>();
                var client = await clientService.GetClientAsync(clientId);

                // Check if locked out
                if (await clientService.IsLockedOutAsync(client.Id))
                {
                    throw new NotFoundException("User Locked Out");
                }
                if (await clientService.IsValidClientAsync(clientId, clientSecret))
                {
                    // ResetAccess
                    await clientService.UpdateFailedLoginAttemptsAsync(client.Id, false);
                    context.Validated();
                }
                else
                {
                    await clientService.UpdateFailedLoginAttemptsAsync(client.Id, true);
                    throw new NotFoundException("Invalid Credentials");
                }
            }
        }

        /// <summary>
        /// Validating client redirect is mainly used for implicit authentication. e.g. angular app, mobile ...
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            var scope = context.OwinContext.GetAutofacLifetimeScope();
            var clientService = scope.Resolve<IClientService>();
            var client = await clientService.GetClientAsync(context.ClientId);

            if (client != null)
            {
                context.Validated(client.RedirectUrl);
            }
        }
    }
}