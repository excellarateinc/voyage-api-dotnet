using Microsoft.Owin;

namespace Launchpad.Web.Middleware.Processors
{
    public class ErrorResponseProcessor : ResponseProcessor
    {
        public override bool ShouldProcess(IOwinResponse response)
        {
            return
                response.Body.CanSeek &&
                response.Body.CanRead &&
                response.StatusCode >= 400 &&
                response.StatusCode <= 599 &&
                response.ContentLength > 0 && (
                response.ContentType.StartsWith("application/json") ||
                response.ContentType.StartsWith("application/xml") ||
                response.ContentType.StartsWith("text/"));
        }
    }
}