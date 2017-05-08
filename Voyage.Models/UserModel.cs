using System.Collections.Generic;
using FluentValidation.Attributes;
using Newtonsoft.Json;
using Voyage.Models.Validators;
using Embarr.WebAPI.AntiXss;

namespace Voyage.Models
{
    [Validator(typeof(UserModelValidator))]
    public class UserModel
    {
        [AntiXss]
        public string Id { get; set; }

        [AntiXss]
        public string FirstName { get; set; }

        [AntiXss]
        public string LastName { get; set; }

        [AntiXss]
        public string Username { get; set; }

        [AntiXss]
        public string Email { get; set; }

        public List<UserPhoneModel> Phones { get; set; }

        public bool IsActive { get; set; }

        public bool IsVerifyRequired { get; set; }

        [AntiXss]
        [JsonIgnore]
        public string PasswordRecoveryToken { get; set; }
    }
}
