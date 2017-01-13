using System.Transactions;

using FluentAssertions;

using Launchpad.Data;
using Launchpad.Data.Repositories.ApplicationLog;

using Xunit;

namespace Launchpad.IntegrationTests.Data
{
    [Collection(Constants.CollectionName)]
    public class ApplicationLogRepositoryTests
    {
        [Fact]
        public void GetRecentActivity_Should_Return_QueryDatabase()
        {
            using (new TransactionScope())
            {
                using (var context = new LaunchpadDataContext())
                {
                    var repository = new ApplicationLogRepository(context);

                    // Force a db query to verify the EF configuration is valid
                    var activity = repository.GetRecentActivity();

                    activity.Should().NotBeNull();
                }
            }
        }
    }
}
