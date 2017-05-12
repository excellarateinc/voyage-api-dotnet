using Embarr.WebAPI.AntiXss;
using FluentValidation.Attributes;
using System.Collections.Generic;
using Voyage.Models.Validators;

namespace Voyage.Models
{
    [Validator(typeof(RegistrationModelValidator))]
    public class RegistrationModel
    {
        [AntiXss]
        public string UserName { get; set; }

        [AntiXss]
        public string Email { get; set; }

        [AntiXss]
        public string Password { get; set; }

        [AntiXss]
        public string ConfirmPassword { get; set; }

        [AntiXss]
        public string FirstName { get; set; }

        [AntiXss]
        public string LastName { get; set; }

        [AntiXss]
        public List<UserPhoneModel> PhoneNumbers { get; set; }
    }
}
