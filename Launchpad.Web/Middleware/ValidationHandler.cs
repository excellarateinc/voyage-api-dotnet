using Microsoft.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Launchpad.Web.Middleware
{
    public class ValidationHandler : OwinMiddleware
    {
        private OwinMiddleware _next;

        public ValidationHandler(OwinMiddleware next)
            : base(next)
        {
            _next = next;
        }

        public async override Task Invoke(IOwinContext context)
        {
            using (var inputBuffer = new MemoryStream())
            {
                //replace the context response with our buffer
                var stream = context.Response.Body;
                context.Response.Body = inputBuffer;

                //invoke the rest of the pipeline
                await _next.Invoke(context);

                //reset the buffer and read out the contents
                inputBuffer.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(inputBuffer);
                using (var bufferReader = new StreamReader(inputBuffer))
                {
                    string body = await bufferReader.ReadToEndAsync();

                    //reset to start of stream
                    inputBuffer.Seek(0, SeekOrigin.Begin);

                    if (Convert.ToInt32(context.Environment["owin.ResponseStatusCode"]) == 400)
                    {
                        var json = JObject.Parse(body);

                        var selection = json.SelectToken("modelState").Children()
                                .Select(x => new { Code = ((JProperty)x).Name, Description = ((JProperty)x).Value[0] }).ToList();

                        var back = JsonConvert.SerializeObject(selection);

                        byte[] bytes = Encoding.UTF8.GetBytes(back);

                        var outputBuffer = new MemoryStream(bytes);

                        outputBuffer.Seek(0, SeekOrigin.Begin);

                        context.Response.ContentType = "application/json";

                        await outputBuffer.CopyToAsync(stream);
                    }
                    else
                    {
                        //copy original content to the original stream and put it back
                        await inputBuffer.CopyToAsync(stream);
                        context.Response.Body = stream;
                    }
                }
            }
        }
    }
}