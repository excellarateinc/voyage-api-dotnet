using Xunit;
using Moq;
using Launchpad.UnitTests.Common;
using Launchpad.Data.Interfaces;
using Launchpad.Models.EntityFramework;
using Ploeh.AutoFixture;
using System.Linq;
using FluentAssertions;
using AutoMapper;
using Launchpad.Services.Fixture;
using Launchpad.Models;

namespace Launchpad.Services.UnitTests
{
    [Collection(AutoMapperCollection.CollectionName)]
    public class WidgetServiceTests : BaseUnitTest
    {
        private WidgetService _widgetService;
        private Mock<IWidgetRepository> _mockWidgetRepository;
        private AutoMapperFixture _mappingFixture;

        public WidgetServiceTests(AutoMapperFixture mappingFixture)
        {
            _mappingFixture = mappingFixture;
            _mockWidgetRepository = Mock.Create<IWidgetRepository>();
            _widgetService = new WidgetService(_mockWidgetRepository.Object, _mappingFixture.MapperInstance);
        }

        [Fact]
        public void GetWidgets_Should_Call_WidgetRepository()
        {
            //Arrange
            var fakeWidgets = Fixture.CreateMany<Widget>().ToList();

            var w = Fixture.Create<Widget>();
            var mappedW = _mappingFixture.MapperInstance.Map<WidgetModel>(w);


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
