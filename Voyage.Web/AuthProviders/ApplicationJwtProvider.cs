using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using Autofac.Integration.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Voyage.Core;
using Voyage.Services.User;

namespace Voyage.Web.AuthProviders
{
    public class ApplicationJwtProvider : OAuthAuthorizationServerProvider
    {
        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var scope = context.OwinContext.GetAutofacLifetimeScope();
            var userService = scope.Resolve<IUserService>();
            var valid = await userService.IsValidCredential(context.UserName, context.Password);

            if (!valid)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            ClaimsIdentity oAuthIdentity = await userService.CreateJwtClaimsIdentityAsync(context.UserName);
            ClaimsIdentity cookiesIdentity = await userService.CreateClaimsIdentityAsync(context.UserName, CookieAuthenticationDefaults.AuthenticationType);

            AuthenticationProperties properties = CreateProperties(context.UserName);
            var ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
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

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
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

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}