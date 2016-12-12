using Launchpad.Web.IntegrationTests.Fixture;
using System.Net.Http;

namespace Launchpad.Web.IntegrationTests
{
    public abstract class BaseEndpointTest
    {
        protected OwinFixture OwinFixture;

        protected string GetEndpoint(string path)
        {
            return $"{OwinFixture.BaseAddress}{path}";
        }

        protected BaseEndpointTest(OwinFixture owin)
        {
            OwinFixture = owin;
        }

        protected HttpRequestMessage CreateSecureRequest(HttpMethod method, string path)
        {
            var message = new HttpRequestMessage(method, GetEndpoint(path));
            message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", OwinFixture.DefaultToken);
            return message;
        }
    }
}
