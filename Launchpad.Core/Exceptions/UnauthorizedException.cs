using System.Net;

namespace Launchpad.Core.Exceptions
{
    public class UnauthorizedException : ApiException
    {
        public UnauthorizedException() : base(HttpStatusCode.Unauthorized)
        {
        }

        public UnauthorizedException(string message) : base(HttpStatusCode.Unauthorized, Constants.ErrorCodes.Unauthorized, message)
        {
        }
    }
}
