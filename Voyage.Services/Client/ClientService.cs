using System.Linq;
using System.Threading.Tasks;
using Voyage.Data.Repositories.Client;
using Voyage.Models;

namespace Voyage.Services.Client
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<bool> IsValidClientAsync(string clientId, string clientSecret)
        {
            return await Task.Run(() => _clientRepository.ValidateClient(clientId, clientSecret));
        }

        public async Task<ClientModel> GetClientAsync(string clientId)
        {
            return await Task.Run(() =>
            {
                var client = _clientRepository.GetByName(clientId);
                return new ClientModel
                {
                    Id = client.Id,
                    Identifier = client.ClientIdentifier,
                    Secret = client.ClientSecret,
                    RedirectUrl = client.RedirectUri,
                    AllowedScopes = client.ClientScopes.Select(c => c.ClientScopeType.Name).ToList()
                };
            });
        }

        public async Task<bool> IsLockedOutAsync(string clientId)
        {
            return await Task.Run(() => _clientRepository.IsLockedOut(clientId));
        }

        public async Task UpdateFailedLoginAttemptsAsync(string clientId, bool isIncrement)
        {
            await Task.Run(() =>
            {
                _clientRepository.UpdateFailedLoginAttempts(clientId, isIncrement);
            });
        }

        public async Task UnlockClientAsync(string clientId)
        {
            await Task.Run(() =>
            {
                _clientRepository.UnlockClient(clientId);
            });
        }
    }
}
