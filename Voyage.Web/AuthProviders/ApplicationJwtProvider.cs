using System.Security.Claims;
using System.Threading.Tasks;
using Autofac;
using Autofac.Integration.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Voyage.Core;
using Voyage.Services.User;

namespace Voyage.Web.AuthProviders
{
    public class ApplicationJwtProvider : OAuthAuthorizationServerProvider
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
            oAuthIdentity.AddClaim(new Claim("ClientId", context.ClientId));
            var ticket = new AuthenticationTicket(oAuthIdentity, new AuthenticationProperties());
            context.Validated(ticket);
        }

        /// <summary>
        /// Validate client that is trying to get access token
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // TODO Your application will need to verify client id and client secret from you database
            string clientId;
            string clientSecret;
            if (context.TryGetBasicCredentials(out clientId, out clientSecret) ||
                context.TryGetFormCredentials(out clientId, out clientSecret))
            {
                if (clientId == Clients.Client1.Id && clientSecret == Clients.Client1.Secret)
                {
                    context.Validated();
                }
                else if (clientId == Clients.Client2.Id && clientSecret == Clients.Client2.Secret)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Validating client redirect is mainly used for implicit authentication. e.g. angular app, mobile ...
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            // TODO Implement client id and redirect url against your application database
            if (context.ClientId == Clients.Client1.Id)
            {
                context.Validated(Clients.Client1.RedirectUrl);
            }
            else if (context.ClientId == Clients.Client2.Id)
            {
                context.Validated(Clients.Client2.RedirectUrl);
            }

            return Task.FromResult(0);
        }
    }
}