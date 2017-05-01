using Voyage.Core;
using Microsoft.Owin;
using Voyage.Api.Extensions;

namespace Voyage.Api.AuthProviders
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
