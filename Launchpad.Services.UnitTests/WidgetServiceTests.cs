using Xunit;
using Moq;
using Launchpad.UnitTests.Common;
using Launchpad.Data.Interfaces;
using Launchpad.Models;
using Ploeh.AutoFixture;
using System.Linq;
using FluentAssertions;

namespace Launchpad.Services.UnitTests
{

    public class WidgetServiceTests : BaseUnitTest
    {
        private WidgetService _widgetService;
        private Mock<IWidgetRepository> _mockWidgetRepository;

        public WidgetServiceTests()
        {
            _mockWidgetRepository = Mock.Create<IWidgetRepository>();
            _widgetService = new WidgetService(_mockWidgetRepository.Object);
        }

        [Fact]
        public void GetWidgets_Should_Call_WidgetRepository()
        {
            //Arrange
            var fakeWidgets = Fixture.CreateMany<Widget>().ToList();

            _mockWidgetRepository.Setup(_ => _.GetAll())
                .Returns(fakeWidgets.AsQueryable());
         
            //Act
            var widgets = _widgetService.GetWidgets();

            //Assert

            //Verify the number of calls to the repo
            _mockWidgetRepository.Verify(_ => _.GetAll(), Times.Once());

            //Verify any expectations setup on mocks created from the factory
            Mock.VerifyAll();

            //Verify the data
            widgets.Should().HaveSameCount(fakeWidgets);
            widgets.All(_ => fakeWidgets.Any(fake => fake.Id.Equals(_.Id) && fake.Name.Equals(_.Name))).Should().BeTrue();
            

        }
    }
}
