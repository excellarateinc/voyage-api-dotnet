using FluentAssertions;
using Launchpad.UnitTests.Common;
using Launchpad.Web;
using Xunit;

namespace Launchpad.UnitTests.Web
{
    /// <summary>
    /// The route prefixes are critical to web api routing, let's test the constants so that
    /// if they accidently change a test breaks
    /// </summary>
    public class ConstantsTests : BaseUnitTest
    {
        [Fact]
        public void ApplicationName_Should_Return_Known_Value()
        {
            Constants.ApplicationName.Should().Be(".Net API");
        }

        [Fact]
        public void V1_Should_Return_Known_Value()
        {
            Constants.RoutePrefixes.V1.Should().Be("api/v1");
        }
    }
}
