using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Ploeh.AutoFixture;
using Launchpad.UnitTests.Common;
using Moq;
using Launchpad.Data.Interfaces;
using AutoMapper;
using Launchpad.Services.Fixture;
using Launchpad.Models;
using Launchpad.Models.EntityFramework;

namespace Launchpad.Services.UnitTests
{
    [Trait("Category", "Audit.Service")]
    [Collection(AutoMapperCollection.CollectionName)]
    public class AuditServiceTests : BaseUnitTest
    {
        private AuditService _auditService;
        private Mock<IActivityAuditRepository> _mockAuditRepository;
        private AutoMapperFixture _mapperFixture;

        public AuditServiceTests(AutoMapperFixture mapperFixture)
        {
            _mockAuditRepository = Mock.Create<IActivityAuditRepository>();
            _mapperFixture = mapperFixture;
            _auditService = new AuditService(_mockAuditRepository.Object, _mapperFixture.MapperInstance);
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
 