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

        [Fact]
        public void AddWidget_Should_Call_Repository()
        {
            var model = Fixture.Create<WidgetModel>();
            var addResult = Fixture.Create<Widget>();

            _mockWidgetRepository.Setup(_ => _.Add(It.IsAny<Widget>())).Returns(addResult);

            var result = _widgetService.AddWidget(model);

            Mock.VerifyAll();
            result.Should().NotBeNull();
            result.Name.Should().Be(addResult.Name);
            result.Id.Should().Be(addResult.Id);
        }

        [Fact]
        public void UpdateWidget_Should_Call_Repository_And_Return_Null_When_Not_Found()
        {
            var model = Fixture.Create<WidgetModel>();
            Widget getResult = null;
            _mockWidgetRepository.Setup(_ => _.Get(model.Id)).Returns(getResult);

            var result = _widgetService.UpdateWidget(model);

            Mock.VerifyAll();
            result.Should().BeNull();
        }
        
        [Fact]
        public void UpdateWidget_Should_Call_Repository_And_Return_Model_When_Found()
        {
            var model = Fixture.Create<WidgetModel>();
            Widget getResult = Fixture.Create<Widget>();
            _mockWidgetRepository.Setup(_ => _.Get(model.Id)).Returns(getResult);
            _mockWidgetRepository.Setup(_ => _.Update(getResult)).Returns(getResult);

            var result = _widgetService.UpdateWidget(model);

            Mock.VerifyAll();
            result.Should().NotBeNull();
            result.Name.Should().Be(getResult.Name);
            result.Id.Should().Be(getResult.Id);

        }

        [Fact]
        public void DeleteWidget_Should_Call_Repository()
        {
            const int id = 33;
            _mockWidgetRepository.Setup(_ => _.Delete(id));

            _widgetService.DeleteWidget(id);

            Mock.VerifyAll();
        }
    }
}
