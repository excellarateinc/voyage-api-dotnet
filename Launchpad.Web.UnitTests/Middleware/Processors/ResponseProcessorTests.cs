using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using Ploeh.AutoFixture;
using Launchpad.UnitTests.Common;
using Launchpad.Web.Middleware.Processors;
using Microsoft.Owin;
using System.IO;

namespace Launchpad.Web.UnitTests.Middleware.Processors
{
    public class ResponseProcessorTests : BaseUnitTest
    {
        //Test class for testing the base methods
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

        //
        [Fact]
        public async void GetResponseStringAsync_Should_Throw_Exception_When_Stream_Not_MemoryStream()
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
        public async void GetResponseStringAsync_Should_Throw_Exception_When_Stream_Cannot_Seek()
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
        public async void GetResponseStringAsync_Should_Throw_Exception_When_ShouldProcess_False()
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
