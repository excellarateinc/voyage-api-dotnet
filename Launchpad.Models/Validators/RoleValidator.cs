using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launchpad.Models.Validators
{
    public class RoleValidator : AbstractValidator<RoleModel>
    {
        public RoleValidator()
        {
            RuleFor(_ => _.Name)
                .NotEmpty()
                .WithMessage("Name is a required field")
                .WithErrorCode("abc");

        }
    }
}
