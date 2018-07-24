using System.Threading.Tasks;

namespace Voyage.Services.Phone
{
    public interface IPhoneService
    {
        string GenerateSecurityCode();

        Task InsertSecurityCodeAsync(int phoneId, string code);

        bool IsValidPhoneNumber(string phoneNumber, out string formatedPhoneNumber);

        string GetE164Format(string phoneNumber);

        Task SendSecurityCodeAsync(string phoneNumber, string securityCode);

        Task SendSecurityCodeToUserPhoneNumberAsync(string userName);

        Task<bool> IsValidSecurityCodeAsync(string userId, string securityCode);

        Task ClearUserPhoneSecurityCodeAsync(string userId);
    }
}
