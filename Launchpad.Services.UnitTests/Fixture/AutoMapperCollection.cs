using Xunit;

namespace Launchpad.Services.Fixture
{
    [CollectionDefinition(AutoMapperCollection.CollectionName)]
    public class AutoMapperCollection : ICollectionFixture<AutoMapperFixture>
    {
        public const string CollectionName = "AutoMapper Collection";
    }
}
