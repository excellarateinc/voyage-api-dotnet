using FluentAssertions;
using Voyage.Models.Entities;
using Voyage.Models.UnitTests.Common;
using Voyage.Models.UnitTests.Common.AutoMapperFixture;
using Xunit;

namespace Voyage.Models.UnitTests.Map.Profiles
{
    [Trait("Category", "Mapping")]
    [Collection(AutoMapperCollection.CollectionName)]
    public class UserPhoneProfileTest : BaseUnitTest
    {
        private readonly AutoMapperFixture _mappingFixture;

        public UserPhoneProfileTest(AutoMapperFixture mappingFixture)
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
                PhoneType = Enum.PhoneType.Office
            };

            var model = _mappingFixture.MapperInstance.Map<UserPhoneModel>(userPhone);

            model.Should().NotBeNull();
            model.Id.Should().Be(userPhone.Id);
            model.UserId.Should().Be(userPhone.UserId);
            model.PhoneNumber.Should().Be(userPhone.PhoneNumber);
            model.PhoneType.Should().Be(userPhone.PhoneType.ToString());
        }
    }
}
