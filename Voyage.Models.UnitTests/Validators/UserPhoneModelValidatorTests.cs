using FluentValidation.TestHelper;
using Voyage.Models.Validators;
using Xunit;

namespace Voyage.Models.UnitTests.Validators
{
    [Trait("Category", "Model.Validation")]
    public class UserPhoneModelValidatorTests
    {
        private readonly UserPhoneModelValidator _validator;

        public UserPhoneModelValidatorTests()
        {
            _validator = new UserPhoneModelValidator();
        }

        [Fact]
        public void Should_Have_Error_When_PhoneNumber_Null()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.PhoneNumber, null as string);
        }

        [Fact]
        public void Should_Have_Error_When_PhoneNumber_Empty()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.PhoneNumber, string.Empty);
        }
    }
}
