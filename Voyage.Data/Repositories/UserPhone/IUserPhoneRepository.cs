using System.Threading.Tasks;
using Amazon.SimpleNotificationService.Model;

namespace Voyage.Data.Repositories.UserPhone
{
    public interface IUserPhoneRepository : IRepository<Models.Entities.UserPhone>
    {
        bool IsValidPhoneNumber(string phoneNumber, out string formatedPhoneNumber);

        string GetE164Format(string phoneNumber);

        Task<PublishResponse> SendSecurityCode(string phoneNumber, string securityCode);
    }
}
