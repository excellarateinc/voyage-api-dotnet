using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Launchpad.Models.Extensions;

namespace Launchpad.Models.Validators
{
    public class RoleValidator : AbstractValidator<RoleModel>
    {
        public RoleValidator()
        {
            RuleFor(_ => _.Name)
                .NotEmpty()
                .WithErrorCodeMessage("missing.required.field", "Name is required");

        }
    }
}
