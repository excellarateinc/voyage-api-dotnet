using Launchpad.Web.IntegrationTests.Client;
using Launchpad.Web.IntegrationTests.Hosting;
using System.Net.Http;

namespace Launchpad.Web.IntegrationTests
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
