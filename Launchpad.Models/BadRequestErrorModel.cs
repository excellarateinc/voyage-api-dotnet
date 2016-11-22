using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launchpad.Models
{
    public class BadRequestErrorModel
    {
        public string Code { get; set; }
        public string Field { get; set; }
        public string Description { get; set; }
    }
}
