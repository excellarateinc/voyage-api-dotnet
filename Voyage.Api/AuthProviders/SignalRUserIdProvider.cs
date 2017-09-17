using System.IdentityModel.Claims;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Security;
using Voyage.Core;

namespace Voyage.Api.AuthProviders
{
    public class SignalRUserIdProvider : IUserIdProvider
    {
        private readonly IAuthenticationManager _authenticationManager;

        public SignalRUserIdProvider(IAuthenticationManager authenticationManager)
        {
            _authenticationManager = authenticationManager.ThrowIfNull(nameof(authenticationManager));
        }

        public string GetUserId(IRequest request)
        {
            var claim = _authenticationManager.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier);
            return claim?.Value;
        }
    }
}
