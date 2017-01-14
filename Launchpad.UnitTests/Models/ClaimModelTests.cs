using FluentAssertions;

using FluentValidation.Attributes;

using Launchpad.Models;
using Launchpad.Models.Validators;

using Xunit;

namespace Launchpad.UnitTests.Models
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
