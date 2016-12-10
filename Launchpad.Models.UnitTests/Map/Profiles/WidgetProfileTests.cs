using Launchpad.Models.UnitTests.Fixture;
using Launchpad.UnitTests.Common;
using Xunit;
using Ploeh.AutoFixture;
using FluentAssertions;
using Launchpad.Models.EntityFramework;

namespace Launchpad.Models.UnitTests.Map.Profiles
{
    [Collection(AutoMapperCollection.CollectionName)]
    public class WidgetProfileTests : BaseUnitTest
    {
        private readonly AutoMapperFixture _mappingFixture;

        public WidgetProfileTests(AutoMapperFixture mappingFixture)
        {
            _mappingFixture = mappingFixture;
        }

        [Fact]
        public void WidgetModel_Should_Map_To_Widget()
        {
            var widgetModel = Fixture.Create<WidgetModel>();

            var widget = _mappingFixture.MapperInstance.Map<Widget>(widgetModel);

            widget.Should().NotBeNull();
            widget.Name.Should().Be(widgetModel.Name);
            widget.Id.Should().Be(0);
        }

        [Fact]
        public void Widget_Should_Map_To_WidgetModel()
        {
            var widget = Fixture.Create<Widget>();

            var model = _mappingFixture.MapperInstance.Map<WidgetModel>(widget);

            model.Should().NotBeNull();
            model.Name.Should().Be(widget.Name);
            model.Id.Should().Be(widget.Id);
        }
    }
}
