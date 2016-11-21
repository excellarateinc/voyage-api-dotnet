using Launchpad.Core;
using Launchpad.Web.Extensions;
using Microsoft.Owin;

namespace Launchpad.Web.AuthProviders
{
    public class IdentityProvider : IIdentityProvider
    {
        private readonly IOwinContext _context;

        public IdentityProvider(IOwinContext context)
        {
            _context = context.ThrowIfNull(nameof(context));
        }
        public string GetUserName()
        {

            return _context.GetIdentityName();
        }
    }
}
   