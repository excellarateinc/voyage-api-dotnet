using FluentValidation;
using Voyage.Core;

namespace Voyage.Models.Validators
{
    public class ClaimModelValidator : AbstractValidator<ClaimModel>
    {
        public ClaimModelValidator()
        {
            RuleFor(_ => _.ClaimType)
                .NotEmpty()
                .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "Claim type is a required field");

            RuleFor(_ => _.ClaimValue)
                .NotEmpty()
                .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "Claim value is a required field");
        }
    }
}
