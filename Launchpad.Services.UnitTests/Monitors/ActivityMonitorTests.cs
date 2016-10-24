using AutoMapper;
using FluentAssertions;
using Launchpad.Models.Enum;
using Launchpad.Services.Fixture;
using Launchpad.Services.Interfaces;
using Launchpad.Services.Monitors;
using Launchpad.UnitTests.Common;
using Moq;
using System;
using Xunit;
using Ploeh.AutoFixture;
using Launchpad.Models;
using System.Linq;

namespace Launchpad.Services.UnitTests.Monitors
{

    [Collection(AutoMapperCollection.CollectionName)]
    public class ActivityMonitorTests : BaseUnitTest
    {
        private readonly ActivityMonitor _monitor;
        private readonly Mock<IRequestMetricsService> _mockMetricsService;
      
        public ActivityMonitorTests(AutoMapperFixture autoMapperFixture)
        {
            _mockMetricsService = Mock.Create<IRequestMetricsService>();
      
            _monitor = new ActivityMonitor(_mockMetricsService.Object, autoMapperFixture.MapperInstance);
        }

        [Fact]
        public void GetStatus_Should_Call_MetricsService()
        {
            var fakeMetrics = Fixture.CreateMany<RequestDataPointModel>();
            _mockMetricsService.Setup(_ => _.GetActivity()).Returns(fakeMetrics);

            var status = _monitor.GetStatus();

            Mock.VerifyAll();
            status.Should().HaveCount(fakeMetrics.Count());

        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_MetricsService_IsNull()
        {
            Action throwAction = () => new ActivityMonitor(null, null);
            throwAction.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("metricsService");
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_Mapper_IsNull()
        {
            Action throwAction = () => new ActivityMonitor(_mockMetricsService.Object, null);
            throwAction.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("mapper");
        }


        [Fact]
        public void Name_Should_Return_Known_Value()
        {
            _monitor.Name.Should().Be("Activity");
        }

        [Fact]
        public void Type_Should_Return_Known_Value()
        {
            _monitor.Type.Should().Be(MonitorType.Activity);
        }
    }
}
