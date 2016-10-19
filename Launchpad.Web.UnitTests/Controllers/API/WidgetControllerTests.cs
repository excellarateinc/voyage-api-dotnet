using FluentAssertions;
using Launchpad.Models;
using Launchpad.Services.Interfaces;
using Launchpad.UnitTests.Common;
using Launchpad.Web.Controllers.API;
using Moq;
using Ploeh.AutoFixture;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Launchpad.Web.UnitTests.Controllers.API
{
    public class WidgetControllerTests : BaseUnitTest
    {
        private Mock<IWidgetService> _mockWidgetService;
        private WidgetController _widgetController;

        public WidgetControllerTests()
        {
            _mockWidgetService = Mock.Create<IWidgetService>();
            _widgetController = new WidgetController(_mockWidgetService.Object);
            _widgetController.Request = new System.Net.Http.HttpRequestMessage();
            _widgetController.Configuration = new System.Web.Http.HttpConfiguration();

        }

        [Fact]
        public void Get_Should_Call_WidgetService()
        {
            //Arrange
            var fakeWidgets = Fixture.CreateMany<WidgetModel>();

            _mockWidgetService.Setup(_ => _.GetWidgets()).Returns(fakeWidgets.AsQueryable());

            //Act
            var widgets = _widgetController.Get();

            //Assert
            _mockWidgetService.Verify(_ => _.GetWidgets(), Times.Once());
            Mock.VerifyAll();
            widgets.Should().BeEquivalentTo(fakeWidgets);

        }

        [Fact]
        public void Get_By_Id_Should_Call_WidgetService()
        {
            //Arrange

            var fakeWidget = Fixture.Create<WidgetModel>();

            _mockWidgetService.Setup(_ => _.GetWidget(fakeWidget.Id)).Returns(fakeWidget);

            //Act
            var response = _widgetController.Get(fakeWidget.Id);

            //Assert
            _mockWidgetService.Verify(_ => _.GetWidget(fakeWidget.Id), Times.Once());
            Mock.VerifyAll();


            WidgetModel widget;
            response.TryGetContentValue<WidgetModel>(out widget).Should().BeTrue(); //Deserialize response content
            widget.ShouldBeEquivalentTo(fakeWidget);

        }

        [Fact]
        public void Get_By_Id_Should_Return_404_When_Widget_Not_Found()
        {
            //Arrange
            const int id = -1;
            WidgetModel model = null;
            _mockWidgetService.Setup(_ => _.GetWidget(id)).Returns(model);

            //Act
            var response = _widgetController.Get(id);

            //Assert
            Mock.VerifyAll();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_Service_Is_Null()
        {
            Action throwAction = () => new WidgetController(null);
            throwAction.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("widgetService");
        }
    }
}
