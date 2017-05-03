using System.Threading.Tasks;

namespace Voyage.Data.Repositories.UserPhone
{
    public interface IUserPhoneRepository : IRepository<Models.Entities.UserPhone>
    {
        bool IsValidPhoneNumber(string phoneNumber, out string formatedPhoneNumber);

        string GetE164Format(string phoneNumber);

        Task SendSecurityCode(string phoneNumber, string securityCode);
    }
}
