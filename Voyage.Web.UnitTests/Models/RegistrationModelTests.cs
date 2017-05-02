using FluentAssertions;
using FluentValidation.Attributes;
using Voyage.Models;
using Voyage.Models.Validators;
using Xunit;

namespace Voyage.Web.UnitTests.Models
{
    [Trait("Category", "Model.Validation")]
    public class RegistrationModelTests
    {
        [Fact]
        public void Class_Should_Have_Validator_Class()
        {
            typeof(RegistrationModel)
                .Should()
                .BeDecoratedWith<ValidatorAttribute>(_ => _.ValidatorType == typeof(RegistrationModelValidator));
        }
    }
}
