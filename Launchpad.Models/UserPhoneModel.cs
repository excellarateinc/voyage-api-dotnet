using FluentValidation.Attributes;
using Launchpad.Models.Enum;
using Launchpad.Models.Validators;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Launchpad.Models
{
    [Validator(typeof(UserPhoneModelValidator))]
    public class UserPhoneModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string PhoneNumber { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PhoneType PhoneType { get; set; }
    }
}
