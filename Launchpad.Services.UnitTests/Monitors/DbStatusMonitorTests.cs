using FluentAssertions;
using Launchpad.Data.Interfaces;
using Launchpad.Models.Enum;
using Launchpad.Services.Monitors;
using Launchpad.UnitTests.Common;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Launchpad.Services.UnitTests.Monitors
{
    public class DbStatusMonitorTests : BaseUnitTest
    {
        DbStatusMonitor _monitor;
        Mock<IDbConnectionStatus> _mockStatus1;
        Mock<IDbConnectionStatus> _mockStatus2;

        public DbStatusMonitorTests()
        {
            _mockStatus1 = Mock.Create<IDbConnectionStatus>();
            _mockStatus2 = Mock.Create<IDbConnectionStatus>();

            _monitor = new DbStatusMonitor(new[] { _mockStatus1.Object, _mockStatus2.Object });
        }
        
        [Fact]
        public void GetStaus_Should_Call_ConnectionStatuses()
        {
            _mockStatus1.Setup(_ => _.Test()).Returns(true);
            _mockStatus1.SetupGet(_ => _.DisplayName).Returns("Status1");

            _mockStatus2.Setup(_ => _.Test()).Returns(false);
            _mockStatus2.SetupGet(_ => _.DisplayName).Returns("Status2");

            var status = _monitor.GetStatus().ToList();

            Mock.VerifyAll();

            status.Should().HaveCount(2);
            status[0].Code.Should().Be(StatusCode.OK);
            status[0].Message.Should().Be("Status1 -> Connected");

            status[1].Code.Should().Be(StatusCode.Critical);
            status[1].Message.Should().Be("Status2 -> Failed");
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_ConnectionStatuses_Are_Null()
        {
            Action throwAction = () => new DbStatusMonitor(null);
            throwAction.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("connectionStatuses");
        }

        [Fact]
        public void Name_Should_Return_Known_Value()
        {
            _monitor.Name.Should().Be("Database Status");
        }

        [Fact]
        public void Type_Should_Return_Known_Value()
        {
            _monitor.Type.Should().Be(MonitorType.Database);
        }

    }
}
