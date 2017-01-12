using FluentAssertions;

using FluentValidation.Attributes;

using Launchpad.Models;
using Launchpad.Models.Validators;

using Xunit;

namespace Launchpad.UnitTests.Models
{
    [Trait("Category", "Model.Validation")]
    public class UserModelTests
    {
        [Fact]
        public void Class_Should_Have_Validator_Class()
        {
            typeof(UserModel)
                .Should()
                .BeDecoratedWith<ValidatorAttribute>(_ => _.ValidatorType == typeof(UserModelValidator));
        }
    }
}
