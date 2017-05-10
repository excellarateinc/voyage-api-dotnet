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
                    PhoneNumber = new List<UserPhoneModel>
                                    {
                                        new UserPhoneModel
                                        {
                                            PhoneNumber=string.Empty
                                        }
                                    }
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
                    PhoneNumber = new List<UserPhoneModel>
                                    {
                                        new UserPhoneModel
                                        {
                                            PhoneNumber=string.Empty
                                        }
                                    }
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
            _validator.ShouldHaveValidationErrorFor(model => model.PhoneNumbers, new List<UserPhoneModel>());
        }

        [Fact]
        public void Should_Have_Error_when_UseName_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.UserName, null as string);
        }
    }
}
