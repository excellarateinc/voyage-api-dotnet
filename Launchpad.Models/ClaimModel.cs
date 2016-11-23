using FluentValidation.Attributes;
using Launchpad.Models.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
