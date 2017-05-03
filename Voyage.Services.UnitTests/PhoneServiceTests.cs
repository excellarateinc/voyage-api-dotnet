using Microsoft.AspNet.Identity;
using Moq;
using Voyage.Data.Repositories.UserPhone;
using Voyage.Models.Entities;
using Voyage.Services.IdentityManagers;
using Voyage.Services.Phone;
using Voyage.Services.UnitTests.Common;
using Voyage.Services.UnitTests.Common.AutoMapperFixture;
using Xunit;

namespace Voyage.Services.UnitTests
{
    [Trait("Category", "Phone.Service")]
    [Collection(AutoMapperCollection.CollectionName)]
    public class PhoneServiceTests : BaseUnitTest
    {
        private readonly AutoMapperFixture _mapperFixture;
        private Mock<IUserPhoneRepository> _userPhoneRepositoryMock;
        private PhoneService _phoneService;
        private Mock<IUserStore<ApplicationUser>> _mockStore;
        private ApplicationUserManager _userManager;

        public PhoneServiceTests(AutoMapperFixture mapperFixture)
        {
            _mockStore = Mock.Create<IUserStore<ApplicationUser>>();
            _mockStore.As<IUserPasswordStore<ApplicationUser>>();
            _mockStore.As<IQueryableUserStore<ApplicationUser>>();
            _mockStore.As<IUserRoleStore<ApplicationUser>>();
            _mockStore.As<IUserClaimStore<ApplicationUser>>();

            _userPhoneRepositoryMock = Mock.Create<IUserPhoneRepository>();
            _userManager = new ApplicationUserManager(_mockStore.Object);
            _mapperFixture = mapperFixture;
        }

        [Fact]
        public void InsertSecurityCode_Should_Set_Collect_values()
        {
            _userPhoneRepositoryMock = new Mock<IUserPhoneRepository>();
            _userPhoneRepositoryMock.Setup(c => c.Get(1234)).Returns(new UserPhone());
            _userPhoneRepositoryMock.Setup(c => c.Update(It.IsAny<UserPhone>()));
            _userPhoneRepositoryMock.Setup(c => c.SaveChanges());
            _phoneService = new PhoneService(_userManager, _mapperFixture.MapperInstance, _userPhoneRepositoryMock.Object);

            _phoneService.InsertSecurityCode(1234, "code");

            _userPhoneRepositoryMock.VerifyAll();
        }

        [Fact]
        public void ResetSecurityCode_Should_Call_All_Require_Parameters()
        {
            _userPhoneRepositoryMock = new Mock<IUserPhoneRepository>();
            _userPhoneRepositoryMock.Setup(c => c.Get(1234)).Returns(new UserPhone());
            _userPhoneRepositoryMock.Setup(c => c.Update(It.IsAny<UserPhone>()));
            _userPhoneRepositoryMock.Setup(c => c.SaveChanges());
            _phoneService = new PhoneService(_userManager, _mapperFixture.MapperInstance, _userPhoneRepositoryMock.Object);

            _phoneService.InsertSecurityCode(1234, "code");

            _userPhoneRepositoryMock.VerifyAll();
        }
    }
}
