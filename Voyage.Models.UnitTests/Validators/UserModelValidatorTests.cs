using FluentValidation.TestHelper;
using Voyage.Models.UnitTests.Common;
using Voyage.Models.Validators;
using Xunit;

namespace Voyage.Models.UnitTests.Validators
{
    [Trait("Category", "Model.Validation")]
    public class UserModelValidatorTests : BaseUnitTest
    {
        private readonly UserModelValidator _validator;

        public UserModelValidatorTests()
        {
            _validator = new UserModelValidator();
        }

        [Fact]
        public void Should_Have_Error_When_FirstName_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.FirstName, null as string);
        }

        [Fact]
        public void Should_Have_Error_When_LastName_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.LastName, null as string);
        }

        [Fact]
        public void Should_Have_Error_When_UserName_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.Username, null as string);
        }

        [Fact]
        public void Should_Have_Error_when_UseName_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.Username, null as string);
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.Email, "abc");
        }
    }
}
