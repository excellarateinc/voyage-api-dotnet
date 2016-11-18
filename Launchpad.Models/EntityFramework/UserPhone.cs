using Launchpad.Models.EntityFramework;
using Launchpad.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launchpad.Models.EntityFramework
{
    public class UserPhone
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string PhoneNumber { get; set; }
        public PhoneType PhoneType { get; set; }
    }
}
