using Embarr.WebAPI.AntiXss;
using FluentValidation.Attributes;
using System.Collections.Generic;
using Voyage.Models.Validators;

namespace Voyage.Models
{
    [Validator(typeof(CodeValidator))]
    public class CodeModel
    {
        [AntiXss]
        public string Code { get; set; }
    }
}
