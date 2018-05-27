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
        public void InsertSecurityCode_Should_Set_Collect_values()
        {
        }

        [Fact]
        public void ResetSecurityCode_Should_Call_All_Require_Parameters()
        {
        }
    }
}
