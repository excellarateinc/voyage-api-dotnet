using Microsoft.Owin;

namespace Launchpad.Web.Middleware.Processors
{
    /// <summary>
    /// Reads a the response as a string if it was an error
    /// </summary>
    /// <remarks>
    /// This assumes the write-only stream has been replaced
    /// with a read-write stream
    /// </remarks>
    public class ErrorResponseProcessor : ResponseProcessor
    {
        public override bool ShouldProcess(IOwinResponse response)
        {
            return
                response.Body.CanSeek && //Can the steam seek?
                response.Body.CanRead && //can the stream read?
                response.StatusCode >= 400 && //Request errors
                response.StatusCode <= 599 && //Server errors
                response.ContentLength > 0 && ( //Readable content
                response.ContentType.StartsWith("application/json") ||
                response.ContentType.StartsWith("application/xml") ||
                response.ContentType.StartsWith("text/"));
        }
    }
}