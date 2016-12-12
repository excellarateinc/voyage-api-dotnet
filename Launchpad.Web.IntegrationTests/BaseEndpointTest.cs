using Launchpad.Web.IntegrationTests.Fixture;

namespace Launchpad.Web.IntegrationTests
{
    public abstract class BaseEndpointTest
    {
        protected OwinFixture OwinFixture;

        protected BaseEndpointTest(OwinFixture owin)
        {
            OwinFixture = owin;
        }
    }
}
