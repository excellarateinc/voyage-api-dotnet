using FluentAssertions;
using Launchpad.Models;
using Launchpad.Services.Interfaces;
using Launchpad.UnitTests.Common;
using Launchpad.Web.Controllers.API.V2;
using Moq;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using Xunit;
using static Launchpad.Web.Constants;

namespace Launchpad.Web.UnitTests.Controllers.API.V2
{
    /// <summary>
    /// This is not DRY - but for now, better to have tests than not 
    /// </summary>
    public class WidgetV2ControllerTests : BaseUnitTest
    {
        private Mock<IWidgetService> _mockWidgetService;
        private WidgetV2Controller _widgetController;

        public WidgetV2ControllerTests()
        {
            _mockWidgetService = Mock.Create<IWidgetService>();
            _widgetController = new WidgetV2Controller(_mockWidgetService.Object);
            _widgetController.Request = new System.Net.Http.HttpRequestMessage();
            _widgetController.Configuration = new System.Web.Http.HttpConfiguration();

        }

        [Fact]
        public void Get_Should_Have_ClaimAuthorizeAttribute()
        {
            _widgetController.AssertClaim(_ => _.Get(), LssClaims.ListWidgets);
        }

        [Fact]
        public async void DeleteWidget_Should_Call_WidgetService_And_Return_No_Content()
        {
            const int id = 55;

            _mockWidgetService.Setup(_ => _.DeleteWidget(id));

            var result = _widgetController.DeleteWidget(id);

            var message = await result.ExecuteAsync(CancellationToken.None);

            Mock.VerifyAll();
            message.Should().NotBeNull();
            message.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async void AddWidget_Should_Call_WidgetService_And_Return_OK_When_Successful()
        {
            //Arrange
            var fakeWidget = Fixture.Create<WidgetModel>();
            var fakeResult = Fixture.Create<WidgetModel>();
            _mockWidgetService.Setup(_ => _.AddWidget(fakeWidget)).Returns(fakeResult);

            //Act
            var result = _widgetController.AddWidget(fakeWidget);

            //Assert
            Mock.VerifyAll();
            var message = await result.ExecuteAsync(CancellationToken.None);

            message.StatusCode.Should().Be(HttpStatusCode.Created);

            WidgetModel widget;
            message.TryGetContentValue(out widget).Should().BeTrue();
            widget.ShouldBeEquivalentTo(fakeResult);
        }

        [Fact]
        public async void UpdateWidget_Should_Call_WidgetService_And_Return_OK_When_Successful()
        {
            //Arrange
            var fakeWidget = Fixture.Create<WidgetModel>();
            var fakeResult = Fixture.Create<WidgetModel>();
            _mockWidgetService.Setup(_ => _.UpdateWidget(fakeWidget)).Returns(fakeResult);

            //Act
            var result = _widgetController.UpdateWidget(fakeWidget);

            //Assert
            Mock.VerifyAll();
            var message = await result.ExecuteAsync(CancellationToken.None);

            message.StatusCode.Should().Be(HttpStatusCode.OK);

            WidgetModel widget;
            message.TryGetContentValue(out widget).Should().BeTrue();
            widget.ShouldBeEquivalentTo(fakeResult);
        }

        [Fact]
        public async void UpdateWidget_Should_Call_WidgetService_And_Return_NotFound_On_Failure()
        {
            //Arrange
            var fakeWidget = Fixture.Create<WidgetModel>();
            WidgetModel fakeResult = null;
            _mockWidgetService.Setup(_ => _.UpdateWidget(fakeWidget)).Returns(fakeResult);

            //Act
            var result = _widgetController.UpdateWidget(fakeWidget);

            //Assert
            Mock.VerifyAll();
            var message = await result.ExecuteAsync(CancellationToken.None);

            message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        }


        [Fact]
        public async void Get_Should_Call_WidgetService()
        {
            //Arrange
            var fakeWidgets = Fixture.CreateMany<WidgetModel>();

            _mockWidgetService.Setup(_ => _.GetWidgets()).Returns(fakeWidgets.AsQueryable());

            //Act
            var result = _widgetController.Get();

            //Assert
            _mockWidgetService.Verify(_ => _.GetWidgets(), Times.Once());
            var message = await result.ExecuteAsync(CancellationToken.None);
            IEnumerable<WidgetModel> widgets;
            message.TryGetContentValue(out widgets);
            Mock.VerifyAll();
            widgets.Should().BeEquivalentTo(fakeWidgets);

        }

        [Fact]
        public async void Get_By_Id_Should_Call_WidgetService()
        {
            //Arrange

            var fakeWidget = Fixture.Create<WidgetModel>();

            _mockWidgetService.Setup(_ => _.GetWidget(fakeWidget.Id)).Returns(fakeWidget);

            //Act
            var result = _widgetController.Get(fakeWidget.Id);

            //Assert
            _mockWidgetService.Verify(_ => _.GetWidget(fakeWidget.Id), Times.Once());
            Mock.VerifyAll();
            var message = await result.ExecuteAsync(CancellationToken.None);


            WidgetModel widget;
            message.TryGetContentValue<WidgetModel>(out widget).Should().BeTrue(); //Deserialize response content
            widget.ShouldBeEquivalentTo(fakeWidget);

        }

        [Fact]
        public async void Get_By_Id_Should_Return_404_When_Widget_Not_Found()
        {
            //Arrange
            const int id = -1;
            WidgetModel model = null;
            _mockWidgetService.Setup(_ => _.GetWidget(id)).Returns(model);

            //Act
            var result = _widgetController.Get(id);

            //Assert
            Mock.VerifyAll();
            var message = await result.ExecuteAsync(CancellationToken.None);

            message.StatusCode.Should().Be(HttpStatusCode.NotFound);

        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_Service_Is_Null()
        {
            Action throwAction = () => new WidgetV2Controller(null);
            throwAction.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("widgetService");
        }

        [Fact]
        public void DeleteWidget_Should_Have_HttpDelete_Attribute()
        {
            ReflectionHelper.GetMethod<WidgetV2Controller>(_ => _.DeleteWidget(1))
                .Should()
                .BeDecoratedWith<HttpDeleteAttribute>();
        }

        [Fact]
        public void AddWidget_Should_Have_HttpPost_Attribute()
        {
            ReflectionHelper.GetMethod<WidgetV2Controller>(_ => _.AddWidget(new WidgetModel()))
                .Should()
                .BeDecoratedWith<HttpPostAttribute>();
        }

        [Fact]
        public void UpdateWidget_Should_Have_HttpPut_Attribute()
        {
            ReflectionHelper.GetMethod<WidgetV2Controller>(_ => _.UpdateWidget(new WidgetModel()))
                .Should()
                .BeDecoratedWith<HttpPutAttribute>();
        }

        [Fact]
        public void Class_Should_Have_V2_RoutePrefix_Attribute()
        {
            typeof(WidgetV2Controller).Should()
                .BeDecoratedWith<RoutePrefixAttribute>(
                _ => _.Prefix.Equals(Constants.RoutePrefixes.V2));
        }

        [Fact]
        public void Get_Should_Have_Route_Attribute()
        {
            ReflectionHelper.GetMethod<WidgetV2Controller>(_ => _.Get())
                .Should()
                .BeDecoratedWith<RouteAttribute>(_ => _.Template.Equals("widget"));
        }

        [Fact]
        public void GetById_Should_Have_Route_Attribute()
        {
            ReflectionHelper.GetMethod<WidgetV2Controller>(_ => _.Get(1))
                    .Should()
                    .BeDecoratedWith<RouteAttribute>(_ => _.Template.Equals("widget/{id:int}"));

        }

        [Fact]
        public void AddWidget_Should_Have_Route_Attribute()
        {
            ReflectionHelper.GetMethod<WidgetV2Controller>(_ => _.AddWidget(new WidgetModel()))
                    .Should()
                    .BeDecoratedWith<RouteAttribute>(_ => _.Template.Equals("widget"));

        }

        [Fact]
        public void Delete_Should_Have_Route_Attribute()
        {
            ReflectionHelper.GetMethod<WidgetV2Controller>(_ => _.DeleteWidget(1))
                    .Should()
                    .BeDecoratedWith<RouteAttribute>(_ => _.Template.Equals("widget/{id:int}"));

        }

        [Fact]
        public void UpdateWidget_Should_Have_Route_Attribute()
        {
            ReflectionHelper.GetMethod<WidgetV2Controller>(_ => _.UpdateWidget(new WidgetModel()))
                    .Should()
                    .BeDecoratedWith<RouteAttribute>(_ => _.Template.Equals("widget"));

        }

    }
}
