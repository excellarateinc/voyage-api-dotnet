using System;
using System.Configuration;
using System.Threading.Tasks;
using Voyage.Core;
using Voyage.Data.Repositories;
using Voyage.Data.Repositories.Client;
using Voyage.Models;

namespace Voyage.Services.Client
{
    public class ClientService : IClientService
    {
        private IClientRepository _clientRepository;
        private IClientScopeRepository _clientScopeRepository;
        private IClientScopeTypeRepository _clientScopeTypeRepository;

        public ClientService(IClientRepository clientRepository, IClientScopeRepository clientScopeRepository, IClientScopeTypeRepository clientScopeTypeRepository)
        {
            _clientRepository = clientRepository;
            _clientScopeRepository = clientScopeRepository;
            _clientScopeTypeRepository = clientScopeTypeRepository;
        }

        public async Task<bool> IsValidClientAsync(string clientId, string clientSecret)
        {
            // TODO: Your domain implementation for client validation goes here
            return await Task.Run(() =>
            {
                return _clientRepository.ValidateClient(clientId, clientSecret);
            });
        }

        public async Task<ClientModel> GetClientAsync(string clientId)
        {
            // TODO: Your domain implementation for client validation goes here
            return await Task.Run(() =>
            {
                var client = _clientRepository.GetByName(clientId);
                var scope = _clientScopeRepository.GetClientScopesByClientId(client.Id);
                return new ClientModel
                {
                    Id = client.ClientIdentifier,
                    Secret = client.ClientSecret,
                    RedirectUrl = client.RedirectUri,
                    AllowedScopes = _clientScopeTypeRepository.GetScopeNamesByScopes(scope)
                };
            });
        }

        public async Task<bool> IsLockedOutAsync(string clientId)
        {
            return await Task.Run(() =>
            {
                return _clientRepository.IsLockedOut(clientId);
            });
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
