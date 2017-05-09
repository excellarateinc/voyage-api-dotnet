using Embarr.WebAPI.AntiXss;
using FluentValidation.Attributes;
using Newtonsoft.Json;
using Voyage.Models.Validators;

namespace Voyage.Models
{
    [Validator(typeof(ClaimModelValidator))]
    public class ClaimModel
    {
        [JsonProperty("PermissionType")]
        [AntiXss]
        public string ClaimType { get; set; }

        [JsonProperty("PermissionValue")]
        [AntiXss]
        public string ClaimValue { get; set; }

        public int Id { get; set; }
    }
}
