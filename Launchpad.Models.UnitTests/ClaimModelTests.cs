using FluentAssertions;
using FluentValidation.Attributes;
using Launchpad.Models.Validators;
using Xunit;

namespace Launchpad.Models.UnitTests
{
    public class ClaimModelTests
    {
        [Fact]
        public void Class_Should_Have_Validator_Class()
        {
            typeof(ClaimModel)
                .Should()
                .BeDecoratedWith<ValidatorAttribute>(_ => _.ValidatorType == typeof(ClaimModelValidator));
        }
    }
}
