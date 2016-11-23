using FluentAssertions;
using FluentValidation.Attributes;
using Launchpad.Models.Validators;
using Xunit;

namespace Launchpad.Models.UnitTests
{
    public class WidgetModelTests
    {
        [Fact]
        public void Class_Should_Have_Validator_Class()
        {
            typeof(WidgetModel)
                .Should()
                .BeDecoratedWith<ValidatorAttribute>(_ => _.ValidatorType == typeof(WidgetModelValidator));
        }
    }
}
