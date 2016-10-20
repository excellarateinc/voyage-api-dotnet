using Xunit;
using Ploeh.AutoFixture;
using FluentAssertions;
using Launchpad.UnitTests.Common;

namespace Launchpad.Models.UnitTests
{
    public class RequestDataPointModelTests : BaseUnitTest
    {
        [Fact]
        public void ToString_Should_Return_Formatted_Message()
        {
            var point = Fixture.Create<RequestDataPointModel>();

            var message = point.ToString();

            message.Should().Be($"{point.RequestDateTime}: {point.Method} -> {point.Path}");
        }
    }
}
