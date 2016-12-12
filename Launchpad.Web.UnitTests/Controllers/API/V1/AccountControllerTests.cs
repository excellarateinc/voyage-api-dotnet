using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using FluentAssertions;
using Launchpad.Models;
using Launchpad.Services.Interfaces;
using Launchpad.UnitTests.Common;
using Launchpad.Web.Controllers.API.V1;
using Moq;
using Ploeh.AutoFixture;
using Xunit;

namespace Launchpad.Web.UnitTests.Controllers.API.V1
{
    public class AccountControllerTests : BaseUnitTest
    {
        private readonly AccountController _accountController;
        private readonly Mock<IUserService> _mockUserService;

        public AccountControllerTests()
        {
            _mockUserService = Mock.Create<IUserService>();
            _accountController = new AccountController(_mockUserService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }

        [Fact]
        public async Task Register_Should_Call_UserManager()
        {
            // ARRANGE
            var model = Fixture.Build<RegistrationModel>()
                .With(_ => _.Email, "test@test.com")
                .With(_ => _.Password, "cool1Password!!")
                .Create();

            var entityResult = new EntityResult(true, false);
        
            _mockUserService.Setup(_ => _.RegisterAsync(model))
                .ReturnsAsync(entityResult);

            // ACT
            var result = await _accountController.Register(model);

            // ASSERT
            Mock.VerifyAll();

            var message = await result.ExecuteAsync(new CancellationToken());

            message.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Register_Should_Return_BadRequest_When_Model_Is_Invalid()
        {
            // ARRANGE
            var model = Fixture.Build<RegistrationModel>()
                .With(_ => _.Email, "testtestcom")
                .With(_ => _.Password, "cool1Password!!")
                .Create();

            var entityResult = new EntityResult(false, false, "My error");

            _mockUserService.Setup(_ => _.RegisterAsync(model))
                .ReturnsAsync(entityResult);

            // ACT
            var result = await _accountController.Register(model);

            // ASSERT
            Mock.VerifyAll();
            var message = await result.ExecuteAsync(new CancellationToken());

            message.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_UserService_IsNull()
        {
            Action throwAction = () => new AccountController(null);

            throwAction.ShouldThrow<ArgumentNullException>()
                .And
                .ParamName
                .Should()
                .Be("userService");
        }

        [Fact]
        public void Class_Should_Have_RoutePrefix_Attribute()
        {
            typeof(AccountController).Should()
                .BeDecoratedWith<RoutePrefixAttribute>(
                _ => _.Prefix.Equals(Constants.RoutePrefixes.V1));
        }

        [Fact]
        public void Register_Should_Have_Route_Attribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            ReflectionHelper.GetMethod<AccountController>(_ => _.Register(new RegistrationModel()))
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                .Should()
                .BeDecoratedWith<RouteAttribute>(_ => _.Template.Equals("account/register"));             
        }

        [Fact]
        public void Register_Should_Have_AllowAnonymous_Attribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            ReflectionHelper.GetMethod<AccountController>(_ => _.Register(new RegistrationModel()))
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                .Should()
                .BeDecoratedWith<AllowAnonymousAttribute>();
        }
    }
}
