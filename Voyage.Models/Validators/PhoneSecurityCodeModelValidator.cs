using FluentValidation;
using Voyage.Core;

namespace Voyage.Models.Validators
{
    public class PhoneSecurityCodeModelValidator : AbstractValidator<PhoneSecurityCodeModel>
    {
        public PhoneSecurityCodeModelValidator()
        {
            RuleFor(_ => _.Code)
                .Length(6)
                .WithErrorCodeMessage(Constants.ErrorCodes.InvalidLength, "Code is invlaid length");
        }
    }
}
