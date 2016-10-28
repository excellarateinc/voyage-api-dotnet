using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using Ploeh.AutoFixture;
using Launchpad.UnitTests.Common;
using System.Web.Http;
using Launchpad.Web.Controllers.API;
using Launchpad.Web.AppIdentity;
using Launchpad.Models;
using Microsoft.AspNet.Identity;
using System.Net;
using Launchpad.Models.EntityFramework;

namespace Launchpad.Web.UnitTests.Controllers.API
{
    public class AccountControllerTests : BaseUnitTest
    {
        private ApplicationUserManager _userManager;
        private AccountController _accountController;
        private Mock<IUserStore<ApplicationUser>> _mockStore;

        public AccountControllerTests()
        {
            _mockStore = Mock.Create<IUserStore<ApplicationUser>>();
            _mockStore.As<IUserPasswordStore<ApplicationUser>>();

            //Cannot moq the interface directly, consider creating a facade around the manager class
            _userManager = new ApplicationUserManager(_mockStore.Object);

            _accountController = new AccountController(_userManager);
            _accountController.Request = new System.Net.Http.HttpRequestMessage();
            _accountController.Configuration = new System.Web.Http.HttpConfiguration();
        }

        [Fact]
        public async Task Register_Should_Call_UserManager() {

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
            var result = await _accountController.Register(model);


            //ASSERT
            Mock.VerifyAll();
            var message = await result.ExecuteAsync(new System.Threading.CancellationToken());

            message.StatusCode.Should().Be(HttpStatusCode.OK);

        }

        [Fact]
        public async Task Register_Should_Return_BadRequest_When_Model_Is_Invalid()
        {

            //ARRANGE

            var model = Fixture.Build<RegistrationModel>()
                .With(_ => _.Email, "testtestcom")
                .With(_ => _.Password, "cool1Password!!")
                .Create();

            var identityResult = new IdentityResult();

            _mockStore.As<IUserPasswordStore<ApplicationUser>>()
                .Setup(_ => _.SetPasswordHashAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Returns(Task.Delay(0));

            _mockStore.Setup(_ => _.FindByNameAsync(It.Is<string>(match => match == model.Email)))
                .ReturnsAsync(new ApplicationUser());

            //ACT
            var result = await _accountController.Register(model);


            //ASSERT
            Mock.VerifyAll();
            var message = await result.ExecuteAsync(new System.Threading.CancellationToken());

            message.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_UserManager_IsNull()
        {
            Action throwAction = () => new AccountController(null);

            throwAction.ShouldThrow<ArgumentNullException>()
                .And
                .ParamName
                .Should()
                .Be("userManager");
        }

        [Fact]
        public void Class_Should_Have_RoutePrefix_Attribute()
        {
            typeof(AccountController).Should()
                .BeDecoratedWith<RoutePrefixAttribute>(
                _ => _.Prefix.Equals(Constants.RoutePrefixes.Account));
        }

        [Fact]
        public void Register_Should_Have_Route_Attribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            ReflectionHelper.GetMethod<AccountController>(_ => _.Register(new Models.RegistrationModel()))
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                .Should()
                .BeDecoratedWith<RouteAttribute>(_ => _.Template.Equals("register"));
              
        }

        [Fact]
        public void Register_Should_Have_AllowAnonymous_Attribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            ReflectionHelper.GetMethod<AccountController>(_ => _.Register(new Models.RegistrationModel()))
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                .Should()
                .BeDecoratedWith<AllowAnonymousAttribute>();

        }


    }
}
