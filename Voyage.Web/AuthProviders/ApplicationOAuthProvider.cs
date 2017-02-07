using Autofac;
using Autofac.Integration.Owin;
using Voyage.Core;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Infrastructure;
using Voyage.Services.User;
using Voyage.Web.Auth_Stuff;

namespace Voyage.Web.AuthProviders
{
    /// <summary>
    /// This provider is a singleton - since our dependency are per request, we cannot inject the dependencies via the constructor
    /// Use the service locator pattern to resolve dependencies here
    /// Reference: http://stackoverflow.com/questions/29591545/resolve-autofac-service-within-instanceperlifetimescope-on-owin-startup
    /// </summary>
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        // Backed off from using a dictionary here - there is no guarantee that the IsMatch logic will be as straight
        // forward as checking the path
        private readonly Dictionary<string, ILoginOrchestrator> _loginOrchestrators;

        public ApplicationOAuthProvider(IEnumerable<ILoginOrchestrator> loginOrchestrators)
        {
            loginOrchestrators.ThrowIfNull(nameof(loginOrchestrators));
            _loginOrchestrators = loginOrchestrators.ToDictionary(_ => _.TokenPath);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }

        /// <summary>
        /// Determines if the incoming request matches an endpoint. This override is here to support multiple versions of the
        /// login service
        /// </summary>
        /// <param name="context">Context</param>
        /// <returns>Task</returns>
        public override Task MatchEndpoint(OAuthMatchEndpointContext context)
        {
            // Check if there is a match path in the array - if so it is a token endpoint
            if (_loginOrchestrators.ContainsKey(context.Request.Path.Value))
            {
                context.MatchesTokenEndpoint();
                return Task.Delay(0);
            }

            // If it does not match a token endpoint, execute the default behavior
            return base.MatchEndpoint(context);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            // Let the orchestrator determine if it is a valid credential and then respond accordingly
            var loginOrchestrator = _loginOrchestrators[context.Request.Path.Value];
            var valid = await loginOrchestrator.ValidateCredential(context);

            if (!valid)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var scope = context.OwinContext.GetAutofacLifetimeScope();
            var userService = scope.Resolve<IUserService>();
            ClaimsIdentity oAuthIdentity = await userService.CreateClaimsIdentityAsync(context.UserName, OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await userService.CreateClaimsIdentityAsync(context.UserName, CookieAuthenticationDefaults.AuthenticationType);

            AuthenticationProperties properties = CreateProperties(context.UserName);
            var ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
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
    }
}
