using System.Collections.Generic;

namespace Voyage.Models
{
    public class CurrentUserModel : UserModel
    {
        public IEnumerable<string> Roles { get; set; }

        public string ProfileImage { get; set; }
    }
}
