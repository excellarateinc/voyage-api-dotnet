using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
            return await Task.Run(() =>
            {
                return _clientRepository.ValidateClient(clientId, clientSecret);
            });
        }

        public async Task<ClientModel> GetClientAsync(string clientId)
        {
            return await Task.Run(() =>
            {
                var client = _clientRepository.GetByName(clientId);
                return new ClientModel
                {
                    Id = client.ClientIdentifier,
                    Secret = client.ClientSecret,
                    RedirectUrl = client.RedirectUri,
                    AllowedScopes = client.ClientScopes.Select(c => c.ClientScopeType.Name).ToList()
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
