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
        public void Get_Should_Return_Record()
        {
            using (new TransactionScope())
            {
                using (var context = new LaunchpadDataContext())
                {
                    var repository = new ActivityAuditRepository(context);

                    var newEntity = context.AddActivityAudit();

                    var record = repository.Get(newEntity.Id);

                    record.Should().NotBeNull();
                }
            }
        }

        [Fact]
        public void GetAll_Should_Return_Records()
        {
            using (new TransactionScope())
            {
                using (var context = new LaunchpadDataContext())
                {
                    var repository = new ActivityAuditRepository(context);

                    context.AddActivityAudit();

                    var records = repository.GetAll().Take(10).ToList();

                    records.Should().NotBeNullOrEmpty();
                }
            }
        }

        [Fact]
        public void Add_Should_Create_Record()
        {
            using (new TransactionScope())
            {
                using (var context = new LaunchpadDataContext())
                {
                    var repository = new ActivityAuditRepository(context);

                    var audit = new ActivityAudit
                    {
                        RequestId = System.Guid.NewGuid().ToString(),
                        IpAddress = "1.1.1.1",
                        UserName = "bob@bob.com",
                        Method = "PATCH",
                        Path = "/widget/1",
                        Date = System.DateTime.Now
                    };

                    repository.Add(audit);

                    var newEntity = repository.Get(audit.Id);

                    newEntity.Should().NotBeNull();
                }
            }
        }
    }
}
