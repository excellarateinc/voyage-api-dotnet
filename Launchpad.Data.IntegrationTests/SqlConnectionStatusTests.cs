using FluentAssertions;
using System.Configuration;
using Xunit;

namespace Launchpad.Data.IntegrationTests
{
    [Collection(Constants.CollectionName)]
    public class SqlConnectionStatusTests
    {
        [Fact]
        public void Test_Should_Return_True_On_Success()
        {
            var status = new SqlConnectionStatus(ConfigurationManager.ConnectionStrings["LaunchpadDataContext"].ConnectionString, "Name");
            status.Test().Should().BeTrue();
        }

        [Fact]
        public void Test_Should_Return_False_On_Failure()
        {
            var status = new SqlConnectionStatus(ConfigurationManager.ConnectionStrings["BrokenContext"].ConnectionString, "Name");
            status.Test().Should().BeFalse();
        }
    }
}
