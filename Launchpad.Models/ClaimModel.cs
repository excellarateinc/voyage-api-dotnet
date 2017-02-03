using FluentValidation.Attributes;
using Voyage.Models.Validators;

namespace Voyage.Models
{
    [Validator(typeof(ClaimModelValidator))]
    public class ClaimModel
    {
        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }

        public int Id { get; set; }
    }
}
