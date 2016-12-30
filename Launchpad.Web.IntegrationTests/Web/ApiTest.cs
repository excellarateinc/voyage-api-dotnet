using System.Net.Http;

using Launchpad.IntegrationTests.Web.Client;
using Launchpad.IntegrationTests.Web.Hosting;

namespace Launchpad.IntegrationTests.Web
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
