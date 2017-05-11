using System.Threading.Tasks;

namespace Voyage.Services.Client
{
    public interface IClientService
    {
        Task<bool> IsValidClientAsync(string clientId, string clientSecret);

        Task<Core.Client> GetClientAsync(string clientId);
    }
}
