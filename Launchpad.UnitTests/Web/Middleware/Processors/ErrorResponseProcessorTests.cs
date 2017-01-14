using System.IO;

using FluentAssertions;

using Launchpad.UnitTests.Common;
using Launchpad.Web.Middleware.Processors;

using Microsoft.Owin;

using Moq;

using Xunit;

namespace Launchpad.UnitTests.Web.Middleware.Processors
{
    [Trait("Category", "Processors")]
    public class ErrorResponseProcessorTests : BaseUnitTest
    {
        private readonly ErrorResponseProcessor _processor;
        private readonly Mock<IOwinResponse> _mockResponse;
        private readonly Mock<Stream> _mockStream;

        public ErrorResponseProcessorTests()
        {
            _mockStream = Mock.Create<Stream>();
            _mockResponse = Mock.Create<IOwinResponse>();
            _mockResponse.Setup(_ => _.Body).Returns(_mockStream.Object);
            _processor = new ErrorResponseProcessor();
        }

        [Fact]
        public void ShouldProcess_Should_Return_False_When_CanSeek_False()
        {
            _mockStream.Setup(_ => _.CanSeek).Returns(false);
           
            _processor.ShouldProcess(_mockResponse.Object).Should().BeFalse();
            Mock.VerifyAll();
        }

        [Fact]
        public void ShouldProcess_Should_Return_False_When_CanRead_False()
        {
            _mockStream.Setup(_ => _.CanSeek).Returns(true);
            _mockStream.Setup(_ => _.CanRead).Returns(false);
           
            _processor.ShouldProcess(_mockResponse.Object).Should().BeFalse();
            Mock.VerifyAll();
        }

        [Theory]
        [InlineData(399)]
        [InlineData(600)]
        public void ShouldProcess_Should_Return_False_When_StatusCode_Out_Of_Range(int statusCode)
        {
            _mockStream.Setup(_ => _.CanSeek).Returns(true);
            _mockStream.Setup(_ => _.CanRead).Returns(true);
            _mockResponse.Setup(_ => _.StatusCode).Returns(statusCode);
            
            _processor.ShouldProcess(_mockResponse.Object).Should().BeFalse();
            Mock.VerifyAll();
        }

        [Fact]
        public void ShouldProcess_Should_Return_False_When_ContentLength_Zero()
        {
            _mockStream.Setup(_ => _.CanSeek).Returns(true);
            _mockStream.Setup(_ => _.CanRead).Returns(true);
            _mockResponse.Setup(_ => _.StatusCode).Returns(400);
            _mockResponse.Setup(_ => _.ContentLength).Returns(0);

            _processor.ShouldProcess(_mockResponse.Object).Should().BeFalse();
            Mock.VerifyAll();
        }

        [Fact]
        public void ShouldProcess_Should_Return_False_When_ContentType_NotValid()
        {
            _mockStream.Setup(_ => _.CanSeek).Returns(true);
            _mockStream.Setup(_ => _.CanRead).Returns(true);
            _mockResponse.Setup(_ => _.StatusCode).Returns(400);
            _mockResponse.Setup(_ => _.ContentLength).Returns(10);
            _mockResponse.Setup(_ => _.ContentType).Returns("some-content");

            _processor.ShouldProcess(_mockResponse.Object).Should().BeFalse();
            Mock.VerifyAll();
        }

        [Theory]
        [InlineData(400)]
        [InlineData(599)]
        public void ShouldProcess_Should_Return_True_When_StatusCode_In_Range(int statusCode)
        {
            _mockStream.Setup(_ => _.CanSeek).Returns(true);
            _mockStream.Setup(_ => _.CanRead).Returns(true);
            _mockResponse.Setup(_ => _.ContentLength).Returns(12);
            _mockResponse.Setup(_ => _.ContentType).Returns("application/json");
            _mockResponse.Setup(_ => _.StatusCode).Returns(statusCode);

            _processor.ShouldProcess(_mockResponse.Object).Should().BeTrue();
            Mock.VerifyAll();            
        }

        [Theory]
        [InlineData("application/json")]
        [InlineData("application/xml")]
        [InlineData("text/")]
        public void ShouldProcess_Should_Return_True_When_ContentType_Valid(string contentType)
        {
            _mockStream.Setup(_ => _.CanSeek).Returns(true);
            _mockStream.Setup(_ => _.CanRead).Returns(true);
            _mockResponse.Setup(_ => _.ContentLength).Returns(12);
            _mockResponse.Setup(_ => _.ContentType).Returns(contentType);
            _mockResponse.Setup(_ => _.StatusCode).Returns(403);

            _processor.ShouldProcess(_mockResponse.Object).Should().BeTrue();
            Mock.VerifyAll();
        }
    }
}
