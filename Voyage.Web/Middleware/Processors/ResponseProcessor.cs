using Microsoft.Owin;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Voyage.Web.Middleware.Processors
{
    /// <summary>
    /// Basic response processor
    /// </summary>
    public abstract class ResponseProcessor
    {
        public abstract bool ShouldProcess(IOwinResponse response);

        public virtual async Task<string> GetResponseStringAsync(IOwinResponse response)
        {
            if (!response.Body.CanSeek)
                throw new Exception("The body does not support seek. Ensure that the RewindResponseMiddleware is registered earlier in the pipeline");

            if (!ShouldProcess(response))
                throw new Exception("ShouldProcess predicate failed. This processor should not read this type of response");

            var responseStream = response.Body as MemoryStream;
            if (responseStream == null)
            {
                throw new Exception("The response.body could not be cast as MemoryStream. Ensure that the RewindResponseMiddleware is registered earlier in the pipeline");
            }

            responseStream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(responseStream);
            string body = await reader.ReadToEndAsync();
            return body;
        }
    }
}