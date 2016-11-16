using Launchpad.Models.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launchpad.Models
{
    public class UserPhoneModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string PhoneNumber { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public PhoneType PhoneType { get; set; }
    }
}
