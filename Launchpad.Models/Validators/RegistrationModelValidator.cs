using FluentValidation;

namespace Launchpad.Models.Validators
{
    public class RegistrationModelValidator : AbstractValidator<RegistrationModel>
    {
        public RegistrationModelValidator()
        {
            RuleFor(_ => _.Email)
              .NotEmpty()
              .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "Email is a required field")
              .EmailAddress()
              .WithErrorCodeMessage(Constants.ErrorCodes.InvalidEmail, "Email is invalid");

            RuleFor(_ => _.Password)
                .NotEmpty()
                .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "Password is a required field")
                .Length(6, 100)
                .WithErrorCodeMessage(Constants.ErrorCodes.InvalidLength, "Password is an invalid length");

            RuleFor(_ => _.ConfirmPassword)
                .NotEmpty()
                .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "Confirm password is a required field")
                .Must((model, value) => !string.IsNullOrEmpty(model.Password) &&
                                        model.Password.Equals(value))
                .WithErrorCodeMessage(Constants.ErrorCodes.InvalidDependentRule, "Confirm password must match password");
                         
            RuleFor(_ => _.FirstName)
               .NotEmpty()
               .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "First name is a required field");

            RuleFor(_ => _.LastName)
                .NotEmpty()
                .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "Last name is a required field");
        }
    }
}
