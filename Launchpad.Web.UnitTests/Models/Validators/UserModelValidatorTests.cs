using FluentValidation.TestHelper;

using Launchpad.Models.Validators;
using Launchpad.UnitTests.Common;

using Xunit;

namespace Launchpad.UnitTests.Models.Validators
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
        public void Should_Have_Error_When_Email_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.Email, null as string);
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.Email, "abc");
        }
    }
}
