using System.Collections.Generic;
using FluentValidation.Attributes;
using Launchpad.Models.Validators;

namespace Launchpad.Models
{
    [Validator(typeof(RoleModelValidator))]
    public class RoleModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<ClaimModel> Claims { get; set; }
    }
}
