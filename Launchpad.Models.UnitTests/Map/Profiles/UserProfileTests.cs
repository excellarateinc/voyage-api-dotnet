using FluentAssertions;
using Launchpad.Models.EntityFramework;
using Launchpad.Models.UnitTests.Fixture;
using Launchpad.UnitTests.Common;
using System;
using Xunit;

namespace Launchpad.Models.UnitTests.Map.Profiles
{

    [Trait("Cateogry", "Mapping")]
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
            appUser.FirstName = "First";
            appUser.LastName = "Last";
            appUser.IsActive = true;

            var user = _mappingFixture.MapperInstance.Map<UserModel>(appUser);

            user.Should().NotBeNull();
            user.Name.Should().Be(appUser.UserName);
            user.Id.Should().Be(appUser.Id);
            user.FirstName.Should().Be(appUser.FirstName);
            user.LastName.Should().Be(appUser.LastName);
            user.IsActive.Should().Be(appUser.IsActive);
        }

        [Fact]
        public void UserModel_Should_Map_To_ApplicationUser()
        {
            var appUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "hank@hank.com",
                Email = "hank@hank.com",
                FirstName = "First",
                LastName = "Second",
                IsActive = false
            };

            var userModel = new UserModel
            {
                IsActive = true,
                Id = Guid.NewGuid().ToString(),
                Name = "tom@tom.com",
                FirstName = "Third",
                LastName = "Fourth"
            };

            var mapResult = _mappingFixture.MapperInstance.Map(userModel, appUser);

            appUser.IsActive.Should().BeFalse();
            appUser.UserName.Should().Be(userModel.Name);
            appUser.Email.Should().Be(userModel.Name);
            appUser.Id.Should().NotBe(userModel.Id);
            appUser.FirstName.Should().Be(userModel.FirstName);
            appUser.LastName.Should().Be(userModel.LastName);
        }

    }
}
