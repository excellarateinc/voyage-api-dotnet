using Launchpad.Models.EntityFramework;
using System.Transactions;
using Xunit;
using FluentAssertions;
using Launchpad.Data.IntegrationTests.Extensions;
using System.Linq;

namespace Launchpad.Data.IntegrationTests
{
    [Collection(Constants.CollectionName)]
    public class ActivityAuditRepositoryTests
    {
        [Fact]
        public void GetAll_Should_Return_Records()
        {
            using (new TransactionScope())
            {
                using (var context = new LaunchpadDataContext())
                {
                    var repository = new ActivityAuditRepository(context);

                    var records = repository.GetAll().Take(10).ToList();

                    records.Should().NotBeNullOrEmpty();
                }
            }
        }
    }
}
