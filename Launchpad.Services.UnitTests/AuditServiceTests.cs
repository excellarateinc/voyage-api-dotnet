using System;
using FluentAssertions;
using Launchpad.Data.Interfaces;
using Launchpad.Models;
using Launchpad.Models.EntityFramework;
using Launchpad.Services.UnitTests.Fixture;
using Launchpad.UnitTests.Common;
using Moq;
using Ploeh.AutoFixture;
using Xunit;

namespace Launchpad.Services.UnitTests
{
    [Trait("Category", "Audit.Service")]
    [Collection(AutoMapperCollection.CollectionName)]
    public class AuditServiceTests : BaseUnitTest
    {
        private readonly AuditService _auditService;
        private readonly Mock<IActivityAuditRepository> _mockAuditRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public AuditServiceTests(AutoMapperFixture mapperFixture)
        {
            _mockUnitOfWork = Mock.Create<IUnitOfWork>();
            _mockAuditRepository = Mock.Create<IActivityAuditRepository>();
            _auditService = new AuditService(_mockAuditRepository.Object, mapperFixture.MapperInstance, _mockUnitOfWork.Object);
        }

        [Fact]
        public void Record_Should_Call_Repository()
        {
            var model = Fixture.Create<ActivityAuditModel>();

            _mockAuditRepository.Setup(_ => _.Add(It.Is<ActivityAudit>(t => t.RequestId == model.RequestId)))
                .Returns<ActivityAudit>(t => t);

            _auditService.Record(model);

            Mock.VerifyAll();
        }

        [Fact]
        public async void RecordAsync_Should_Call_Repository()
        {
            var model = Fixture.Create<ActivityAuditModel>();

            _mockAuditRepository.Setup(_ => _.Add(It.Is<ActivityAudit>(t => t.RequestId == model.RequestId)))
                .Returns<ActivityAudit>(t => t);

            await _auditService.RecordAsync(model);

            Mock.VerifyAll();
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_Repository_Null()
        {
            Action throwAction = () => new AuditService(null, null, null);

            throwAction.ShouldThrow<ArgumentNullException>()
                .And
                .ParamName
                .Should()
                .Be("activityRepository");
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_Mapper_Null()
        {
            Action throwAction = () => new AuditService(_mockAuditRepository.Object, null, null);

            throwAction.ShouldThrow<ArgumentNullException>()
                .And
                .ParamName
                .Should()
                .Be("mapper");
        }
    }
} 