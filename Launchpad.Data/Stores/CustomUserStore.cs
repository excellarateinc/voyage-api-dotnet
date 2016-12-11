using System.Data.Entity;
using System.Threading.Tasks;
using Launchpad.Models.EntityFramework;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Launchpad.Data.Stores
{
    public class CustomUserStore : UserStore<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public CustomUserStore(DbContext context) : base(context)
        {
        }

        public override Task<ApplicationUser> FindByEmailAsync(string email)
        {

            return GetUserAggregateAsync(u => u.Email.ToUpper() == email.ToUpper() && !u.Deleted);
        }

        public override Task<ApplicationUser> FindByIdAsync(string userId)
        {

            return GetUserAggregateAsync(u => u.Id.Equals(userId) && !u.Deleted);
        }

        public override async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            var user = await GetUserAggregateAsync(u => u.UserName.ToUpper() == userName.ToUpper() && !u.Deleted);
            return user;
        }
    }
}
