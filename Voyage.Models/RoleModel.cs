using System.Collections.Generic;
using FluentValidation.Attributes;
using Newtonsoft.Json;
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

        [AntiXss]
        public string Description { get; set; }

        [JsonProperty("Permissions")]
        public IEnumerable<ClaimModel> Claims { get; set; }
    }
}
