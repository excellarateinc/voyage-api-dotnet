using Launchpad.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launchpad.Models
{
    public class PhoneContactModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string PhoneNumber { get; set; }
        public PhoneType PhoneType { get; set; }
    }
}
