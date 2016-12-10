using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Routing;
using FluentAssertions;
using Launchpad.Models;
using Launchpad.Services.Interfaces;
using Launchpad.UnitTests.Common;
using Launchpad.Web.Controllers.API.V1;
using Moq;
using Ploeh.AutoFixture;
using Xunit;
using static Launchpad.Web.Constants;

namespace Launchpad.Web.UnitTests.Controllers.API.V1
{
    public class WidgetControllerTests : BaseUnitTest
    {
        private readonly Mock<IWidgetService> _mockWidgetService;
        private readonly WidgetController _widgetController;
        private readonly Mock<UrlHelper> _mockUrlHelper;
        private readonly string _testUrl;

        public WidgetControllerTests()
        {
            _testUrl = "http://test.com";
            _mockWidgetService = Mock.Create<IWidgetService>();
            _widgetController = new WidgetController(_mockWidgetService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            _mockUrlHelper = Mock.Create<UrlHelper>();
            _widgetController.Url = _mockUrlHelper.Object;
        }

        [Fact]
        public async void DeleteWidget_Should_Call_WidgetService_And_Return_No_Content()
        {
            const int id = 55;

            _mockWidgetService.Setup(_ => _.DeleteWidget(id)).Returns(new EntityResult(true, false));

            var result = _widgetController.DeleteWidget(id);

            var message = await result.ExecuteAsync(CancellationToken.None);
            Mock.VerifyAll();
            message.Should().NotBeNull();
            message.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public void Class_Should_Have_Authorize_Attribute()
        {
            typeof(WidgetController).Should()
             .BeDecoratedWith<AuthorizeAttribute>();
        }

        [Fact]
        public void Get_Should_Have_ClaimAuthorizeAttribute()
        {
            _widgetController.AssertClaim(_ => _.Get(), LssClaims.ListWidgets);
        }

        [Fact]
        public void GetById_Should_Have_ClaimAuthorizeAttribute()
        {
            _widgetController.AssertClaim(_ => _.Get(1), LssClaims.ViewWidget);
        }

        [Fact]
        public void AddWidget_Should_Have_ClaimAuthorizeAttribute()
        {
            _widgetController.AssertClaim(_ => _.AddWidget(new WidgetModel()), LssClaims.CreateWidget);
        }


        [Fact]
        public void DeleteWidget_Should_Have_ClaimAuthorizeAttribute()
        {
            _widgetController.AssertClaim(_ => _.DeleteWidget(1), LssClaims.DeleteWidget);
        }

        [Fact]
        public void UpdateWidget_Should_Have_ClaimAuthorizeAttribute()
        {
            _widgetController.AssertClaim(_ => _.UpdateWidget(1, new WidgetModel()), LssClaims.UpdateWidget);
        }


        [Fact]
        public async void AddWidget_Should_Call_WidgetService_And_Return_OK_When_Successful()
        {
            //Arrange
            var inputModel = Fixture.Create<WidgetModel>();
            var outputModel = Fixture.Create<WidgetModel>();
            var entityResult = new EntityResult<WidgetModel>(outputModel, true, false);
            _mockWidgetService.Setup(_ => _.AddWidget(inputModel)).Returns(entityResult);

            _mockUrlHelper
             .Setup(_ => _.Link("GetWidgetById", It.IsAny<Dictionary<string, object>>()))
             .Returns(_testUrl);

            //Act
            var result = _widgetController.AddWidget(inputModel);

            //Assert

            var message = await result.ExecuteAsync(CancellationToken.None);
            message.StatusCode.Should().Be(HttpStatusCode.Created);
            message.Headers.Location.Should().Be(_testUrl);

            WidgetModel widget;
            message.TryGetContentValue(out widget).Should().BeTrue();
            widget.ShouldBeEquivalentTo(outputModel);
        }

        [Fact]
        public async void UpdateWidget_Should_Call_WidgetService_And_Return_OK_When_Successful()
        {
            //Arrange
            var inputModel = Fixture.Create<WidgetModel>();
            var outputModel = Fixture.Create<WidgetModel>();
            var entityResult = new EntityResult<WidgetModel>(outputModel, true, false);
            _mockWidgetService.Setup(_ => _.UpdateWidget(inputModel.Id, inputModel)).Returns(entityResult);

            //Act
            var result = _widgetController.UpdateWidget(inputModel.Id, inputModel);

            //Assert
            Mock.VerifyAll();
            var message = await result.ExecuteAsync(CancellationToken.None);
            message.StatusCode.Should().Be(HttpStatusCode.OK);

            WidgetModel widget;
            message.TryGetContentValue(out widget).Should().BeTrue();
            widget.ShouldBeEquivalentTo(outputModel);
        }

        [Fact]
        public async void UpdateWidget_Should_Call_WidgetService_And_Return_NotFound_On_Failure()
        {
            //Arrange
            var fakeWidget = Fixture.Create<WidgetModel>();
            Fixture.Create<WidgetModel>();
            var entityResult = new EntityResult<WidgetModel>(null, false, true);
            _mockWidgetService.Setup(_ => _.UpdateWidget(fakeWidget.Id, fakeWidget)).Returns(entityResult);

            //Act
            var result = _widgetController.UpdateWidget(fakeWidget.Id, fakeWidget);

            //Assert
            Mock.VerifyAll();
            var message = await result.ExecuteAsync(CancellationToken.None);
            message.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async void Get_Should_Call_WidgetService()
        {
            //Arrange
            var fakeWidgets = Fixture.CreateMany<WidgetModel>().ToList();
            var entityResult = new EntityResult<IEnumerable<WidgetModel>>(fakeWidgets, true, false);

            _mockWidgetService.Setup(_ => _.GetWidgets()).Returns(entityResult);

            //Act
            var result = _widgetController.Get();

            //Assert
            var message = await result.ExecuteAsync(CancellationToken.None);
            IEnumerable<WidgetModel> widgets;
            message.TryGetContentValue(out widgets).Should().BeTrue();
            _mockWidgetService.Verify(_ => _.GetWidgets(), Times.Once());
            Mock.VerifyAll();
            widgets.Should().BeEquivalentTo(fakeWidgets);
        }

        [Fact]
        public async void GetById_Should_Call_WidgetService()
        {
            //Arrange
            var fakeWidget = Fixture.Create<WidgetModel>();
            var entityResult = new EntityResult<WidgetModel>(fakeWidget, true, false);

            _mockWidgetService.Setup(_ => _.GetWidget(fakeWidget.Id))
                .Returns(entityResult);

            //Act
            var result = _widgetController.Get(fakeWidget.Id);

            //Assert
            _mockWidgetService.Verify(_ => _.GetWidget(fakeWidget.Id), Times.Once());
            var message = await result.ExecuteAsync(CancellationToken.None);
            Mock.VerifyAll();

            WidgetModel widget;
            message.TryGetContentValue(out widget).Should().BeTrue(); //Deserialize response content
            widget.ShouldBeEquivalentTo(fakeWidget);
        }

        [Fact]
        public async void Get_By_Id_Should_Return_404_When_Widget_Not_Found()
        {
            //Arrange
            const int id = -1;

            var entityResult = new EntityResult<WidgetModel>(null, false, true);
            _mockWidgetService.Setup(_ => _.GetWidget(id)).Returns(entityResult);

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
            Action throwAction = () => new WidgetController(null);
            throwAction.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("widgetService");
        }

        [Fact]
        public void DeleteWidget_Should_Have_HttpDelete_Attribute()
        {
            ReflectionHelper.GetMethod<WidgetController>(_ => _.DeleteWidget(1))
                .Should()
                .BeDecoratedWith<HttpDeleteAttribute>();
        }

        [Fact]
        public void AddWidget_Should_Have_HttpPost_Attribute()
        {
            ReflectionHelper.GetMethod<WidgetController>(_ => _.AddWidget(new WidgetModel()))
                .Should()
                .BeDecoratedWith<HttpPostAttribute>();
        }

        [Fact]
        public void UpdateWidget_Should_Have_HttpPut_Attribute()
        {
            ReflectionHelper.GetMethod<WidgetController>(_ => _.UpdateWidget(1, new WidgetModel()))
                .Should()
                .BeDecoratedWith<HttpPutAttribute>();
        }

        [Fact]
        public void Class_Should_Have_V1RoutePrefix_Attribute()
        {
            typeof(WidgetController).Should()
                .BeDecoratedWith<RoutePrefixAttribute>(
                _ => _.Prefix.Equals(RoutePrefixes.V1));
        }

        [Fact]
        public void Get_Should_Have_Route_Attribute()
        {
            ReflectionHelper.GetMethod<WidgetController>(_ => _.Get())
                .Should()
                .BeDecoratedWith<RouteAttribute>(_ => _.Template.Equals("widgets"));
        }

        [Fact]
        public void GetById_Should_Have_Route_Attribute()
        {
            ReflectionHelper.GetMethod<WidgetController>(_ => _.Get(1))
                    .Should()
                    .BeDecoratedWith<RouteAttribute>(_ => _.Template.Equals("widgets/{id:int}"));
        }

        [Fact]
        public void AddWidget_Should_Have_Route_Attribute()
        {
            ReflectionHelper.GetMethod<WidgetController>(_ => _.AddWidget(new WidgetModel()))
                    .Should()
                    .BeDecoratedWith<RouteAttribute>(_ => _.Template.Equals("widgets"));
        }

        [Fact]
        public void Delete_Should_Have_Route_Attribute()
        {
            ReflectionHelper.GetMethod<WidgetController>(_ => _.DeleteWidget(1))
                    .Should()
                    .BeDecoratedWith<RouteAttribute>(_ => _.Template.Equals("widgets/{id:int}"));
        }

        [Fact]
        public void UpdateWidget_Should_Have_Route_Attribute()
        {
            ReflectionHelper.GetMethod<WidgetController>(_ => _.UpdateWidget(1, new WidgetModel()))
                    .Should()
                    .BeDecoratedWith<RouteAttribute>(_ => _.Template.Equals("widgets/{id:int}"));
        }
    }
}
