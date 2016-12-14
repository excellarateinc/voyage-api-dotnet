using System;
using System.Linq;
using FluentAssertions;
using Launchpad.Data.Interfaces;
using Launchpad.Models;
using Launchpad.Models.EntityFramework;
using Launchpad.Services.UnitTests.Fixture;
using Launchpad.UnitTests.Common;
using Moq;
using Ploeh.AutoFixture;
using Xunit;

namespace Launchpad.Services.UnitTests
{
    [Collection(AutoMapperCollection.CollectionName)]
    public class WidgetServiceTests : BaseUnitTest
    {
        private readonly WidgetService _widgetService;
        private readonly Mock<IWidgetRepository> _mockWidgetRepository;
        private readonly AutoMapperFixture _mappingFixture;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public WidgetServiceTests(AutoMapperFixture mappingFixture)
        {
            _mappingFixture = mappingFixture;
            _mockUnitOfWork = Mock.Create<IUnitOfWork>();
            _mockWidgetRepository = Mock.Create<IWidgetRepository>();
            _widgetService = new WidgetService(_mockWidgetRepository.Object, _mappingFixture.MapperInstance, _mockUnitOfWork.Object);
        }

        [Fact]
        public void GetWidgets_Should_Call_WidgetRepository()
        {
            // Arrange
            var fakeWidgets = Fixture.CreateMany<Widget>().ToList();

            _mockWidgetRepository.Setup(_ => _.GetAll())
                .Returns(fakeWidgets.AsQueryable());

            // Act
            var entityResult = _widgetService.GetWidgets();

            // Assert

            // Verify the number of calls to the repo
            _mockWidgetRepository.Verify(_ => _.GetAll(), Times.Once());

            // Verify any expectations setup on mocks created from the factory
            Mock.VerifyAll();

            // Verify the data
            entityResult.Model.Should().HaveSameCount(fakeWidgets);
            entityResult.Model.All(_ => fakeWidgets.Any(fake => fake.Id.Equals(_.Id) && fake.Name.Equals(_.Name))).Should().BeTrue();
        }

        [Fact]
        public void GetWidget_Should_Call_WidgetRepository()
        {
            // Arrange
            var fakeWidget = Fixture.Create<Widget>();

            _mockWidgetRepository.Setup(_ => _.Get(fakeWidget.Id)).Returns(fakeWidget);

            // Act
            var entityResult = _widgetService.GetWidget(fakeWidget.Id);

            // Assert
            Mock.VerifyAll();
            entityResult.Model.Name.Should().Be(fakeWidget.Name);
            entityResult.Model.Id.Should().Be(fakeWidget.Id);
        }

        [Fact]
        public void GetWidget_Should_Return_IsEntityNotFound_True()
        {
            // Arrange
            const int id = -1;

            _mockWidgetRepository
                .Setup(_ => _.Get(id))
                .Returns((Widget)null);

            // Act
            var widget = _widgetService.GetWidget(id);

            // Assert
            Mock.VerifyAll();
            widget.IsEntityNotFound.Should().BeTrue();
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_Repository_Is_Null()
        {
            Action throwAction = () => new WidgetService(null, _mappingFixture.MapperInstance, null);
            throwAction.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("widgetRepository");
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_Mapper_Is_Null()
        {
            Action throwAction = () => new WidgetService(_mockWidgetRepository.Object, null, null);
            throwAction.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("mapper");
        }

        [Fact]
        public void AddWidget_Should_Call_Repository()
        {
            var model = Fixture.Create<WidgetModel>();
            var addResult = Fixture.Create<Widget>();

            _mockWidgetRepository.Setup(_ => _.Add(It.IsAny<Widget>())).Returns(addResult);

            var entityResult = _widgetService.AddWidget(model);

            Mock.VerifyAll();
            entityResult.Should().NotBeNull();
            entityResult.Model.Name.Should().Be(addResult.Name);
            entityResult.Model.Id.Should().Be(addResult.Id);
        }

        [Fact]
        public void UpdateWidget_Should_Call_Repository_And_Return_IsEntityNotFound_True()
        {
            var model = Fixture.Create<WidgetModel>();
            new EntityResult<WidgetModel>(null, false, true);
            _mockWidgetRepository.Setup(_ => _.Get(model.Id)).Returns((Widget)null);

            var result = _widgetService.UpdateWidget(model.Id, model);

            Mock.VerifyAll();
            result.IsEntityNotFound.Should().BeTrue();
        }

        [Fact]
        public void UpdateWidget_Should_Call_Repository_And_Return_Model_When_Found()
        {
            var model = Fixture.Create<WidgetModel>();
            Widget getResult = Fixture.Create<Widget>();
            _mockWidgetRepository.Setup(_ => _.Get(model.Id)).Returns(getResult);
            _mockWidgetRepository.Setup(_ => _.Update(getResult)).Returns(getResult);

            var entityResult = _widgetService.UpdateWidget(model.Id, model);

            Mock.VerifyAll();
            entityResult.Model.Should().NotBeNull();
            entityResult.Model.Name.Should().Be(getResult.Name);
            entityResult.Model.Id.Should().Be(getResult.Id);
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
