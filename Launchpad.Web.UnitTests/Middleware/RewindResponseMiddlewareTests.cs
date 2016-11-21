using Launchpad.Web.Middleware;
using Microsoft.Owin.Testing;
using Xunit;
using System.Net.Http;
using Launchpad.UnitTests.Common;
using Owin;
using FluentAssertions;
using System.IO;

namespace Launchpad.Web.UnitTests.Middleware
{
    public class RewindResponseMiddlewareTests : BaseUnitTest
    {
        [Fact]
        public async void Invoke_Should_Write_Back_To_Stream()
        {
            using (var server = TestServer.Create(app =>
            {

                app.Use<RewindResponseMiddleware>();

                app.Run(context =>
                {
                    //Assert that after the middleware executes, it is a memory stream
                    context.Response.Body.Should().BeOfType<MemoryStream>();
                    return context.Response.WriteAsync("Hello world using OWIN TestServer");
                });

            }))
            {
                HttpResponseMessage response = await server.HttpClient.GetAsync("/");
                var payload = await response.Content.ReadAsStringAsync();
                
                payload.Should().Be("Hello world using OWIN TestServer");
                //Verify the audit and logger was called
                Mock.VerifyAll();
            }
        }
    }
}
