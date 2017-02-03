using System.Linq;
using System.Transactions;
using FluentAssertions;
using Voyage.Data;
using Voyage.Data.Repositories.ActivityAudit;
using Xunit;

namespace Voyage.IntegrationTests.Data
{
    [Collection(Constants.CollectionName)]
    public class ActivityAuditRepositoryTests
    {
        [Fact]
        public void GetAll_Should_Return_Records()
        {
            using (var context = new VoyageDataContext())
            {
                var repository = new ActivityAuditRepository(context);

                var records = repository.GetAll().Take(10).ToList();

                records.Should().NotBeNull();
            }
        }
    }
}
