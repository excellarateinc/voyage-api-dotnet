using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, GetUrl("/api/v1/login"))
            {
                Content =
                    new StringContent(
                    "grant_type=password&username=admin%40admin.com&password=Hello123!",
                    Encoding.UTF8,
                    "application/x-www-form-urlencoded")
            };

            var response = await Client.SendAsync(httpRequestMessage);
            var rawResponse = response.Content.ReadAsStringAsync().Result;
            dynamic tokenResponse = JsonConvert.DeserializeObject(rawResponse);
            Token = tokenResponse.access_token;
        }
    }
}
