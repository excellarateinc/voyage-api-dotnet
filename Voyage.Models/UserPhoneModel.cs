using FluentValidation.Attributes;
using Voyage.Models.Enum;
using Voyage.Models.Validators;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Embarr.WebAPI.AntiXss;

namespace Voyage.Models
{
    [Validator(typeof(UserPhoneModelValidator))]
    public class UserPhoneModel
    {
        public int Id { get; set; }

        [AntiXss]
        public string UserId { get; set; }

        [AntiXss]
        public string PhoneNumber { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PhoneType PhoneType { get; set; }

        [AntiXss]
        public string VerificationCode { get; set; }
    }
}
