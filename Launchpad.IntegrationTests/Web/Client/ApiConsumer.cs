using System;
using System.Net.Http;

using Launchpad.IntegrationTests.Web.Hosting;

namespace Launchpad.IntegrationTests.Web.Client
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

        protected HttpRequestMessage CreateSecureRequest(HttpMethod method, string path, params object[] pathArgs)
        {
            var message = new HttpRequestMessage(method, GetUrl(string.Format(path, pathArgs)));
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
