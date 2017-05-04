using System.Collections.Generic;
using FluentValidation.Attributes;
using Voyage.Models.Validators;
using Embarr.WebAPI.AntiXss;

namespace Voyage.Models
{
    [Validator(typeof(RoleModelValidator))]
    public class RoleModel
    {
        [AntiXss]
        public string Id { get; set; }

        [AntiXss]
        public string Name { get; set; }

        public IEnumerable<ClaimModel> Claims { get; set; }
    }
}
