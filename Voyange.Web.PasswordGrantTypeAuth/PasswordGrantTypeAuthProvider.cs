using Autofac;
using Autofac.Integration.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;
using Voyage.Services.User;

namespace Voyage.Web.AuthProviders
{
    public class PasswordGrantTypeAuthProvider : ApplicationJwtProvider
    {
        /// <summary>
        /// Added for grant_type: password type of authentication
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var scope = context.OwinContext.GetAutofacLifetimeScope();
            var userService = scope.Resolve<IUserService>();
            var isValidCredential = await userService.IsValidCredential(context.UserName, context.Password);
            if (!isValidCredential)
            {
                context.Rejected();
            }

            var oAuthIdentity = await userService.CreateClaimsIdentityAsync(context.UserName, OAuthDefaults.AuthenticationType);
            var ticket = new AuthenticationTicket(oAuthIdentity, new AuthenticationProperties());
            context.Validated(ticket);
        }
    }
}
