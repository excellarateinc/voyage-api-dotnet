using FluentValidation;
using Launchpad.Models.Extensions;

namespace Launchpad.Models.Validators
{
    public class WidgetModelValidator : AbstractValidator<WidgetModel>
    {
        public WidgetModelValidator()
        {
            RuleFor(_=>_.Name)
                .NotEmpty()
                .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "Name is a required field");
        }
    }
}
