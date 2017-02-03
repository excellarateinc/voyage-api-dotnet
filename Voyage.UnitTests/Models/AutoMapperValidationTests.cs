using Voyage.UnitTests.Common;
using Voyage.UnitTests.Common.AutoMapperFixture;

using Xunit;

namespace Voyage.UnitTests.Models
{
    [Collection(AutoMapperCollection.CollectionName)]
    public class AutoMapperValidationTests : BaseUnitTest
    {
        private readonly AutoMapperFixture _mappingFixture;

        public AutoMapperValidationTests(AutoMapperFixture mappingFixture)
        {
            _mappingFixture = mappingFixture;
        }

        [Fact]
        public void Configuration_Should_Be_Valid()
        {
            _mappingFixture.MapperInstance.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
