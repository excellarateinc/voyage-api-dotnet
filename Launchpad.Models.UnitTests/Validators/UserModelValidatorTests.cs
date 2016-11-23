using Xunit;
using Launchpad.UnitTests.Common;
using Launchpad.Models.Validators;
using FluentValidation.TestHelper;

namespace Launchpad.Models.UnitTests.Validators
{
    [Trait("Category", "Model.Validation")]
    public class UserModelValidatorTests : BaseUnitTest
    {
        private UserModelValidator _validator;

        public UserModelValidatorTests()
        {
            _validator = new UserModelValidator();
        }

        [Fact]
        public void Should_Have_Error_When_FirstName_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(role => role.FirstName, null as string);
        }

        [Fact]
        public void Should_Have_Error_When_LastName_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(role => role.LastName, null as string);
        }

        [Fact]
        public void Should_Have_Error_When_UserName_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(role => role.Username, null as string);
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(role => role.Email, null as string);
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            _validator.ShouldHaveValidationErrorFor(role => role.Email, "abc");
        }
    }
}
