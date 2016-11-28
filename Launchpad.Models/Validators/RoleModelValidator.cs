using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
