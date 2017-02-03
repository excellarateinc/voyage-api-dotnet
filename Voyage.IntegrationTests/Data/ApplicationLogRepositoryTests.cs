using System.Transactions;

using FluentAssertions;

using Voyage.Data;
using Voyage.Data.Repositories.ApplicationLog;

using Xunit;

namespace Voyage.IntegrationTests.Data
{
    [Collection(Constants.CollectionName)]
    public class ApplicationLogRepositoryTests
    {
        [Fact]
        public void GetRecentActivity_Should_Return_QueryDatabase()
        {
            using (new TransactionScope())
            {
                using (var context = new VoyageDataContext())
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
