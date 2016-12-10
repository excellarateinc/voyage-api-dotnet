using Launchpad.Services.UnitTests.Fixture;
using Xunit;

namespace Launchpad.Services.Fixture
{
    [CollectionDefinition(CollectionName)]
    public class AutoMapperCollection : ICollectionFixture<AutoMapperFixture>
    {
        public const string CollectionName = "AutoMapper Collection";
    }
}
