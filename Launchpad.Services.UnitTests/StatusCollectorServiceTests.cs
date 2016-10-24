using Launchpad.Models;
using Launchpad.Services.Interfaces;
using Launchpad.UnitTests.Common;
using Moq;
using System.Linq;
using Xunit;
using Ploeh.AutoFixture;
using FluentAssertions;
using System;

namespace Launchpad.Services.UnitTests
{
    public class StatusCollectorServiceTests : BaseUnitTest
    {
        private const string _monitor1Name = "Monitor1";
        private const string _monitor2Name = "Monitor2";
        private StatusCollectorService _collector;
        private Mock<IStatusMonitor> _mockDbMonitor;
        private Mock<IStatusMonitor> _mockErrorMonitor;


        public StatusCollectorServiceTests()
        {
            _mockDbMonitor = Mock.Create<IStatusMonitor>();
            _mockErrorMonitor = Mock.Create<IStatusMonitor>();
            _collector = new StatusCollectorService(new[] { _mockDbMonitor.Object, _mockErrorMonitor.Object });
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_Monitors_Is_Null()
        {
            Action throwAction = () => new StatusCollectorService(null);

            throwAction.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("monitors");
        }

        [Fact]
        public void Collect_Should_Call_Monitors()
        {
            _mockDbMonitor.SetupGet(_ => _.Name).Returns(_monitor1Name);
            _mockDbMonitor.SetupGet(_ => _.Type).Returns(Models.Enum.MonitorType.Database);

            _mockErrorMonitor.SetupGet(_ => _.Name).Returns(_monitor2Name);
            _mockErrorMonitor.SetupGet(_ => _.Type).Returns(Models.Enum.MonitorType.Error);

            var monitor1Results = Fixture.CreateMany<StatusModel>();
            _mockDbMonitor.Setup(_ => _.GetStatus()).Returns(monitor1Results);

            var monitor2Results = Fixture.CreateMany<StatusModel>();
            _mockErrorMonitor.Setup(_ => _.GetStatus()).Returns(monitor2Results);

            var result = _collector.Collect().ToList();

            result.Should().HaveCount(2);
            result[0].Name.Should().Be(_monitor1Name);
            result[0].Type.Should().Be(Models.Enum.MonitorType.Database);
            result[0].Status.ShouldBeEquivalentTo(monitor1Results);

            result[1].Name.Should().Be(_monitor2Name);
            result[1].Type.Should().Be(Models.Enum.MonitorType.Error);
            result[1].Status.ShouldBeEquivalentTo(monitor2Results);
        }

        [Fact]
        public void Collect_Should_Filter_And_Call_Monitors()
        {
            var filter = Models.Enum.MonitorType.Error;

            _mockDbMonitor.SetupGet(_ => _.Type).Returns(Models.Enum.MonitorType.Database);

            _mockErrorMonitor.SetupGet(_ => _.Name).Returns(_monitor2Name);
            _mockErrorMonitor.SetupGet(_ => _.Type).Returns(Models.Enum.MonitorType.Error);

            var monitor2Results = Fixture.CreateMany<StatusModel>();
            _mockErrorMonitor.Setup(_ => _.GetStatus()).Returns(monitor2Results);

            var result = _collector.Collect(filter).ToList();

            result.Should().HaveCount(1);
            result[0].Name.Should().Be(_monitor2Name);
            result[0].Type.Should().Be(Models.Enum.MonitorType.Error);
            result[0].Status.ShouldBeEquivalentTo(monitor2Results);
        }
    }
}
