using FluentValidation;
using Launchpad.Models.Extensions;

namespace Launchpad.Models.Validators
{
    public class RoleModelValidator : AbstractValidator<RoleModel>
    {
        public RoleModelValidator()
        {
            RuleFor(_ => _.Name)
                .NotEmpty()
                .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "Name is a required field");
        }
    }
}
