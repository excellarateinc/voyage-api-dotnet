using FluentValidation;
using Voyage.Core;

namespace Voyage.Models.Validators
{
    public class TransferModelValidator : AbstractValidator<TransferModel>
    {
        public TransferModelValidator()
        {
            RuleFor(_ => _.FromAccountId)
                .GreaterThan(0)
                .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "From Account is a required field");

            RuleFor(_ => _.ToAccountId)
                .GreaterThan(0)
                .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "To Account is a required field");

            RuleFor(_ => _.Amount)
                .GreaterThan(0)
                .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "Amount is a required field");
        }
    }
}
