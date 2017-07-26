using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voyage.Models
{
    public class ClientModel
    {
        public string Id { get; set; }

        public string Identifier { get; set; }

        public string Secret { get; set; }

        public string RedirectUrl { get; set; }

        public List<string> AllowedScopes { get; set; }
    }
}
