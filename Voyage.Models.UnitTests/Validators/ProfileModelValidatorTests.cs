using System.Collections.Generic;
using FluentValidation.TestHelper;
using Voyage.Models.UnitTests.Common;
using Voyage.Models.Validators;
using Xunit;

namespace Voyage.Models.UnitTests.Validators
{
    [Trait("Category", "Model.Validation")]
    public class ProfileModelValidatorTests : BaseUnitTest
    {
        private readonly ProfileModelValidator _validator;

        public ProfileModelValidatorTests()
        {
            _validator = new ProfileModelValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Changing_Password_And_CurrentPassword_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(
                model => model.CurrentPassword,
                new ProfileModel
                {
                    NewPassword = "password!!!"
                });
        }

        [Fact]
        public void Should_Not_Have_Error_When_Not_Changing_Password_And_CurrentPassword_Is_Null()
        {
            _validator.ShouldNotHaveValidationErrorFor(
                model => model.CurrentPassword,
                new ProfileModel
                {
                    NewPassword = null
                });
        }

        [Fact]
        public void Should_Have_Error_When_Changing_Password_And_Password_Is_Too_Short()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.NewPassword, "abc");
        }

        [Fact]
        public void Should_Have_Error_When_Changing_Password_And_Password_Is_Too_Long()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.NewPassword, "01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789");
        }

        [Fact]
        public void Should_Have_Error_When_Changing_Password_And_Password_Has_No_Special_Characters()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.NewPassword, "Hellooo");
        }

        [Fact]
        public void Should_Have_Error_When_Changing_Password_And_Password_Has_No_Uppercase_Letter()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.NewPassword, "hellooo");
        }

        [Fact]
        public void Should_Have_Error_When_Changing_Password_And_Password_Has_No_Lowercase_Letter()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.NewPassword, "HELLLO");
        }

        [Fact]
        public void Should_Have_Error_When_Changing_Password_And_Password_Has_No_Digit()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.NewPassword, "Hellooo");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Changing_Password_With_Valid_Password()
        {
            _validator.ShouldNotHaveValidationErrorFor(model => model.NewPassword, "Hello!1");
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
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.Email, "abc");
        }

        [Fact]
        public void Should_Have_Error_when_PhoneNumber_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.Phones, new List<UserPhoneModel>());
        }
    }
}
