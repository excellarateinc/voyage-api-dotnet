using FluentValidation.TestHelper;

using Voyage.Models;
using Voyage.Models.Validators;
using Voyage.UnitTests.Common;

using Xunit;

namespace Voyage.UnitTests.Models.Validators
{
    [Trait("Category", "Model.Validation")]
    public class RegistrationModelValidatorTests : BaseUnitTest
    {
        private readonly RegistrationModelValidator _validator;

        public RegistrationModelValidatorTests()
        {
            _validator = new RegistrationModelValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Password_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.Password, null as string);
        }

        [Fact]
        public void Should_Have_Error_When_Password_Is_Too_Short()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.Password, "abc");
        }

        [Fact]
        public void Should_Have_Error_When_Password_Is_Too_Long()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.Password, "01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789");
        }

        [Fact]
        public void Should_Have_Error_When_ConfirmPassword_Does_Not_Match_Password()
        {
            _validator.ShouldHaveValidationErrorFor(
                model => model.ConfirmPassword,
                new RegistrationModel
                {
                    FirstName = "first",
                    LastName = "last",
                    Email = "first.last@firstlast.com",
                    Password = "password!!!",
                    ConfirmPassword = "notpassword!!"
                });
        }

        [Fact]
        public void Should_Not_Have_Error_When_ConfirmPassword_Matches_Password()
        {
            _validator.ShouldNotHaveValidationErrorFor(
                model => model.ConfirmPassword,
                new RegistrationModel
                {
                    FirstName = "first",
                    LastName = "last",
                    Email = "first.last@firstlast.com",
                    Password = "password!!!",
                    ConfirmPassword = "password!!!"
                });
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
