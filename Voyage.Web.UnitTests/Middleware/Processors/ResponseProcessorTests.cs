using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Owin;
using Moq;
using Voyage.Web.Middleware.Processors;
using Voyage.Web.UnitTests.Common;
using Xunit;

namespace Voyage.Web.UnitTests.Middleware.Processors
{
    public class ResponseProcessorTests : BaseUnitTest
    {
        // Test class for testing the base methods
        internal class TestProcessor : ResponseProcessor
        {
            public bool Valid { get; set; }

            public override bool ShouldProcess(IOwinResponse response)
            {
                return Valid;
            }
        }

        private readonly TestProcessor _testProcessor;
        private readonly Mock<IOwinResponse> _mockResponse;

        public ResponseProcessorTests()
        {
            _mockResponse = Mock.Create<IOwinResponse>();
            _testProcessor = new TestProcessor();
        }

        [Fact]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async void GetResponseStringAsync_Should_Throw_Exception_When_Stream_Not_MemoryStream()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            _testProcessor.Valid = true;
            var mockStream = Mock.Create<Stream>();
            mockStream.Setup(_ => _.CanSeek)
                .Returns(true);

            _mockResponse.Setup(_ => _.Body)
                .Returns(mockStream.Object);

            Func<Task> throwAction = async () => await _testProcessor.GetResponseStringAsync(_mockResponse.Object);
            throwAction.ShouldThrow<Exception>()
                .And
                .Message
                .Should()
                .Be("The response.body could not be cast as MemoryStream. Ensure that the RewindResponseMiddleware is registered earlier in the pipeline");
        }

        [Fact]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async void GetResponseStringAsync_Should_Throw_Exception_When_Stream_Cannot_Seek()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            _testProcessor.Valid = false;
            var mockStream = Mock.Create<Stream>();
            mockStream.Setup(_ => _.CanSeek)
                .Returns(false);

            _mockResponse.Setup(_ => _.Body)
                .Returns(mockStream.Object);

            Func<Task> throwAction = async () => await _testProcessor.GetResponseStringAsync(_mockResponse.Object);
            throwAction.ShouldThrow<Exception>()
                .And
                .Message
                .Should()
                .Be("The body does not support seek. Ensure that the RewindResponseMiddleware is registered earlier in the pipeline");
        }

        [Fact]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async void GetResponseStringAsync_Should_Throw_Exception_When_ShouldProcess_False()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            _testProcessor.Valid = false;
            var memoryStream = new MemoryStream();

            _mockResponse.Setup(_ => _.Body)
                .Returns(memoryStream);

            Func<Task> throwAction = async () => await _testProcessor.GetResponseStringAsync(_mockResponse.Object);
            throwAction.ShouldThrow<Exception>()
                .And
                .Message
                .Should()
                .Be("ShouldProcess predicate failed. This processor should not read this type of response");
        }

        [Fact]
        public async void GetResponseStringAsync_Should_Read_Body_And_Return_String()
        {
            const string payload = "payload";
            using (var memoryStream = new MemoryStream(Encoding.Default.GetBytes(payload)))
            {
                _mockResponse.Setup(_ => _.Body)
                    .Returns(memoryStream);

                _testProcessor.Valid = true;

                var result = await _testProcessor.GetResponseStringAsync(_mockResponse.Object);

                Mock.VerifyAll();
                result.Should().Be(payload);
            }
        }
    }
}
