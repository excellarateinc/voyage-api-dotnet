using Xunit;

namespace Voyage.Api.UnitTests.Common.AutoMapperFixture
{
    [CollectionDefinition(CollectionName)]
    public class AutoMapperCollection : ICollectionFixture<Api.UnitTests.Common.AutoMapperFixture.AutoMapperFixture>
    {
        public const string CollectionName = "AutoMapper Collection";
    }
}
