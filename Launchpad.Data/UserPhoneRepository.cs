using Launchpad.Data.Interfaces;
using Launchpad.Models.EntityFramework;

namespace Launchpad.Data
{
    public class UserPhoneRepository : BaseRepository<UserPhone>, IUserPhoneRepository
    {
        public UserPhoneRepository(ILaunchpadDataContext context) : base(context)
        {
        }
    }
}
