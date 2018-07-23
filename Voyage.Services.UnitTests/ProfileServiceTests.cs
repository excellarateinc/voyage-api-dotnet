using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNet.Identity;
using Moq;
using Voyage.Data.Repositories.ProfileImage;
using Voyage.Data.Repositories.UserPhone;
using Voyage.Models.Entities;
using Voyage.Services.IdentityManagers;
using Voyage.Services.Phone;
using Voyage.Services.Profile;
using Voyage.Services.UnitTests.Common;
using Voyage.Services.UnitTests.Common.AutoMapperFixture;
using Voyage.Services.User;
using Xunit;
using Ploeh.AutoFixture;
using Voyage.Models;
using System.Threading.Tasks;

namespace Voyage.Services.UnitTests
{
    [Trait("Category", "Profile.Service")]
    [Collection(AutoMapperCollection.CollectionName)]
    public class ProfileServiceTests : BaseUnitTest
    {
        private readonly AutoMapperFixture _mapperFixture;
        private ProfileService _profileService;
        private Mock<IUserStore<ApplicationUser>> _mockStore;
        private ApplicationUserManager _userManager;
        private Mock<IUserService> _userServiceMock;
        private Mock<IProfileImageRepository> _profileImageRepositoryMock;

        public ProfileServiceTests(AutoMapperFixture mapperFixture)
        {
            _mockStore = Mock.Create<IUserStore<ApplicationUser>>();
            _mockStore.As<IUserPasswordStore<ApplicationUser>>();
            _mockStore.As<IQueryableUserStore<ApplicationUser>>();
            _mockStore.As<IUserRoleStore<ApplicationUser>>();
            _mockStore.As<IUserClaimStore<ApplicationUser>>();
            _userServiceMock = Mock.Create<IUserService>();
            _profileImageRepositoryMock = Mock.Create<IProfileImageRepository>();

            _userManager = new ApplicationUserManager(_mockStore.Object);
            _mapperFixture = mapperFixture;
            _profileService = new ProfileService(_userServiceMock.Object, _profileImageRepositoryMock.Object);
        }

        [Fact]
        public async void GetCurrentUserAync_Should_Return_User_For_UserId()
        {
            var userId = Fixture.Create<string>();
            _userServiceMock.Setup(_ => _.GetUserAsync(userId)).ReturnsAsync(new UserModel
            {
                Id = userId,
                Email = "test@test.com"
            });
            _userServiceMock.Setup(_ => _.GetUserRolesAsync(userId)).ReturnsAsync(new List<RoleModel>()
            {
                new RoleModel
                {
                    Id = "role1",
                    Name = "role1"
                }
            });
            _profileImageRepositoryMock.Setup(_ => _.GetAll()).Returns(new List<ProfileImage>
            {
                new ProfileImage
                {
                    UserId = userId,
                    ImageData = "ImageData"
                }
            }.AsQueryable());
            var result = await _profileService.GetCurrentUserAync(userId);
            result.Id.Should().Be(userId);
            result.Email.Should().Be("test@test.com");
            result.ProfileImage.Should().Be("ImageData");
            result.Roles.Count().Should().Be(1);
        }

        [Fact]
        public async void UpdateProfileAsync_Should_Update_Users_Profile()
        {
            var userId = Fixture.Create<string>();
            var userModel = Fixture.Create<UserModel>();
            userModel.Id = userId;
            _userServiceMock.Setup(_ => _.GetUserAsync(userId)).ReturnsAsync(userModel);

            _userServiceMock.Setup(_ => _.GetUserRolesAsync(userId)).ReturnsAsync(new List<RoleModel>()
            {
                new RoleModel
                {
                    Id = "role1",
                    Name = "role1"
                }
            });

            _userServiceMock.Setup(_ => _.UpdateUserAsync(userId, userModel)).ReturnsAsync(userModel);

            _userServiceMock.Setup(_ => _.ChangePassword(userId, "Password", "NewPassword"))
                .ReturnsAsync(new IdentityResult());

            var images = new List<ProfileImage>
            {
                new ProfileImage
                {
                    UserId = userId,
                    ImageData = "ImageData"
                }
            };
            _profileImageRepositoryMock.Setup(_ => _.GetAll()).Returns(images.AsQueryable());

            var image = images.First();
            _profileImageRepositoryMock.Setup(_ => _.UpdateAsync(image)).ReturnsAsync(new ProfileImage());

            var result = await _profileService.UpdateProfileAsync(userId, new ProfileModel
            {
                ProfileImage = "TestImage",
                NewPassword = "NewPassword",
                CurrentPassword = "Password",
                Email = "test@test.com",
                FirstName = "test",
                LastName = "tester",
                Phones = new List<UserPhoneModel>()
            });

            result.Id.Should().Be(userId);
        }
    }
}
