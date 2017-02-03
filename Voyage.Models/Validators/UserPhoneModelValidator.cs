using FluentValidation;
using Voyage.Core;

namespace Voyage.Models.Validators
{
    public class UserPhoneModelValidator : AbstractValidator<UserPhoneModel>
    {
        public UserPhoneModelValidator()
        {
            RuleFor(_ => _.PhoneNumber)
                .NotEmpty()
                .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "Phone number is a required field");
        }
    }
}
