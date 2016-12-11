using FluentValidation.Attributes;
using Launchpad.Models.Validators;

namespace Launchpad.Models
{
    [Validator(typeof(ClaimModelValidator))]
    public class ClaimModel
    {
        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }

        public int Id { get; set; }       
    }
}
