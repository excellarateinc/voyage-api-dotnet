using FluentAssertions;
using Launchpad.Services.Interfaces;
using Launchpad.UnitTests.Common;
using Launchpad.Web.Controllers.API.V1;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Xunit;
using Launchpad.Models;
using System.Net;
using System.Net.Http;
using Launchpad.Models.Enum;
using System.Web.Http;
using System.Web.Http.Results;
using System.Threading;

namespace Launchpad.Web.UnitTests.Controllers.API.V1
{
    public class StatusControllerTests : BaseUnitTest
    {
        private StatusController _controller;
        private Mock<IStatusCollector> _mockStatusCollector;

        public StatusControllerTests()
        {
            _mockStatusCollector = Mock.Create<IStatusCollector>();
            _controller = new StatusController(_mockStatusCollector.Object);
            _controller.Request = new System.Net.Http.HttpRequestMessage();
            _controller.Configuration = new System.Web.Http.HttpConfiguration();
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_when_StatusCollector_Is_Null()
        {
            Action throwAction = () => new StatusController(null);

            throwAction.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("statusCollector");
        }

        [Fact]
        public async void Get_Should_Call_StatusCollection_With_MontiorTypeParameter()
        {
            //Arrange
            var fakeStatuses = Fixture.CreateMany<StatusAggregateModel>().ToList();
            MonitorType type = MonitorType.Error;
            _mockStatusCollector.Setup(_ => _.Collect(type)).Returns(fakeStatuses);
            
            //Act
            var result = _controller.Get(type);


            //Assert
            Mock.VerifyAll();

            result.Should().BeOfType<OkNegotiatedContentResult<IEnumerable<StatusAggregateModel>>>();

            var message = await result.ExecuteAsync(CancellationToken.None);

            message.StatusCode.Should().Be(HttpStatusCode.OK);
            IEnumerable<StatusAggregateModel> models;
            message.TryGetContentValue(out models).Should().BeTrue();
            models.ShouldBeEquivalentTo(fakeStatuses);

        }

        [Fact]
        public async void Get_Should_Call_StatusCollector()
        {
            //Arrange
            var fakeStatuses = Fixture.CreateMany<StatusAggregateModel>().ToList();
            _mockStatusCollector.Setup(_ => _.Collect()).Returns(fakeStatuses);

            //Act
            var result = _controller.Get();


            //Assert
            Mock.VerifyAll();

            result.Should().BeOfType<OkNegotiatedContentResult<IEnumerable<StatusAggregateModel>>>();


            var message = await result.ExecuteAsync(CancellationToken.None);

            message.StatusCode.Should().Be(HttpStatusCode.OK);

            IEnumerable<StatusAggregateModel> models;
            message.TryGetContentValue(out models).Should().BeTrue();
            models.ShouldBeEquivalentTo(fakeStatuses);
        }

        [Fact]
        public void Class_Should_Have_V1RoutePrefix_Attribute()
        {
            typeof(StatusController).Should()
                .BeDecoratedWith<RoutePrefixAttribute>(
                _ => _.Prefix.Equals(Constants.RoutePrefixes.V1));
        }

        [Fact]
        public void GetByMonitorType_Should_Have_Route_Attribute()
        {
            ReflectionHelper.GetMethod<StatusController>(_ => _.Get(MonitorType.Activity))
                .Should()
                .BeDecoratedWith<RouteAttribute>(_ => _.Template.Equals("statuses/{id:int}"));
        }

        [Fact]
        public void Get_Should_Have_Route_Attribute()
        {
            ReflectionHelper.GetMethod<StatusController>(_ => _.Get())
             .Should()
             .BeDecoratedWith<RouteAttribute>(_ => _.Template.Equals("statuses"));
        }
    }
}
