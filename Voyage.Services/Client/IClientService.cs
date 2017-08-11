using System.Collections.Generic;
using System.Threading.Tasks;
using Voyage.Models;

namespace Voyage.Services.Client
{
    public interface IClientService
    {
        Task<bool> IsValidClientAsync(string clientId, string clientSecret);

        Task<ClientModel> GetClientAsync(string clientId);

        Task<bool> IsLockedOutAsync(string clientId);

        Task UpdateFailedLoginAttemptsAsync(string clientId, bool isIncrement = true);

        Task UnlockClientAsync(string clientId);

        IList<string> GetScopeListByClientId(string clientName);
    }
}
