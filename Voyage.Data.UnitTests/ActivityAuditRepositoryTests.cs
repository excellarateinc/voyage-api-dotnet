using System;
using System.Threading.Tasks;
using FluentAssertions;
using Voyage.Data.Repositories.ActivityAudit;
using Voyage.Data.UnitTests.Common;
using Xunit;

namespace Voyage.Data.UnitTests
{
    [Trait("Category", "Auditing")]
    public class ActivityAuditRepositoryTests : BaseUnitTest
    {
        private readonly ActivityAuditRepository _repository;

        public ActivityAuditRepositoryTests()
        {
            var mockContext = Mock.Create<IVoyageDataContext>();
            _repository = new ActivityAuditRepository(mockContext.Object);
        }

        [Fact]
        public void Delete_Should_Throw_NotImpelementedException()
        {
            Func<Task> throwAction = async () => await _repository.DeleteAsync(1);
            throwAction.ShouldThrow<NotImplementedException>();
        }

        [Fact]
        public void Update_Should_Throw_NotImpelementedException()
        {
            Func<Task> throwAction = async () => await _repository.UpdateAsync(new Voyage.Models.Entities.ActivityAudit());
            throwAction.ShouldThrow<NotImplementedException>();
        }
    }
}
