using Xunit;
using Launchpad.UnitTests.Common;
using Launchpad.Web.Middleware;
using Serilog;
using Moq;
using Microsoft.Owin;
using Microsoft.Owin.Testing;
using Owin;
using System.Net.Http;

namespace Launchpad.Web.UnitTests.Middleware
{
    [Trait("Category", "CustomMiddleware")]
    public class ActivityAuditMiddlewareTests : BaseUnitTest
    {
        private Mock<ILogger> _mockLogger;
        private Mock<IOwinContext> _mockOwinContext;

        public ActivityAuditMiddlewareTests()
        {
            _mockOwinContext = Mock.Create<IOwinContext>();
            _mockLogger = Mock.Create<ILogger>();
        }

    
        [Fact]
        public async void Invoke_Should_Call_Logger()
        {
            _mockLogger.Setup(_ => _.ForContext<ActivityAuditMiddleware>())
                .Returns(_mockLogger.Object);

            _mockLogger.Setup(_ => _.Information(It.IsAny<string>(), It.IsAny<object[]>()));

            using (var server = TestServer.Create(app =>
            {

                app.Use(typeof(ActivityAuditMiddleware), _mockLogger.Object);
                app.Run(context =>
                {
                    return context.Response.WriteAsync("Hello world using OWIN TestServer");
                });

            }))
            {
                HttpResponseMessage response = await server.HttpClient.GetAsync("/");
            }
        }
    }
}
