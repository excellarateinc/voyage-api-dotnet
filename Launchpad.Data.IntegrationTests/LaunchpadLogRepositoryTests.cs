using Xunit;
using FluentAssertions;
using System.Transactions;

namespace Launchpad.Data.IntegrationTests
{
    public class LaunchpadLogRepositoryTests
    {
        [Fact]
        public void GetRecentActivity_Should_Return_QueryDatabase()
        {
            using (var transactionScope = new TransactionScope())
            {
                using (var context = new LaunchpadDataContext())
                {
                    var repository = new LaunchpadLogRepository(context);
                    
                    //Force a db query to verify the EF configuration is valid
                    var widgets = repository.GetRecentActivity();

                    widgets.Should().NotBeNull();
                }
            }
        }
    }
}
