using System.Threading.Tasks;

namespace Voyage.Services.Phone
{
    public interface IPhoneService
    {
        string GenerateSecurityCode();

        void InsertSecurityCode(int phoneId, string code);

        void ResetSecurityCode(int phoneId);

        bool IsValidPhoneNumber(string phoneNumber, out string formatedPhoneNumber);

        Task SendSecurityCode(string phoneNumber, string securityCode);
    }
}
