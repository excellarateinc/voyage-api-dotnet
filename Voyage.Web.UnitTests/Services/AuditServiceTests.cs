using System;
using FluentAssertions;
using Moq;
using Ploeh.AutoFixture;
using Voyage.Data.Repositories.ActivityAudit;
using Voyage.Models;
using Voyage.Models.Entities;
using Voyage.Services.Audit;
using Voyage.Web.UnitTests.Common;
using Voyage.Web.UnitTests.Common.AutoMapperFixture;
using Xunit;

namespace Voyage.Web.UnitTests.Services
{
    [Trait("Category", "Audit.Service")]
    [Collection(AutoMapperCollection.CollectionName)]
    public class AuditServiceTests : BaseUnitTest
    {
        private readonly AuditService _auditService;
        private readonly Mock<IActivityAuditRepository> _mockAuditRepository;

        public AuditServiceTests(AutoMapperFixture mapperFixture)
        {
            _mockAuditRepository = Mock.Create<IActivityAuditRepository>();
            _auditService = new AuditService(_mockAuditRepository.Object, mapperFixture.MapperInstance);
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
            Action throwAction = () => new AuditService(null, null);

            throwAction.ShouldThrow<ArgumentNullException>()
                .And
                .ParamName
                .Should()
                .Be("activityRepository");
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_Mapper_Null()
        {
            Action throwAction = () => new AuditService(_mockAuditRepository.Object, null);

            throwAction.ShouldThrow<ArgumentNullException>()
                .And
                .ParamName
                .Should()
                .Be("mapper");
        }
    }
}