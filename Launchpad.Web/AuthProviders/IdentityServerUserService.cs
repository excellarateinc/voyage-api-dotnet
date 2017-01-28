using System.Threading.Tasks;
using System.Web;
using Autofac;
using Autofac.Integration.Owin;
using IdentityServer3.Core.Extensions;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.Default;
using Launchpad.Services.User;

namespace Launchpad.Web.AuthProviders
{
    public class IdentityServerUserService : UserServiceBase
    {
        private readonly IUserService _userService;

        public IdentityServerUserService()
        {
            _userService = HttpContext.Current.GetOwinContext().GetAutofacLifetimeScope().Resolve<IUserService>();
        }

        public override async Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            bool isValid = await _userService.IsValidCredential(context.UserName, context.Password);
            if (isValid)
            {
                var claims = await _userService.CreateClaimsIdentityAsync(context.UserName, "OAuth");
                context.AuthenticateResult = new AuthenticateResult(context.UserName, context.UserName, claims.Claims);
            }
        }

        public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var claimsIdentity = await _userService.CreateClaimsIdentityAsync(context.Subject.GetSubjectId(), "OAuth");
            if (claimsIdentity != null)
            {
                context.IssuedClaims = claimsIdentity.Claims;
            }
        }
    }
}