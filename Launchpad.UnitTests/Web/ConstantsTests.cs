using FluentAssertions;

using Voyage.UnitTests.Common;
using Voyage.Web;

using Xunit;

namespace Voyage.UnitTests.Web
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
            Constants.ApplicationName.Should().Be("Voyage .Net API");
        }

        [Fact]
        public void V1_Should_Return_Known_Value()
        {
            Constants.RoutePrefixes.V1.Should().Be("api/v1");
        }
    }
}
