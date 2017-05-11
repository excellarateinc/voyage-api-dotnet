ss﻿using FluentValidation;
using Voyage.Core;

namespace Voyage.Models.Validators
{
    public class RegistrationModelValidator : AbstractValidator<RegistrationModel>
    {
        public RegistrationModelValidator()
        {
            RuleFor(_ => _.UserName)
                .NotEmpty()
                .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "Username is a required field");

            RuleFor(_ => _.Email)
              .EmailAddress()
              .WithErrorCodeMessage(Constants.ErrorCodes.InvalidEmail, "Email is invalid");

            RuleFor(_ => _.Password)
                .NotEmpty()
                .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "Password is a required field")
                .Length(6, 100)
                .WithErrorCodeMessage(Constants.ErrorCodes.InvalidLength, "Password is an invalid length")
                .Matches(@"(?=.*[!@#$&*])")
                .WithErrorCodeMessage(Constants.ErrorCodes.InvalidPassword, "Password must have once special character")
                .Matches(@"(?=.*[A-Z])")
                .WithErrorCodeMessage(Constants.ErrorCodes.InvalidPassword, "Password must have one upper case letter")
                .Matches(@"(?=.*[a-z])")
                .WithErrorCodeMessage(Constants.ErrorCodes.InvalidPassword, "Password must have one lower case letter")
                .Matches(@"(?=.*[0-9])")
                .WithErrorCodeMessage(Constants.ErrorCodes.InvalidPassword, "Password must have one digit");

            RuleFor(_ => _.ConfirmPassword)
                .NotEmpty()
                .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "Confirm password is a required field")
                .Must((model, value) => !string.IsNullOrEmpty(model.Password) && model.Password.Equals(value))
                .WithErrorCodeMessage(Constants.ErrorCodes.InvalidDependentRule, "Confirm password must match password");

            RuleFor(_ => _.FirstName)
               .NotEmpty()
               .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "First name is a required field");

            RuleFor(_ => _.LastName)
                .NotEmpty()
                .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "Last name is a required field");

            RuleFor(_ => _.PhoneNumbers)
                .NotEmpty()
                .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "Phone Number is a required field");
        }
    }
}
