using System.Collections.Generic;
using FluentValidation.Attributes;
using Newtonsoft.Json;
using Voyage.Models.Validators;

namespace Voyage.Models
{
    [Validator(typeof(RoleModelValidator))]
    public class RoleModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [JsonProperty("Permissions")]
        public IEnumerable<ClaimModel> Claims { get; set; }
    }
}
