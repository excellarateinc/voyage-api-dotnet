using Voyage.Core;
using Voyage.Web.Extensions;
using Microsoft.Owin;

namespace Voyage.Web.AuthProviders
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
