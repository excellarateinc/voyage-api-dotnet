using FluentValidation;
using Launchpad.Models.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launchpad.Models.Validators
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
