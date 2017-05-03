using System.Collections.Generic;
using FluentValidation.TestHelper;
using Voyage.Models.UnitTests.Common;
using Voyage.Models.Validators;
using Xunit;

namespace Voyage.Models.UnitTests.Validators
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
        public void Should_Have_Error_When_Password_Has_No_Special_Characters()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.Password, "Hellooo");
        }

        [Fact]
        public void Should_Have_Error_When_Password_Has_No_Uppercase_Letter()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.Password, "hellooo");
        }

        [Fact]
        public void Should_Have_Error_When_Password_Has_No_Lowercase_Letter()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.Password, "HELLLO");
        }

        [Fact]
        public void Should_Have_Error_When_Password_Has_No_Digit()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.Password, "Hellooo");
        }

        [Fact]
        public void Should_Not_Have_Error_With_Valid_Password()
        {
            _validator.ShouldNotHaveValidationErrorFor(model => model.Password, "Hello!1");
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
                    ConfirmPassword = "notpassword!!",
                    PhoneNumber = "1234567890"
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
                    ConfirmPassword = "password!!!",
                    PhoneNumber = "1234567890"
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
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.Email, "abc");
        }

        [Fact]
        public void Should_Have_Error_when_PhoneNumber_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.PhoneNumber, null as string);
        }

        [Fact]
        public void Should_Have_Error_when_UseName_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.UserName, null as string);
        }

        [Fact]
        public void Should_Have_Error_When_PhoneNumber_Is_Not_In_Valid_Format()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.PhoneNumber, "123abc4567");
        }

        [Fact]
        public void Should_Not_Have_Error_When_PhoneNumber_Is_In_Valid_Format()
        {
            List<string> phoneNumbers = new List<string>();

            phoneNumbers.Add("3087774825");
            phoneNumbers.Add("(281)388-0388");
            phoneNumbers.Add("(979) 778-0978");
            phoneNumbers.Add("281-342-2452");

            foreach (string phoneNumber in phoneNumbers)
            {
                _validator.ShouldNotHaveValidationErrorFor(model => model.PhoneNumber, phoneNumber);
            }
        }
    }
}
