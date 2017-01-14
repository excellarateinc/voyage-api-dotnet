using System.Net;

namespace Launchpad.Core.Exceptions
{
    public class NotFoundException : ApiException
    {
        public NotFoundException() : base(HttpStatusCode.NotFound)
        {
        }        

        public NotFoundException(string message) : base(HttpStatusCode.NotFound, Constants.ErrorCodes.EntityNotFound, message)
        {
        }
    }
}
