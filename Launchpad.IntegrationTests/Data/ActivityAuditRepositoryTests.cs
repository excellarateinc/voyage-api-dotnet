using System.Linq;
using System.Transactions;
using FluentAssertions;
using Launchpad.Data;
using Launchpad.Data.Repositories.ActivityAudit;
using Xunit;

namespace Launchpad.IntegrationTests.Data
{
    [Collection(Constants.CollectionName)]
    public class ActivityAuditRepositoryTests
    {
        [Fact]
        public void GetAll_Should_Return_Records()
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
