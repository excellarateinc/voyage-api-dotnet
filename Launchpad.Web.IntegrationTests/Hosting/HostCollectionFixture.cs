using Xunit;

namespace Launchpad.Web.IntegrationTests.Hosting
{
    [CollectionDefinition(Name)]
    public class HostCollectionFixture : ICollectionFixture<HostFixture>
    {
        public const string Name = "HostCollectionFixture";
    }
}
