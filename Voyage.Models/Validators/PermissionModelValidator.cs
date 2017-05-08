using FluentValidation;
using Voyage.Core;

namespace Voyage.Models.Validators
{
    public class PermissionModelValidator : AbstractValidator<PermissionModel>
    {
        public PermissionModelValidator()
        {
            RuleFor(_ => _.PermissionType)
                .NotEmpty()
                .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "Permission type is a required field");

            RuleFor(_ => _.PermissionValue)
                .NotEmpty()
                .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "Permission value is a required field");
        }
    }
}
