using Launchpad.Models.Enum;

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
