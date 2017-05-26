using System.Threading.Tasks;

namespace Voyage.Services.Phone
{
    public interface IPhoneService
    {
        string GenerateSecurityCode();

        void InsertSecurityCode(int phoneId, string code);

        bool IsValidPhoneNumber(string phoneNumber, out string formatedPhoneNumber);

        string GetE164Format(string phoneNumber);

        Task InsertSecurityCodeAsync(int phoneId, string code);

        Task SendSecurityCodeAsync(string phoneNumber, string securityCode);

        Task SendSecurityCodeToUserPhoneNumber(string userName);

        Task<bool> IsValidSecurityCodeAsync(string userId, string securityCode);

        Task ClearUserPhoneSecurityCodeAsync(string userId);
    }
}
