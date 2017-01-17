using System.Net;

namespace Launchpad.Core.Exceptions
{
    public class BadRequestException : ApiException
    {
        public BadRequestException()
            : base(HttpStatusCode.BadRequest)
        {
        }

        public BadRequestException(string message)
            : base(HttpStatusCode.BadRequest, message)
        {
        }

        public BadRequestException(string errorCode, string errorDescription)
            : base(HttpStatusCode.BadRequest, errorCode, errorDescription)
        {
        }
    }
}
