using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Voyage.Models;
using Voyage.Services.Audit;
using Voyage.UnitTests.Common;
using Voyage.Web.Middleware;
using Voyage.Web.Middleware.Processors;

using Microsoft.Owin;
using Microsoft.Owin.Testing;

using Moq;

using Owin;

using Xunit;

namespace Voyage.UnitTests.Web.Middleware
{
    [Trait("Category", "CustomMiddleware")]
    public class ActivityAuditMiddlewareTests : BaseUnitTest
    {
        private readonly Mock<IAuditService> _mockAuditService;
        private readonly Mock<ErrorResponseProcessor> _mockProcessor;

        public ActivityAuditMiddlewareTests()
        {
            _mockAuditService = Mock.Create<IAuditService>();
            _mockProcessor = Mock.Create<ErrorResponseProcessor>();
        }

        [Fact]
        public async void Invoke_Should_Call_Logger_And_AuditService()
        {
            _mockProcessor.Setup(_ => _.ShouldProcess(It.IsAny<IOwinResponse>()))
                .Returns(false);

            var id = Guid.NewGuid().ToString();

            _mockAuditService.Setup(_ =>
                _.RecordAsync(It.Is<ActivityAuditModel>(m => m.RequestId == id)))
                .Returns(Task.FromResult(0));

            using (var server = TestServer.Create(app =>
            {
                // Use the test environment middleware to setup an context.Environment variables
                app.Use<TestEnvironmentMiddleware>(new Dictionary<string, object> { { "owin.RequestId", id } });

                // Middleware under test
                app.Use(
                    typeof(ActivityAuditMiddleware),
                    _mockAuditService.Object,
                    _mockProcessor.Object);

                app.Run(context =>
                {
                    return context.Response.WriteAsync("Hello world using OWIN TestServer");
                });
            }))
            {
                await server.HttpClient.GetAsync("/");

                // Verify the audit and logger was called
                Mock.VerifyAll();
            }
        }

        [Fact]
        public async void Invoke_Should_Set_ErrorMessage_When_Available()
        {
            const string error = "error!";

            _mockProcessor.Setup(_ => _.ShouldProcess(It.IsAny<IOwinResponse>()))
                .Returns(true);

            _mockProcessor.Setup(_ => _.GetResponseStringAsync(It.IsAny<IOwinResponse>()))
                .ReturnsAsync(error);

            var id = Guid.NewGuid().ToString();

            _mockAuditService.Setup(_ =>
                _.RecordAsync(It.Is<ActivityAuditModel>(m => m.RequestId == id && m.Error == null)))
                .Returns(Task.FromResult(0));

            _mockAuditService.Setup(_ =>
                _.RecordAsync(It.Is<ActivityAuditModel>(m => m.RequestId == id && m.Error == error)))
                .Returns(Task.FromResult(0));

            using (var server = TestServer.Create(app =>
            {
                // Use the test environment middleware to setup an context.Environment variables
                app.Use<TestEnvironmentMiddleware>(new Dictionary<string, object> { { "owin.RequestId", id } });

                // Middleware under test
                app.Use(
                    typeof(ActivityAuditMiddleware),
                    _mockAuditService.Object,
                    _mockProcessor.Object);

                app.Run(context =>
                {
                    return context.Response.WriteAsync("Hello world using OWIN TestServer");
                });
            }))
            {
                await server.HttpClient.GetAsync("/");

                // Verify the audit and logger was called
                Mock.VerifyAll();
            }
        }

        [Fact]
        public async void Invoke_Should_Call_Logger_And_AuditService_And_Overwrite_Empty_RequestId()
        {
            _mockProcessor.Setup(_ => _.ShouldProcess(It.IsAny<IOwinResponse>()))
               .Returns(false);

            var id = Guid.Empty.ToString();

            _mockAuditService.Setup(_ => _.RecordAsync(It.Is<ActivityAuditModel>(m => m.RequestId != id)))
                .Returns(Task.FromResult(0));

            using (var server = TestServer.Create(app =>
            {
                // Use the test environment middleware to setup an context.Environment variables
                app.Use<TestEnvironmentMiddleware>(new Dictionary<string, object> { { "owin.RequestId", Guid.Empty } });

                // Middleware under test
                app.Use(
                    typeof(ActivityAuditMiddleware),
                    _mockAuditService.Object,
                    _mockProcessor.Object);

                app.Run(context =>
                {
                    return context.Response.WriteAsync("Hello world using OWIN TestServer");
                });
            }))
            {
                await server.HttpClient.GetAsync("/");

                // Verify the audit and logger was called
                Mock.VerifyAll();
            }
        }
    }
}
