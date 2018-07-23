using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ploeh.AutoFixture;
using Voyage.Data.Repositories.ActivityAudit;
using Voyage.Models;
using Voyage.Models.Entities;
using Voyage.Services.Audit;
using Voyage.Services.UnitTests.Common;
using Voyage.Services.UnitTests.Common.AutoMapperFixture;
using Xunit;

namespace Voyage.Services.UnitTests
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
        public async void Record_Should_Call_Repository()
        {
            var model = Fixture.Create<ActivityAuditModel>();

            _mockAuditRepository.Setup(_ => _.AddAsync(It.Is<ActivityAudit>(t => t.RequestId == model.RequestId)))
                .Returns<ActivityAudit>(t => Task.FromResult(t));

            await _auditService.RecordAsync(model);

            Mock.VerifyAll();
        }

        [Fact]
        public async void RecordAsync_Should_Call_Repository()
        {
            var model = Fixture.Create<ActivityAuditModel>();

            _mockAuditRepository.Setup(_ => _.AddAsync(It.Is<ActivityAudit>(t => t.RequestId == model.RequestId)))
                .Returns<ActivityAudit>(t => Task.FromResult(t));

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