using System.Threading.Tasks;
using Amazon.SimpleNotificationService.Model;

namespace Voyage.Services.Phone
{
    public interface IPhoneService
    {
        string GenerateSecurityCode();

        void InsertSecurityCode(int phoneId, string code);

        bool IsValidPhoneNumber(string phoneNumber, out string formatedPhoneNumber);

        string GetE164Format(string phoneNumber);

        Task SendSecurityCode(string phoneNumber, string securityCode);

        Task SendSecurityCodeToUserPhoneNumber(string userName);

        Task<bool> IsValidSecurityCode(string userId, string securityCode);

        Task ClearUserPhoneSecurityCode(string userId);
    }
}
