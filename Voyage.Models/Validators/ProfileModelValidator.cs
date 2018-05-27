﻿using FluentValidation;
using Voyage.Core;

namespace Voyage.Models.Validators
{
    public class ProfileModelValidator : AbstractValidator<ProfileModel>
    {
        public ProfileModelValidator()
        {
            RuleFor(_ => _.Email)
              .EmailAddress()
              .WithErrorCodeMessage(Constants.ErrorCodes.InvalidEmail, "Email is invalid");

            RuleFor(_ => _.FirstName)
               .NotEmpty()
               .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "First name is a required field");

            RuleFor(_ => _.LastName)
                .NotEmpty()
                .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "Last name is a required field");

            RuleFor(_ => _.Phones)
                .NotEmpty()
                .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "Phone Number is a required field");

            RuleFor(_ => _.CurrentPassword)
                .NotEmpty()
                .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "Current Password is a required field")
                .When(_ => !string.IsNullOrEmpty(_.NewPassword));

            RuleFor(_ => _.NewPassword)
                .Length(6, 100)
                .WithErrorCodeMessage(Constants.ErrorCodes.InvalidLength, "New Password is an invalid length")
                .Matches(@"(?=.*[!@#$&*])")
                .WithErrorCodeMessage(Constants.ErrorCodes.InvalidPassword, "New Password must have once special character")
                .Matches(@"(?=.*[A-Z])")
                .WithErrorCodeMessage(Constants.ErrorCodes.InvalidPassword, "New Password must have one upper case letter")
                .Matches(@"(?=.*[a-z])")
                .WithErrorCodeMessage(Constants.ErrorCodes.InvalidPassword, "New Password must have one lower case letter")
                .Matches(@"(?=.*[0-9])")
                .WithErrorCodeMessage(Constants.ErrorCodes.InvalidPassword, "New Password must have one digit")
                .When(_ => !string.IsNullOrEmpty(_.NewPassword));

            RuleFor(_ => _.ConfirmNewPassword)
                .NotEmpty()
                .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "Confirm New Password is a required field")
                .Must((model, value) => !string.IsNullOrEmpty(model.NewPassword) && model.NewPassword.Equals(value))
                .WithErrorCodeMessage(Constants.ErrorCodes.InvalidDependentRule, "Confirm New Password must match New Password")
                .When(_ => !string.IsNullOrEmpty(_.NewPassword));
        }
    }
}
