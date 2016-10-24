using System;
using Xunit;
using FluentAssertions;
using Launchpad.UnitTests.Common;
using Launchpad.Web.Handlers;
using Moq;
using Launchpad.Services.Interfaces;
using System.Net.Http;
using Launchpad.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Launchpad.Web.UnitTests.Handlers
{
    public class RequestMetricsHandlerTests : BaseUnitTest
    {
        /// <summary>
        /// Fake handler needed to terminate the delegation chain 
        /// </summary>
        public class FakeHandler : DelegatingHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                response.Content = new StringContent("Hello!");
                return Task.FromResult(response);
            }
        }
     
        private readonly RequestMetricsHandler _handler;
        private readonly Mock<IRequestMetricsService> _mockMetricsService;

        public RequestMetricsHandlerTests()
        {
            _mockMetricsService = Mock.Create<IRequestMetricsService>();
            _handler = new RequestMetricsHandler(_mockMetricsService.Object);
        }

        [Fact]
        public void SendAsync_Should_Call_MetricsService()
        {
            //ARRANGE
            var request = new HttpRequestMessage() { RequestUri = new Uri("http://abc.com/widget"), Method = new HttpMethod("GET") };
            _handler.InnerHandler = new FakeHandler();
            var invoker = new HttpMessageInvoker(_handler);

            _mockMetricsService.Setup(_ => _.Log(It.Is<RequestDataPointModel>(arg => arg.Method.Equals("GET") && arg.Path.Equals("/widget"))));

            //ACT
            var task = invoker.SendAsync(request, new CancellationToken());
            task.Wait();

            //ASSERT
            Mock.VerifyAll();
        }

        [Fact]
        public void Ctor_Should_Throw_NullArgumentException_When_MetricsService_Is_Null()
        {
            Action throwAction = () => new RequestMetricsHandler(null);
            throwAction.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("metricsService");
        }
    }
}
