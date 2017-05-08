using FluentValidation.Attributes;
using Voyage.Models.Validators;

namespace Voyage.Models
{
    [Validator(typeof(PermissionModelValidator))]
    public class PermissionModel
    {
        public string PermissionType { get; set; }

        public string PermissionValue { get; set; }

        public int Id { get; set; }
    }
}
