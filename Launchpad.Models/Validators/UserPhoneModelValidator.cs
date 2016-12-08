using FluentValidation;
using Launchpad.Models.Extensions;

namespace Launchpad.Models.Validators
{
    public class UserPhoneModelValidator : AbstractValidator<UserPhoneModel>
    {
        public UserPhoneModelValidator()
        {
            RuleFor(_ => _.PhoneNumber)
                .NotEmpty()
                .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "Phone number is a required field");

            RuleFor(_ => _.UserId)
                .NotEmpty()
                .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "UserId is a required field");

        }
    }
}
