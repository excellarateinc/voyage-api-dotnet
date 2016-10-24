using FluentAssertions;
using Launchpad.Services.Monitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Threading;
using System.Net;
using Launchpad.Models.Enum;

namespace Launchpad.Services.UnitTests.Monitors
{


    public class HttpEndpointStatusMonitorTests : IDisposable
    {

        public class FakeResponse
        {
            public bool ThrowError { get; set; }
            public string Content { get; set; }
            
            public HttpStatusCode StatusCode { get; set; }
             
            public HttpResponseMessage ToMessage()
            {
                if (ThrowError)  throw new AggregateException("Forced HTTP error");
                var message = new HttpResponseMessage(StatusCode);
                message.Content = new StringContent(Content);
                return message;

            }
        }

        public class FakeMessageHandle : HttpMessageHandler
        {
            Queue<FakeResponse> _responses;

            public FakeMessageHandle()
            {
                _responses = new Queue<FakeResponse>();
            }

            public void AddResponse(FakeResponse response)
            {
                _responses.Enqueue(response);
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_responses.Dequeue().ToMessage());
            }
        }

        private HttpEndpointStatusMonitor _monitor;
        private HttpClient _client;
        private FakeMessageHandle _handler;

        public HttpEndpointStatusMonitorTests()
        {
            _handler = new FakeMessageHandle();
            _client = new HttpClient(_handler);
            _monitor = new HttpEndpointStatusMonitor(_client);
        }

        [Fact]
        public void GetStatus_Should_Return_Statuses()
        {
            var response1 = new FakeResponse { StatusCode = HttpStatusCode.OK, Content = "Some Content" };
            var response2 = new FakeResponse { StatusCode = HttpStatusCode.NotFound, Content = "Not Found" };
            var response3 = new FakeResponse { ThrowError = true };

            _handler.AddResponse(response1);
            _handler.AddResponse(response2);
            _handler.AddResponse(response3);

            var result = _monitor.GetStatus().ToList();

            //Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(3);

            result[0].Code.Should().Be(StatusCode.OK);
            result[0].Message.Should().StartWith("Connected to");

            result[1].Code.Should().Be(StatusCode.Critical);
            result[1].Message.Should().StartWith("Unable to connect to");

            result[2].Code.Should().Be(StatusCode.Critical);
            result[2].Message.Should().StartWith("Error processing");

        }

        [Fact]
        public void Name_Should_Return_Known_Value()
        {
            _monitor.Name.Should().Be("Http Endpoint Status");
        }

        [Fact]
        public void Type_Should_Return_Known_Value()
        {
            _monitor.Type.Should().Be(MonitorType.HttpEndpoint);
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_Client_Is_Null()
        {
            Action throwAction = () => new HttpEndpointStatusMonitor(null);
            throwAction.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("client");
        }

        public void Dispose()
        {
            _client.Dispose();
            _handler.Dispose();
        }
    }
}
