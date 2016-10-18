using Xunit;

namespace Launchpad.Models.UnitTests.Fixture
{
    [CollectionDefinition(AutoMapperCollection.CollectionName)]
    public class AutoMapperCollection : ICollectionFixture<AutoMapperFixture>
    {
        public const string CollectionName = "AutoMapper Collection";
    }
}
