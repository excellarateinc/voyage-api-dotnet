using System;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using FluentAssertions;
using Xunit;
using Moq;
using Launchpad.UnitTests.Common;
using Launchpad.Models.EntityFramework;
using Launchpad.Services.IdentityManagers;
using Microsoft.AspNet.Identity;
using Launchpad.Models;
using System.Text;

namespace Launchpad.Services.UnitTests
{
    public class UserServiceTests : BaseUnitTest
    {
        private UserService _userService;
        private ApplicationUserManager _userManager;
        private Mock<IUserStore<ApplicationUser>> _mockStore;

        public UserServiceTests()
        {
            _mockStore = Mock.Create<IUserStore<ApplicationUser>>();
            _mockStore.As<IUserPasswordStore<ApplicationUser>>();

            //Cannot moq the interface directly, consider creating a facade around the manager class
            _userManager = new ApplicationUserManager(_mockStore.Object);
            _userService = new UserService(_userManager);
        }

        [Fact]
        public async void CreateClaimsIdentity_Should_Return_Identity()
        {

            string user = "bob@bob.com";
            var model = new ApplicationUser() { UserName = user };

            _mockStore.Setup(_ => _.FindByNameAsync(user))
                .ReturnsAsync(model);

            var result = await _userService.CreateClaimsIdentityAsync(user, "OAuth");

            result.Should().NotBeNull();
            result.HasClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", user).Should().BeTrue();

            Mock.VerifyAll();

        }

        [Fact]
        public void CreateClaimsIdentity_Should_Throw_ArgumentException_when_User_Is_Not_Found()
        {
            string user = "bob@bob.com";

            _mockStore.Setup(_ => _.FindByNameAsync(user))
                .ReturnsAsync(null);


            Func<Task> throwAction = async () =>
                await _userService.CreateClaimsIdentityAsync(user, "OAuth");

            throwAction.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public async void IsValidCredential_Should_Return_True_When_User_Is_Found()
        {
            string user = "bob@bob.com";
            string password = "giraffe";
            var hpw = new PasswordHasher().HashPassword(password);
            var model = new ApplicationUser();

            _mockStore.Setup(_ => _.FindByNameAsync(user)).ReturnsAsync(model);
            _mockStore.As<IUserPasswordStore<ApplicationUser>>()
                .Setup(_ => _.GetPasswordHashAsync(model))
                .ReturnsAsync(hpw);
            var result = await _userService.IsValidCredential(user, password);

            Mock.VerifyAll();
            result.Should().BeTrue();
        }

        [Fact]
        public async void IsValidCredential_Should_Return_False_When_User_Is_Not_Found()
        {
            string user = "bob@bob.cm";
            string password = "giraffe";
            var hash = Convert.ToBase64String(new byte[] { 1, 2, 3 });
            var model = new ApplicationUser();

            _mockStore.Setup(_ => _.FindByNameAsync(user)).ReturnsAsync(model);
            _mockStore.As<IUserPasswordStore<ApplicationUser>>()
                .Setup(_ => _.GetPasswordHashAsync(model))
                .ReturnsAsync(hash);


            var result = await _userService.IsValidCredential(user, password);


            Mock.VerifyAll();
            result.Should().BeFalse();
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_UserManager_IsNull()
        {
            Action throwAction = () => new UserService(null);

            throwAction.ShouldThrow<ArgumentNullException>()
                .And
                .ParamName
                .Should()
                .Be("userManager");
        }

        [Fact]
        public async Task Register_Should_Call_UserManager()
        {

            //ARRANGE

            var model = Fixture.Build<RegistrationModel>()
                .With(_ => _.Email, "test@test.com")
                .With(_ => _.Password, "cool1Password!!")
                .Create();

            var identityResult = new IdentityResult();

            _mockStore.As<IUserPasswordStore<ApplicationUser>>()
                .Setup(_ => _.SetPasswordHashAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Returns(Task.Delay(0));

            _mockStore.Setup(_ => _.FindByNameAsync(It.Is<string>(match => match == model.Email)))
                .ReturnsAsync(null);

            _mockStore.Setup(_ => _.CreateAsync(It.Is<ApplicationUser>(match => match.Email == model.Email && match.UserName == model.Email)))
                .Returns(Task.Delay(0));

            //ACT
            var result = await _userService.RegisterAsync(model);

            //ASSERT
            Mock.VerifyAll();
        }

        [Fact]
        public async Task Register_Should_Return_Error_When_Model_Is_Invalid()
        {

            //ARRANGE

            var model = Fixture.Build<RegistrationModel>()
                .With(_ => _.Email, "testtestcom")
                .With(_ => _.Password, "cool1Password!!")
                .Create();

            _mockStore.As<IUserPasswordStore<ApplicationUser>>()
                .Setup(_ => _.SetPasswordHashAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Returns(Task.Delay(0));

            _mockStore.Setup(_ => _.FindByNameAsync(It.Is<string>(match => match == model.Email)))
                .ReturnsAsync(new ApplicationUser());

            //ACT
            var result = await _userService.RegisterAsync(model);


            //ASSERT
            Mock.VerifyAll();
            result.Succeeded.Should().BeFalse();
        }
    }
}
