using FluentAssertions;
using System.Transactions;
using Launchpad.Data.Repositories.ApplicationLog;
using Xunit;

namespace Launchpad.Data.IntegrationTests
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
