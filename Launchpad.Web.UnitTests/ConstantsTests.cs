using Launchpad.UnitTests.Common;
using Xunit;
using FluentAssertions;


namespace Launchpad.Web.UnitTests
{
    /// <summary>
    // The route prefixes are critical to web api routing, let's test the constants so that 
    // if they accidently change a test breaks
    /// </summary>
    public class ConstantsTests : BaseUnitTest
    {
        [Fact]
        public void V1_Should_Return_Known_Value()
        {
            Constants.RoutePrefixes.V1.Should().Be("api/v1");
        }

        [Fact]
        public void V2_Should_Return_Known_Value()
        {
            Constants.RoutePrefixes.V2.Should().Be("api/v2");
        }

    }
}
