using Embarr.WebAPI.AntiXss;
using FluentValidation.Attributes;
using Voyage.Models.Validators;

namespace Voyage.Models
{
    [Validator(typeof(ClaimModelValidator))]
    public class ClaimModel
    {
        [AntiXss]
        public string ClaimType { get; set; }

        [AntiXss]
        public string ClaimValue { get; set; }

        public int Id { get; set; }
    }
}
