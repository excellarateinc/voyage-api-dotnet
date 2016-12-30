using FluentAssertions;

using Launchpad.Models;
using Launchpad.Models.Entities;
using Launchpad.UnitTests.Common;
using Launchpad.UnitTests.Common.AutoMapperFixture;

using Xunit;

namespace Launchpad.UnitTests.Models.Map.Profiles
{
    [Trait("Category", "Mapping")]
    [Collection(AutoMapperCollection.CollectionName)]
    public class UserPhonesProfileTest : BaseUnitTest
    {
        private readonly AutoMapperFixture _mappingFixture;

        public UserPhonesProfileTest(AutoMapperFixture mappingFixture)
        {
            _mappingFixture = mappingFixture;
        }

        [Fact]
        public void UserPhoneEntity_Should_MapTo_UserPhoneModel()
        {
            var userPhone = new UserPhone
            {
                Id = 1,
                UserId = string.Empty,
                PhoneNumber = "6125555555",
                PhoneType = Launchpad.Models.Enum.PhoneType.Office
            };

            var model = _mappingFixture.MapperInstance.Map<UserPhoneModel>(userPhone);

            model.Should().NotBeNull();
            model.Id.Should().Be(userPhone.Id);
            model.UserId.Should().Be(userPhone.UserId);
            model.PhoneNumber.Should().Be(userPhone.PhoneNumber);
            model.PhoneType.Should().Be(userPhone.PhoneType);
        }
    }
}
