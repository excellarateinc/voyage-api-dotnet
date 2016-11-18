using Launchpad.Data.Interfaces;
using Launchpad.UnitTests.Common;
using Moq;
using System;
using FluentAssertions;
using Xunit;

namespace Launchpad.Data.UnitTests
{
    [Trait("Category", "Auditing")]
    public class ActivityAuditRepositoryTests : BaseUnitTest
    {
        ActivityAuditRepository _repository;
        Mock<ILaunchpadDataContext> _mockContext;

        public ActivityAuditRepositoryTests()
        {
            _mockContext = Mock.Create<ILaunchpadDataContext>();
            _repository = new ActivityAuditRepository(_mockContext.Object);
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
            Action throwAction = () => _repository.Update(new Models.EntityFramework.ActivityAudit());
            throwAction.ShouldThrow<NotImplementedException>();
        }
    }
}
