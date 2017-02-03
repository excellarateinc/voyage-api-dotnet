using Xunit;

namespace Voyage.IntegrationTests.Web.Hosting
{
    [CollectionDefinition(Name)]
    public class HostCollectionFixture : ICollectionFixture<HostFixture>
    {
        public const string Name = "HostCollectionFixture";
    }
}
