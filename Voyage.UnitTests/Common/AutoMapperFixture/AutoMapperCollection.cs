using Xunit;

namespace Voyage.UnitTests.Common.AutoMapperFixture
{
    [CollectionDefinition(CollectionName)]
    public class AutoMapperCollection : ICollectionFixture<AutoMapperFixture>
    {
        public const string CollectionName = "AutoMapper Collection";
    }
}
