using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Ploeh.AutoFixture;
using Moq;
using Launchpad.UnitTests.Common;
using Launchpad.Services.Fixture;
using AutoMapper;
using Launchpad.Data.Interfaces;
using Launchpad.Services.Monitors;
using Launchpad.Models.EntityFramework;

namespace Launchpad.Services.UnitTests.Monitors
{
    [Collection(AutoMapperCollection.CollectionName)]
    public class LogMonitorTests : BaseUnitTest
    {
        private IMapper _mapper;
        private Mock<ILaunchpadLogRepository> _mockRepository;
        private LogMonitor _monitor;

        public LogMonitorTests(AutoMapperFixture fixture)
        {
            _mapper = fixture.MapperInstance;
            _mockRepository = Mock.Create<ILaunchpadLogRepository>();
            _monitor = new LogMonitor(_mockRepository.Object, _mapper);
        }

        [Fact]
        public void Ctor_Should_Throw_NullArgumentException_When_Repository_Is_Null()
        {
            Action throwAction = () => new LogMonitor(null, null);
            throwAction.ShouldThrow<ArgumentNullException>().
                And.
                ParamName.
                Should().
                Be("logRepository");

        }

        [Fact]
        public void Ctor_Should_Throw_NullArgumentException_When_Mapper_Is_Null()
        {
            Action throwAction = () => new LogMonitor(_mockRepository.Object, null);
            throwAction.ShouldThrow<ArgumentNullException>().
                And.
                ParamName.
                Should().
                Be("mapper");

        }

        [Fact]
        public void Name_Should_Return_Known_Value()
        {
            _monitor.Name.Should().Be("Log Monitor");
        }

        [Fact]
        public void MonitorType_Should_Return_Known_Value()
        {
            _monitor.Type.Should().Be(Models.Enum.MonitorType.Error);
        }

        [Fact]
        public void GetStatus_Should_Call_Repository()
        {
            var fakeLogs = Fixture.CreateMany<LaunchpadLog>();
            _mockRepository.Setup(_ => _.GetRecentActivity(10)).Returns(fakeLogs.AsQueryable());

            var status = _monitor.GetStatus();

            status.Should().NotBeNull().And.HaveCount(fakeLogs.Count());

        }
    }
}
