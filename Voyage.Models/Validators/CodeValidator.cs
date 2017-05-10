using FluentValidation;
using Voyage.Core;

namespace Voyage.Models.Validators
{
    public class CodeValidator : AbstractValidator<CodeModel>
    {
        public CodeValidator()
        {
            RuleFor(_ => _.Code)
                .Length(6)
                .WithErrorCodeMessage(Constants.ErrorCodes.InvalidLength, "Code is invlaid length");
        }
    }
}
