using Xunit;

namespace Launchpad.Services.UnitTests.Fixture
{
    [CollectionDefinition(CollectionName)]
    public class AutoMapperCollection : ICollectionFixture<AutoMapperFixture>
    {
        public const string CollectionName = "AutoMapper Collection";
    }
}
