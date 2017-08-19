using System.Collections.Generic;
using Embarr.WebAPI.AntiXss;
using FluentValidation.Attributes;
using Voyage.Models.Validators;

namespace Voyage.Models
{
    [Validator(typeof(ProfileModelValidator))]
    public class ProfileModel
    {
        [AntiXss]
        public string Email { get; set; }

        [AntiXss]
        public string FirstName { get; set; }

        [AntiXss]
        public string LastName { get; set; }

        public List<UserPhoneModel> Phones { get; set; }
    }
}
