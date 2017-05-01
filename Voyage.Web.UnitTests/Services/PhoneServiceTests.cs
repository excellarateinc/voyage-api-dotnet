using Moq;
using Voyage.Data.Repositories.UserPhone;
using Voyage.Models.Entities;
using Voyage.Services.Phone;
using Voyage.Services.User;
using Voyage.UnitTests.Common.AutoMapperFixture;
using Xunit;

namespace Voyage.UnitTests.Services
{
    [Trait("Category", "Phone.Service")]
    [Collection(AutoMapperCollection.CollectionName)]
    public class PhoneServiceTests
    {
        private Mock<IUserService> _userServiceMock;
        private Mock<IUserPhoneRepository> _userPhoneRepositoryMock;
        private PhoneService _phoneService;

        [Fact]
        public void InsertSecurityCode_Should_Set_Collect_values()
        {
            CreateNewMockServices();
            _userPhoneRepositoryMock.Setup(c => c.Get(1234)).Returns(new UserPhone());
            _userPhoneRepositoryMock.Setup(c => c.Update(It.IsAny<UserPhone>()));
            _userPhoneRepositoryMock.Setup(c => c.SaveChanges());
            _phoneService = new PhoneService(_userPhoneRepositoryMock.Object, _userServiceMock.Object);

            _phoneService.InsertSecurityCode(1234, "code");

            _userPhoneRepositoryMock.VerifyAll();
        }

        [Fact]
        public void ResetSecurityCode_Should_Call_All_Require_Parameters()
        {
            CreateNewMockServices();
            _userPhoneRepositoryMock.Setup(c => c.Get(1234)).Returns(new UserPhone());
            _userPhoneRepositoryMock.Setup(c => c.Update(It.IsAny<UserPhone>()));
            _userPhoneRepositoryMock.Setup(c => c.SaveChanges());
            _phoneService = new PhoneService(_userPhoneRepositoryMock.Object, _userServiceMock.Object);

            _phoneService.InsertSecurityCode(1234, "code");

            _userPhoneRepositoryMock.VerifyAll();
        }

        private void CreateNewMockServices()
        {
            _userPhoneRepositoryMock = new Mock<IUserPhoneRepository>();
            _userServiceMock = new Mock<IUserService>();
        }
    }
}
