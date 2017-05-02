using System.Collections.Generic;
using System.Threading.Tasks;
using Voyage.Models;
using Voyage.Web.Models;

namespace Voyage.Services.PasswordRecovery
{
    public interface IPasswordRecoverService
    {
        Task<ForgotPasswordModel> ValidateUserInfoAsync(string userName, string phoneNumber);

        Task<ForgotPasswordModel> VerifyCodeAsync(UserApplicationSession appUser, string code);

        ForgotPasswordModel VerifySecurityAnswers(string userId, List<string> anwers);

        Task<ForgotPasswordModel> ResetPasswordAsync(UserApplicationSession appUser, string newPassword, string confirmNewPassword);
    }
}
