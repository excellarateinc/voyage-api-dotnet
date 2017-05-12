using Embarr.WebAPI.AntiXss;
using FluentValidation.Attributes;
using System.Collections.Generic;
using Voyage.Models.Validators;

namespace Voyage.Models
{
    [Validator(typeof(PhoneSecurityCodeValidator))]
    public class PhoneSecurityCode
    {
        [AntiXss]
        public string Code { get; set; }
    }
}
