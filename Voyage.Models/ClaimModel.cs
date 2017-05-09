using FluentValidation.Attributes;
using Newtonsoft.Json;
using Voyage.Models.Validators;

namespace Voyage.Models
{
    [Validator(typeof(ClaimModelValidator))]
    public class ClaimModel
    {
        [JsonProperty("PermissionType")]
        public string ClaimType { get; set; }

        [JsonProperty("PermissionValue")]
        public string ClaimValue { get; set; }

        public int Id { get; set; }
    }
}
