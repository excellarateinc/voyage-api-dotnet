using Microsoft.Owin;
using System.IO;
using System.Threading.Tasks;

namespace Launchpad.Web.Middleware
{
    public class RewindResponseMiddleware : OwinMiddleware
    {
        public RewindResponseMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public async override Task Invoke(IOwinContext context)
        {
            using (var pipelineStream = new MemoryStream())
            {
                //replace the context response with our buffer
                var remoteStream = context.Response.Body;
                context.Response.Body = pipelineStream;

                //invoke the rest of the pipeline
                await Next.Invoke(context);

                pipelineStream.Seek(0, SeekOrigin.Begin);
                await pipelineStream.CopyToAsync(remoteStream);
                context.Response.Body = remoteStream;

            }
        }
    }
}