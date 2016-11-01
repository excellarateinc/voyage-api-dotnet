using FluentAssertions;
using Launchpad.Models.EntityFramework;
using Launchpad.Models.UnitTests.Fixture;
using Launchpad.UnitTests.Common;
using Xunit;

namespace Launchpad.Models.UnitTests.Map.Profiles
{


    [Collection(AutoMapperCollection.CollectionName)]
    public class UserProfileTests : BaseUnitTest
    {
        private AutoMapperFixture _mappingFixture;

        public UserProfileTests(AutoMapperFixture mappingFixture)
        {
            _mappingFixture = mappingFixture;
        }

        [Fact]
        public void ApplicationUser_Should_Map_To_UserModel()
        {
            var appUser = new ApplicationUser();
            appUser.UserName = "user1";
            appUser.Id = "123";

            var user = _mappingFixture.MapperInstance.Map<UserModel>(appUser);

            user.Should().NotBeNull();
            user.Name.Should().Be(appUser.UserName);
            user.Id.Should().Be(appUser.Id);
        }

    }
}
