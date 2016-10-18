using Xunit;
using Moq;
using Launchpad.UnitTests.Common;
using Launchpad.Data.Interfaces;
using Launchpad.Models.EntityFramework;
using Ploeh.AutoFixture;
using System.Linq;
using FluentAssertions;
using Launchpad.Services.Fixture;
using System;

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

        [Fact]
        public void GetWidget_Should_Call_WidgetRepository()
        {
            //Arrange
            var fakeWidget = Fixture.Create<Widget>();

            _mockWidgetRepository.Setup(_ => _.Get(fakeWidget.Id)).Returns(fakeWidget);

            //Act
            var widget = _widgetService.GetWidget(fakeWidget.Id);

            //Assert
            Mock.VerifyAll();
            widget.Name.Should().Be(fakeWidget.Name);
            widget.Id.Should().Be(fakeWidget.Id);

        }
        
        [Fact]
        public void GetWidget_Should_Return_Null_When_Id_Not_Found()
        {
            //Arrange
            Widget fakeWidget = null;
            const int id = -1;

            _mockWidgetRepository.Setup(_ => _.Get(id)).Returns(fakeWidget);

            //Act
            var widget = _widgetService.GetWidget(id);

            //Assert
            Mock.VerifyAll();
            widget.Should().BeNull();
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_Repository_Is_Null()
        {
            Action throwAction = () => new WidgetService(null, null);
            throwAction.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("widgetRepository");
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_Mapper_Is_Null()
        {
            Action throwAction = () => new WidgetService(_mockWidgetRepository.Object, null);
            throwAction.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("mapper");
        }
    }
}
