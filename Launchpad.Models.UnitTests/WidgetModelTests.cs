using FluentAssertions;
using Xunit;

namespace Launchpad.Models.UnitTests
{
    public class WidgetModelTests
    {
        [Fact]
        public void Name_Is_Required()
        {
            var model = new WidgetModel() { Name = null };
            var result = model.RunValidations();

            result.Item1.Should().BeFalse();
            result.Item2.Should().HaveCount(1);
            result.Item2[0].MemberNames.Should().Contain("Name");
        }
    }
}
