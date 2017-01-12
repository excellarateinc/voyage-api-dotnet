using System;

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
    public class UserProfileTests : BaseUnitTest
    {
        private readonly AutoMapperFixture _mappingFixture;

        public UserProfileTests(AutoMapperFixture mappingFixture)
        {
            _mappingFixture = mappingFixture;
        }
      
        [Fact]
        public void ApplicationUser_Should_Map_To_UserModel()
        {
            var appUser = new ApplicationUser
            {
                UserName = "user1",
                Id = "123",
                FirstName = "First",
                LastName = "Last",
                IsActive = true
            };

            var user = _mappingFixture.MapperInstance.Map<UserModel>(appUser);

            user.Should().NotBeNull();
            user.Username.Should().Be(appUser.UserName);
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
                Username = "tom@tom.com",
                FirstName = "Third",
                LastName = "Fourth"
            };

            _mappingFixture.MapperInstance.Map(userModel, appUser);

            appUser.IsActive.Should().BeTrue();
            appUser.UserName.Should().Be(userModel.Username);
            appUser.Email.Should().Be(userModel.Email);
            appUser.Id.Should().NotBe(userModel.Id);
            appUser.FirstName.Should().Be(userModel.FirstName);
            appUser.LastName.Should().Be(userModel.LastName);
        }
    }
}
