using System.Threading.Tasks;

namespace Voyage.Services.Verification
{
    public interface IVerificationService
    {
        Task SendCode(string userId);

        Task VerifyCodeAsync(string userId, string code);
    }
}
