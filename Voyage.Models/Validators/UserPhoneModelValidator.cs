using System.Collections.Generic;
using FluentValidation;
using Voyage.Core;
using Voyage.Models.Enum;

namespace Voyage.Models.Validators
{
    public class UserPhoneModelValidator : AbstractValidator<UserPhoneModel>
    {
        public UserPhoneModelValidator()
        {
            RuleFor(_ => _.PhoneNumber)
                .NotEmpty()
                .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "Phone number is a required field");

            RuleFor(_ => _.PhoneType)
                .NotEmpty()
                .Must(type => new List<string> { PhoneType.Mobile, PhoneType.Office, PhoneType.Home, PhoneType.Other }.Contains(type))
                .WithErrorCodeMessage(Constants.ErrorCodes.InvalidPhoneNumber, "Invalid phone type. Must be one of 'Mobile', 'Office', 'Home', 'Other'.");
        }
    }
}
