using Xunit;

namespace Launchpad.Web.IntegrationTests.Fixture
{
    [CollectionDefinition(Name)]
    public class OwinCollectionFixture : ICollectionFixture<OwinFixture>
    {
        public const string Name = "OwinCollectionFixture";
    }
}
