using Launchpad.Web.IntegrationTests.Client;
using Launchpad.Web.IntegrationTests.Hosting;

namespace Launchpad.Web.IntegrationTests
{
    public class ApiTest : ApiConsumer
    {
        private readonly HostFixture _hostFixture;

        public ApiTest(HostFixture hostFixture)
        {
            _hostFixture = hostFixture;
        }
    }
}
