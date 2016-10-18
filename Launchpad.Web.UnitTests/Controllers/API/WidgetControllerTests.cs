using FluentAssertions;
using Launchpad.Models;
using Launchpad.Services.Interfaces;
using Launchpad.UnitTests.Common;
using Launchpad.Web.Controllers.API;
using Moq;
using Ploeh.AutoFixture;
using System.Linq;
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


    }
}
