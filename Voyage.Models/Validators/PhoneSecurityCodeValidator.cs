using FluentValidation;
using Voyage.Core;

namespace Voyage.Models.Validators
{
    public class PhoneSecurityCodeValidator : AbstractValidator<PhoneSecurityCode>
    {
        public PhoneSecurityCodeValidator()
        {
            RuleFor(_ => _.Code)
                .Length(6)
                .WithErrorCodeMessage(Constants.ErrorCodes.InvalidLength, "Code is invlaid length");
        }
    }
}
