using System.Collections.Generic;
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
            return await _clientRepository.ValidateClientAsync(clientId, clientSecret);
        }

        public async Task<ClientModel> GetClientAsync(string clientId)
        {
            var client = await _clientRepository.GetByNameAsync(clientId);
            return new ClientModel
            {
                Id = client.Id,
                Identifier = client.ClientIdentifier,
                Secret = client.ClientSecret,
                RedirectUrl = client.RedirectUri,
                AllowedScopes = client.ClientScopes.Select(c => c.ClientScopeType.Name).ToList()
            };
        }

        public async Task<bool> IsLockedOutAsync(string clientId)
        {
            return await _clientRepository.IsLockedOutAsync(clientId);
        }

        public async Task UpdateFailedLoginAttemptsAsync(string clientId, bool isIncrement)
        {
            await _clientRepository.UpdateFailedLoginAttemptsAsync(clientId, isIncrement);
        }

        public async Task UnlockClientAsync(string clientId)
        {
            await _clientRepository.UnlockClientAsync(clientId);
        }

        public async Task<IList<string>> GetScopeListByClientId(string clientId)
        {
            var client = await GetClientAsync(clientId);
            return client.AllowedScopes;
        }
    }
}
