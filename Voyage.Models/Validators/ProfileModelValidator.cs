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
        }
    }
}
