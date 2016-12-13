using Launchpad.Web.IntegrationTests.Hosting;
using System;
using System.Net.Http;

namespace Launchpad.Web.IntegrationTests.Client
{
    public abstract class ApiConsumer : IDisposable
    {
        protected readonly HttpClient Client;

        public ApiConsumer()
        {
            Client = new HttpClient();
        }

        public string GetUrl(string path)
        {
            return $"{HostingOptions.BaseAddress}{path}";
        }

        protected HttpRequestMessage CreateSecureRequest(HttpMethod method, string path)
        {
            var message = new HttpRequestMessage(method, GetUrl(path));
            message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", TokenProvider.Instance.Token);
            return message;
        }

        protected HttpRequestMessage CreateUnauthorizedRequest(HttpMethod method, string path)
        {
            var message = new HttpRequestMessage(method, GetUrl(path));
            message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Guid.NewGuid().ToString());
            return message;
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
