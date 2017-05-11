using System.Threading.Tasks;
using Voyage.Core;

namespace Voyage.Services.Client
{
    public class ClientService : IClientService
    {
        public async Task<bool> IsValidClientAsync(string clientId, string clientSecret)
        {
            // TODO: Your domain implementation for client validation goes here
            return await Task.Run(() =>
            {
                var isValidClient = false;
                if (clientId == Clients.Client1.Id && clientSecret == Clients.Client1.Secret)
                {
                    isValidClient = true;
                }
                else if (clientId == Clients.Client2.Id && clientSecret == Clients.Client2.Secret)
                {
                    isValidClient = true;
                }

                return isValidClient;
            });
        }

        public async Task<Core.Client> GetClientAsync(string clientId)
        {
            // TODO: Your domain implementation for client validation goes here
            return await Task.Run(() =>
            {
                Core.Client client = null;
                if (clientId == Clients.Client1.Id)
                {
                    client = Clients.Client1;
                }
                else if (clientId == Clients.Client2.Id)
                {
                    client = Clients.Client2;
                }

                return client;
            });
        }
    }
}
