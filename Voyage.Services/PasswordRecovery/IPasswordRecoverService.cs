using System.Collections.Generic;
using System.Threading.Tasks;
using Voyage.Models;

namespace Voyage.Services.PasswordRecovery
{
    public interface IPasswordRecoverService
    {
        Task<ForgotPasswordModel> ValidateUserInfoAsync(string userName, string phoneNumber);

        Task<ForgotPasswordModel> VerifyCodeAsync(string userId, string code);

        ForgotPasswordModel VerifySecurityAnswers(string userId, List<string> anwers);

        Task<ForgotPasswordModel> ResetPasswordAsync(string userName, string verificationCode, string newPassword, string confirmNewPassword);
    }
}
