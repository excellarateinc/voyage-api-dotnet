using Xunit;
using Launchpad.UnitTests.Common;
using Launchpad.Web.Middleware;
using Serilog;
using Moq;
using Microsoft.Owin;
using Microsoft.Owin.Testing;
using Owin;
using System.Net.Http;
using Launchpad.Services.Interfaces;
using System;
using System.Threading.Tasks;
using Launchpad.Models;
using System.Collections.Generic;

namespace Launchpad.Web.UnitTests.Middleware
{
    [Trait("Category", "CustomMiddleware")]
    public class ActivityAuditMiddlewareTests : BaseUnitTest
    {
        private Mock<ILogger> _mockLogger;
        private Mock<IOwinContext> _mockOwinContext;
        private Mock<IAuditService> _mockAuditService;

        public ActivityAuditMiddlewareTests()
        {
            _mockOwinContext = Mock.Create<IOwinContext>();
            _mockLogger = Mock.Create<ILogger>();
            _mockAuditService = Mock.Create<IAuditService>();
        }

    
        [Fact]
        public async void Invoke_Should_Call_Logger()
        {
            //Setup the dependencies 
            _mockLogger.Setup(_ => _.ForContext<ActivityAuditMiddleware>())
                .Returns(_mockLogger.Object);

            _mockLogger.Setup(_ => _.Information(It.IsAny<string>(), It.IsAny<object[]>()));

            var id = Guid.NewGuid().ToString();

            _mockAuditService.Setup(_ => _.RecordAsync(It.Is<ActivityAuditModel>(m => m.RequestId == id)))
                .Returns(Task.FromResult(0));

            using (var server = TestServer.Create(app =>
            {
                //Use the test environment middleware to setup an context.Environment variables
                app.Use<TestEnvironmentMiddleware>(new Dictionary<string, object>() { { "owin.RequestId", id } });

                //Middleware under test
                app.Use(typeof(ActivityAuditMiddleware), _mockLogger.Object, _mockAuditService.Object);

                app.Run(context =>
                {
                    return context.Response.WriteAsync("Hello world using OWIN TestServer");
                });

            }))
            {
                HttpResponseMessage response = await server.HttpClient.GetAsync("/");

                //Verify the audit and logger was called
                Mock.VerifyAll();
            }
        }
    }
}
