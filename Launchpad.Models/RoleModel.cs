using FluentValidation.Attributes;
using Launchpad.Models.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launchpad.Models
{
    [Validator(typeof(RoleValidator))]
    public class RoleModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<ClaimModel> Claims { get; set; }
    }
}
