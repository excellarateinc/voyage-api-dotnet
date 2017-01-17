using FluentValidation.Attributes;
using Launchpad.Models.Validators;

namespace Launchpad.Models
{
    [Validator(typeof(RegistrationModelValidator))]
    public class RegistrationModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
