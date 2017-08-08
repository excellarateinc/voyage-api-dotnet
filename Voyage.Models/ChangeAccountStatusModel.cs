using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voyage.Models
{
    public class ChangeAccountStatusModel
    {
        public bool? IsActive { get; set; }

        public bool? IsVerifyRequired { get; set; }
    }
}
