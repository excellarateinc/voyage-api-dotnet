using System;
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
            Action throwAction = () => _repository.Delete(1);
            throwAction.ShouldThrow<NotImplementedException>();
        }

        [Fact]
        public void Update_Should_Throw_NotImpelementedException()
        {
            Action throwAction = () => _repository.Update(new Voyage.Models.Entities.ActivityAudit());
            throwAction.ShouldThrow<NotImplementedException>();
        }
    }
}
