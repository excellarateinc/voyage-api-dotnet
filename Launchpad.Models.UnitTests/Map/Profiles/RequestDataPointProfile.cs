using Launchpad.Models.UnitTests.Fixture;
using Launchpad.UnitTests.Common;
using Xunit;
using Ploeh.AutoFixture;
using FluentAssertions;
using Launchpad.Models.Enum;

namespace Launchpad.Models.UnitTests.Map.Profiles
{
    [Collection(AutoMapperCollection.CollectionName)]
    public class RequestDataPointProfile : BaseUnitTest
    {
        private AutoMapperFixture _mappingFixture;

        public RequestDataPointProfile(AutoMapperFixture mappingFixture)
        {
            _mappingFixture = mappingFixture;
        }

        [Fact]
        public void RequestDataPointModel_Should_Map_To_Status()
        {
            var dataPoint = Fixture.Create<RequestDataPointModel>();

            var status = _mappingFixture.MapperInstance.Map<StatusModel>(dataPoint);

            status.Should().NotBeNull();
            status.Code.Should().Be(StatusCode.OK);
            status.Message.Should().Be(dataPoint.ToString());
        }

    }
}
