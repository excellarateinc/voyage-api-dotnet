using System.Net.Http;

using Voyage.IntegrationTests.Web.Client;
using Voyage.IntegrationTests.Web.Hosting;

namespace Voyage.IntegrationTests.Web
{
    public abstract class ApiTest : ApiConsumer
    {
        private readonly HostFixture _hostFixture;

        public abstract HttpMethod Method { get; }

        public abstract string PathUnderTest { get; }

        public ApiTest(HostFixture hostFixture)
        {
            _hostFixture = hostFixture;
        }
    }
}
