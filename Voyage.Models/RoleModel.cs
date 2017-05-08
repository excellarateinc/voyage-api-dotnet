using System.Collections.Generic;
using FluentValidation.Attributes;
using Voyage.Models.Validators;

namespace Voyage.Models
{
    [Validator(typeof(RoleModelValidator))]
    public class RoleModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<PermissionModel> Permissions { get; set; }
    }
}
