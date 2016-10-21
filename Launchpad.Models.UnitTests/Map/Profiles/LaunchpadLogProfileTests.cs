using Launchpad.Models.UnitTests.Fixture;
using Xunit;
using FluentAssertions;
using Ploeh.AutoFixture;
using Launchpad.UnitTests.Common;
using Launchpad.Models.EntityFramework;
using Launchpad.Models.Enum;

namespace Launchpad.Models.UnitTests.Map.Profiles
{
    [Collection(AutoMapperCollection.CollectionName)]
    public class LaunchpadLogProfileTests : BaseUnitTest
    {
        private AutoMapperFixture _mappingFixture;

        public LaunchpadLogProfileTests(AutoMapperFixture mappingFixture)
        {
            _mappingFixture = mappingFixture;
        }

        [Fact]
        public void LaunchpadLog_Should_Map_To_Status()
        {
            var log = Fixture.Build<LaunchpadLog>()
                .With(_ => _.Level, "Information")
                .Create();

            var status = _mappingFixture.MapperInstance.Map<StatusModel>(log);

            status.Time.Should().Be(log.TimeStamp);
            status.Code.Should().Be(StatusCode.OK);
            status.Message.Should().Be(log.Message);
        }
    }
}
