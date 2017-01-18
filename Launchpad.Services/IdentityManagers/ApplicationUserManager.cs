using System.Threading.Tasks;
using Launchpad.Models.Entities;
using Microsoft.AspNet.Identity;

namespace Launchpad.Services.IdentityManagers
{
    public class ApplicationUserManager : UserManager<ApplicationUser, string>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser, string> store)
            : base(store)
        {
        }

        public ApplicationUserManager(IUserStore<ApplicationUser, string> store, IUserTokenProvider<ApplicationUser, string> tokenProvider)
            : base(store)
        {
            UserValidator = new UserValidator<ApplicationUser>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            UserTokenProvider = tokenProvider;
        }

        public override async Task<IdentityResult> DeleteAsync(ApplicationUser user)
        {
            user.Deleted = true;
            var result = await UpdateAsync(user);
            return result;
        }
    }
}