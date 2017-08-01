//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.Http;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Web.Http;
//using System.Web.Http.Results;
//using System.Web.Http.Routing;
//using FluentAssertions;
//using Microsoft.AspNet.Identity;
//using Moq;
//using Ploeh.AutoFixture;
//using Voyage.Api.API.V1;
//using Voyage.Api.UnitTests.Common;
//using Voyage.Core.Exceptions;
//using Voyage.Models;
//using Voyage.Services.User;
//using Xunit;

// TODO: Fix tests (We need to add mocks for the new services added to the controller)
//namespace Voyage.Api.UnitTests.API.V1
//{
//    public class AccountControllerTests : BaseUnitTest
//    {
//        private readonly AccountController _accountController;
//        private readonly Mock<IUserService> _mockUserService;
//        private readonly Mock<UrlHelper> _mockUrlHelper;

//        public AccountControllerTests()
//        {
//            _mockUserService = Mock.Create<IUserService>();
//            _mockUrlHelper = Mock.Create<UrlHelper>();
//            _accountController = new AccountController(_mockUserService.Object)
//            {
//                Request = new HttpRequestMessage(),
//                Configuration = new HttpConfiguration(),
//                Url = _mockUrlHelper.Object
//        };
//        }

//        [Fact]
//        public async Task Register_Should_Call_UserManager()
//        {
//            // ARRANGE
//            var model = Fixture.Build<RegistrationModel>()
//                .With(_ => _.Email, "test@test.com")
//                .With(_ => _.Password, "cool1Password!!")
//                .Create();

//            _mockUserService.Setup(_ => _.RegisterAsync(model))
//                .ReturnsAsync(new UserModel());

//            _mockUrlHelper.Setup(_ => _.Link("GetUserAsync", It.IsAny<Dictionary<string, object>>()))
//                .Returns("http://testlink.com");

//            // ACT
//            var result = await _accountController.Register(model);
            
//            var message = await result.ExecuteAsync(new CancellationToken());
            
//            message.StatusCode.Should().Be(HttpStatusCode.Created);
//        }

//        [Fact]
//        public async Task Register_Should_Return_BadRequest_When_Model_Is_Invalid()
//        {
//            // ARRANGE
//            var model = Fixture.Build<RegistrationModel>()
//                .With(_ => _.Email, "testtestcom")
//                .With(_ => _.Password, "cool1Password!!")
//                .Create();

//            _mockUserService.Setup(_ => _.RegisterAsync(model)).Throws(new BadRequestException());            

//            // ACT
//            var result = await _accountController.Register(model);

//            // ASSERT
//            var message = await result.ExecuteAsync(new CancellationToken());

//            message.StatusCode.Should().Be(HttpStatusCode.BadRequest);
//        }

//        [Fact]
//        public void Ctor_Should_Throw_ArgumentNullException_When_UserService_IsNull()
//        {
//            Action throwAction = () => new AccountController(null);

//            throwAction.ShouldThrow<ArgumentNullException>()
//                .And
//                .ParamName
//                .Should()
//                .Be("userService");
//        }

//        [Fact]
//        public void Class_Should_Have_RoutePrefix_Attribute()
//        {
//            typeof(AccountController).Should()
//                .BeDecoratedWith<RoutePrefixAttribute>(
//                _ => _.Prefix.Equals(Constants.RoutePrefixes.V1));
//        }

//        [Fact]
//        public void Register_Should_Have_Route_Attribute()
//        {
//#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
//            ReflectionHelper.GetMethod<AccountController>(_ => _.Register(new RegistrationModel()))
//#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
//                .Should()
//                .BeDecoratedWith<RouteAttribute>(_ => _.Template.Equals("profile"));
//        }
//    }
//}
