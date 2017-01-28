using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json;

namespace Launchpad.IntegrationTests.Web.Client
{
    public sealed class TokenProvider : ApiConsumer
    {
        // Threadsafe initialization of the underlying variable
        private static readonly Lazy<TokenProvider> _lazyTokenProvider = new Lazy<TokenProvider>(() => new TokenProvider());

        // Static instance of the token provider
        public static TokenProvider Instance => _lazyTokenProvider.Value;

        public string Token { get; private set; }

        /// <summary>
        /// Requests a new authorization token from the server
        /// </summary>
        /// <returns>Authorization Token</returns>
        public async Task Configure()
        {
             var client = new TokenClient(
                GetUrl("/oauth/connect/token"),
                "test",
                "F621F470-9731-4A25-80EF-67A6F7C5F4B8");

             var response = await client.RequestResourceOwnerPasswordAsync("admin@admin.com", "Hello123!", "api");

             Token = response.AccessToken;
        }
    }
}
