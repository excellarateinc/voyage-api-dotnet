using Launchpad.Models.UnitTests.Fixture;
using Xunit;
using Launchpad.UnitTests.Common;

namespace Launchpad.Models.UnitTests
{
    [Collection(AutoMapperCollection.CollectionName)]
    public class AutoMapperValidationTests : BaseUnitTest
    {
        private AutoMapperFixture _mappingFixture;

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
