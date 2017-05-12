using Embarr.WebAPI.AntiXss;
using FluentValidation.Attributes;
using System.Collections.Generic;
using Voyage.Models.Validators;

namespace Voyage.Models
{
    [Validator(typeof(PhoneSecurityCodeModelValidator))]
    public class PhoneSecurityCodeModel
    {
        [AntiXss]
        public string Code { get; set; }
    }
}
