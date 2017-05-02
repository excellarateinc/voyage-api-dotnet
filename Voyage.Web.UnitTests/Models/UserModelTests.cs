using FluentAssertions;
using FluentValidation.Attributes;
using Voyage.Models;
using Voyage.Models.Validators;
using Xunit;

namespace Voyage.Web.UnitTests.Models
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
