using System.Collections.Generic;
using FluentValidation.Attributes;
using Launchpad.Models.Validators;

namespace Launchpad.Models
{
    [Validator(typeof(UserModelValidator))]
    public class UserModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<UserPhoneModel> Phones { get; set; }
        public bool IsActive { get; set; }
    }
}
