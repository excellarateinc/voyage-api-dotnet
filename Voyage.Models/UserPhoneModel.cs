using FluentValidation.Attributes;
using Voyage.Models.Validators;
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

        [AntiXss]
        public string PhoneType { get; set; }

        [AntiXss]
        public string VerificationCode { get; set; }
    }
}
