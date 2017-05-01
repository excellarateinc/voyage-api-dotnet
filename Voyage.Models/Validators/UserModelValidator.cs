using FluentValidation;
using Voyage.Core;

namespace Voyage.Models.Validators
{
    public class UserModelValidator : AbstractValidator<UserModel>
    {
        public UserModelValidator()
        {
            RuleFor(_ => _.FirstName)
                .NotEmpty()
                .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "First name is a required field");

            RuleFor(_ => _.LastName)
                .NotEmpty()
                .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "Last name is a required field");

            RuleFor(_ => _.Username)
                .NotEmpty()
                .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "Username is a required field");

            RuleFor(_ => _.Email)
                .EmailAddress()
                .WithErrorCodeMessage(Constants.ErrorCodes.InvalidEmail, "Email is invalid");

            RuleFor(_ => _.Username)
                .NotEmpty()
                .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "Username is a required field");
        }
    }
}
