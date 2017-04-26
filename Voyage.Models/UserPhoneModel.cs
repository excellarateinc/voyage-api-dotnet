using FluentValidation.Attributes;
using Voyage.Models.Enum;
using Voyage.Models.Validators;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Voyage.Models
{
    [Validator(typeof(UserPhoneModelValidator))]
    public class UserPhoneModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string PhoneNumber { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PhoneType PhoneType { get; set; }

        public string VerificationCode { get; set; }
    }
}
