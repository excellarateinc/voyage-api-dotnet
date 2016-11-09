using FluentAssertions;
using Launchpad.Models.EntityFramework;
using Launchpad.Models.UnitTests.Fixture;
using Launchpad.UnitTests.Common;
using System;
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
        public void ApplicationUser_Should_Map_To_UserWithRolesModel()
        {
            var appUser = new ApplicationUser();
            appUser.UserName = "user1";
            appUser.Id = "123";
           

            var user = _mappingFixture.MapperInstance.Map<UserWithRolesModel>(appUser);

            user.Should().NotBeNull();
            user.Name.Should().Be(appUser.UserName);
            user.Id.Should().Be(appUser.Id);
            user.Roles.Should().BeNull();
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

        [Fact]
        public void UserModle_Should_Map_To_ApplicationUser()
        {
            var appUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "hank@hank.com",
                Email = "hank@hank.com"
            };

            var userModel = new UserModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "tom@tom.com"
            };

            var mapResult = _mappingFixture.MapperInstance.Map(userModel, appUser);

            appUser.UserName.Should().Be(userModel.Name);
            appUser.Email.Should().Be(userModel.Name);
            appUser.Id.Should().NotBe(userModel.Id);
        }

    }
}
