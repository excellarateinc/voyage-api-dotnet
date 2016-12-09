using FluentAssertions;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Launchpad.Web.IntegrationTests.Fixture
{
    public class OwinFixture : IDisposable
    {
        public string BaseAddress => "http://localhost:9000";

        public string DefaultToken { get; set; }

        Lazy<HttpClient> _lazyClient = new Lazy<HttpClient>(() => new HttpClient());

        public HttpClient Client => _lazyClient.Value;


        /// <summary>
        /// Requests a new authorization token from the server
        /// </summary>
        /// <returns>Authorization Token</returns>
        protected async Task<string> GenerateToken()
        {


            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"{BaseAddress}/api/v1/login");
            httpRequestMessage.Content = new StringContent("grant_type=password&username=admin%40admin.com&password=Hello123!", Encoding.UTF8,
                                "application/x-www-form-urlencoded");

            var response = await Client.SendAsync(httpRequestMessage);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var rawResponse = response.Content.ReadAsStringAsync().Result;
            dynamic tokenResponse = JsonConvert.DeserializeObject(rawResponse);
            string token = tokenResponse.access_token;
            token.Should().NotBeNullOrEmpty();

            return token;
        }


        public OwinFixture()
        {
        }

        public IDisposable Start()
        {

            var webAppInstance = WebApp.Start<Startup>(url: BaseAddress);
            return webAppInstance;
        }

        public async Task Init()
        {
            DefaultToken = await GenerateToken();
        }


        public void Dispose()
        {

            if (Client != null)
            {
                Client.Dispose();
            }

        }
    }
}
