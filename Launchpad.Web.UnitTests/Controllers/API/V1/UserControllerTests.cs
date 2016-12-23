using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Routing;
using FluentAssertions;
using Launchpad.Models;
using Launchpad.Services.User;
using Launchpad.UnitTests.Common;
using Launchpad.Web.Controllers.API.V1;
using Moq;
using Ploeh.AutoFixture;
using Xunit;
using static Launchpad.Web.Constants;

namespace Launchpad.Web.UnitTests.Controllers.API.V1
{
    [Trait("Category", "User.Controller")]
    public class UserControllerTests : BaseUnitTest
    {
        private readonly UserController _userController;
        private readonly Mock<UrlHelper> _mockUrlHelper;
        private readonly Mock<IUserService> _mockUserService;

        public UserControllerTests()
        {
            _mockUserService = Mock.Create<IUserService>();
            _mockUrlHelper = Mock.Create<UrlHelper>();

            _userController = new UserController(_mockUserService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var claimIdentity = new ClaimsIdentity();
            claimIdentity.AddClaim(new Claim("fakeClaim", "fake"));
            claimIdentity.AddClaim(new Claim(LssClaims.Type, "view.test"));
            _userController.User = new ClaimsPrincipal(claimIdentity);
            _userController.Url = _mockUrlHelper.Object;
        }

        [Fact]
        public void CreateUser_Should_Have_ClaimAuthorizeAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _userController.AssertClaim(_ => _.CreateUser(new UserModel()), LssClaims.CreateUser);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void CreateUser_Should_Have_HttpPostAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _userController.AssertAttribute<UserController, HttpPostAttribute>(_ => _.CreateUser(new UserModel()));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void CreateUser_Should_Have_RouteAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _userController.AssertRoute(_ => _.CreateUser(new UserModel()), "users");
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public async void CreateUser_Should_Call_UserService_And_Return_Created_On_Success()
        {
            // ARRANGE
            var inputModel = Fixture.Create<UserModel>();
            var entityResult = new EntityResult<UserModel>(Fixture.Create<UserModel>(), true, false);

            _mockUserService.Setup(_ => _.CreateUserAsync(inputModel))
                .ReturnsAsync(entityResult);

            const string url = "http://testlink.com";

            // Matcher for determining if route params match
            Func<Dictionary<string, object>, bool> routeDictionaryMatcher = routeDictionary =>
            {
                routeDictionary.ContainsKey("UserId").Should().BeTrue();
                routeDictionary["UserId"].ToString().Should().Be(entityResult.Model.Id);
                return true;
            };

            _mockUrlHelper.Setup(_ => _.Link("GetUserAsync", It.Is<Dictionary<string, object>>(arg => routeDictionaryMatcher(arg))))
                .Returns(url);

            // ACT
            var result = await _userController.CreateUser(inputModel);

            // ASSERT
            var message = await result.ExecuteAsync(CreateCancelToken());
            message.StatusCode.Should().Be(HttpStatusCode.Created);
            message.Headers.Location.Should().Be(url);
            UserModel messageModel;
            message.TryGetContentValue(out messageModel).Should().BeTrue();
            messageModel.ShouldBeEquivalentTo(entityResult.Model);
        }

        [Fact]
        public async void CreateUser_Should_Call_UserService_And_Return_BadRequest_On_Failure()
        {
            // ARRANGE
            var inputModel = Fixture.Create<UserModel>();
            var serviceResult = new EntityResult<UserModel>(Fixture.Create<UserModel>(), false, false, "err1");

            _mockUserService.Setup(_ => _.CreateUserAsync(inputModel))
                .ReturnsAsync(serviceResult);

            // ACT
            var result = await _userController.CreateUser(inputModel);

            // ASSERT
            var message = await result.ExecuteAsync(CreateCancelToken());
            message.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void DeleteUser_Should_Have_ClaimAuthorizeAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _userController.AssertClaim(_ => _.DeleteUser("id"), LssClaims.DeleteUser);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void DeleteUser_Should_Have_HttpDeleteAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _userController.AssertAttribute<UserController, HttpDeleteAttribute>(_ => _.DeleteUser("Id"));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void DeleteUser_Should_Have_RouteAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _userController.AssertRoute(_ => _.DeleteUser("id"), "users/{userId}");
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public async void DeleteUser_Should_Call_Service_And_Return_NoContent_On_Success()
        {
            var id = Fixture.Create<string>();
            var entityResult = new EntityResult(true, false);

            _mockUserService.Setup(_ => _.DeleteUserAsync(id))
                .ReturnsAsync(entityResult);

            var result = await _userController.DeleteUser(id);

            var message = await result.ExecuteAsync(CreateCancelToken());
            message.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async void DeleteUser_Should_Call_Service_And_Return_BadRequest_On_Failure()
        {
            var id = Fixture.Create<string>();
            var entityResult = new EntityResult(false, false, "error");

            _mockUserService.Setup(_ => _.DeleteUserAsync(id))
                .ReturnsAsync(entityResult);

            var result = await _userController.DeleteUser(id);

            var message = await result.ExecuteAsync(CreateCancelToken());

            message.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void UpdateUser_Should_Have_ClaimAuthorizeAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _userController.AssertClaim(_ => _.UpdateUser("id", new UserModel()), LssClaims.UpdateUser);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void UpdateUser_Should_Have_HttpPutAtribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _userController.AssertAttribute<UserController, HttpPutAttribute>(_ => _.UpdateUser("id", new UserModel()));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void UpdateUser_Should_Have_RouteAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _userController.AssertRoute(_ => _.UpdateUser("id", new UserModel()), "users/{userId}");
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public async void UpdateUser_Should_Call_UserService()
        {
            var userModel = Fixture.Create<UserModel>();
            var returnModel = Fixture.Create<UserModel>();
            var entityResult = new EntityResult<UserModel>(returnModel, true, false);
            var id = Fixture.Create<string>();

            _mockUserService.Setup(_ => _.UpdateUserAsync(id, userModel))
                .ReturnsAsync(entityResult);

            var result = await _userController.UpdateUser(id, userModel);

            var message = await result.ExecuteAsync(new CancellationToken());
            message.StatusCode.Should().Be(HttpStatusCode.OK);

            UserModel messageModel;
            message.TryGetContentValue(out messageModel).Should().BeTrue();

            messageModel.ShouldBeEquivalentTo(returnModel);
        }

        [Fact]
        public void GetUserRoleById_Should_Have_HttpGetAttribute()
        {
            _userController.AssertAttribute<UserController, HttpGetAttribute>(_ => _.GetUserRoleById("userId", "roleId"));
        }

        [Fact]
        public void GetUserRoleById_Should_Have_RouteAttribute()
        {
            _userController.AssertRoute(_ => _.GetUserRoleById("userId", "roleId"), "users/{userId}/roles/{roleId}");
        }

        [Fact]
        public void GetUserRoleById_Should_Have_ClaimAuthorizeAttribute()
        {
            _userController.AssertClaim(_ => _.GetUserRoleById("userId", "roleId"), LssClaims.ViewRole);
        }

        [Fact]
        public async void GetUserRoleById_Should_Call_UserService()
        {
            var model = Fixture.Create<RoleModel>();
            var userId = Fixture.Create<string>();
            var roleId = Fixture.Create<string>();
            var entityResult = new EntityResult<RoleModel>(model, true, false);

            _mockUserService.Setup(_ => _.GetUserRoleById(userId, roleId))
                .Returns(entityResult);

            var result = _userController.GetUserRoleById(userId, roleId);

            var message = await result.ExecuteAsync(new CancellationToken());
            message.StatusCode.Should().Be(HttpStatusCode.OK);

            RoleModel messageModel;
            message.TryGetContentValue(out messageModel).Should().BeTrue();

            messageModel.ShouldBeEquivalentTo(model);
        }

        [Fact]
        public void GetUsers_Should_Have_RouteAttribute()
        {
            _userController.AssertRoute(_ => _.GetUsers(), "users");
        }

        [Fact]
        public void GetUsers_Should_Have_ClaimAuthorizeAttribute()
        {
            _userController.AssertClaim(
                _ => _.GetUsers(),
                LssClaims.ListUsers);
        }

        [Fact]
        public void GetClaims_Should_Have_ClaimAuthorizeAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _userController.AssertClaim(_ => _.GetClaims("abc"), LssClaims.ListUserClaims);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void AssignRole_Should_Have_ClaimAuthorizeAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _userController.AssertClaim(_ => _.AssignRole("userId", new RoleModel()), LssClaims.AssignRole);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public async void GetClaims_Should_Call_User_Service()
        {
            var id = "abc";
            var fakeClaims = Fixture.CreateMany<ClaimModel>().ToList();
            var entityResult = new EntityResult<IEnumerable<ClaimModel>>(fakeClaims, true, false);

            _mockUserService.Setup(_ => _.GetUserClaimsAsync(id))
                .ReturnsAsync(entityResult);

            var result = await _userController.GetClaims(id);

            var message = await result.ExecuteAsync(new CancellationToken());

            message.StatusCode.Should().Be(HttpStatusCode.OK);

            IEnumerable<ClaimModel> models;
            message.TryGetContentValue(out models).Should().BeTrue();

            models.Should().BeEquivalentTo(fakeClaims);
        }

        [Fact]
        public async void GetUsers_Should_Call_UserService()
        {
            // Arrange 
            var users = Fixture.CreateMany<UserModel>();
            var entityResult = new EntityResult<IEnumerable<UserModel>>(users, true, false);

            _mockUserService.Setup(_ => _.GetUsers())
                .Returns(entityResult);

            var result = _userController.GetUsers();

            var message = await result.ExecuteAsync(new CancellationToken());
            message.StatusCode.Should().Be(HttpStatusCode.OK);

            IEnumerable<UserModel> models;
            message.TryGetContentValue(out models).Should().BeTrue();

            message.StatusCode.Should().Be(HttpStatusCode.OK);

            Mock.VerifyAll();
            models.ShouldBeEquivalentTo(users);
        }

        [Fact]
        public async void Assign_Should_Call_User_Service_And_Return_Created_When_Successful()
        {
            // arrange
            var userId = Fixture.Create<string>();
            var model = Fixture.Create<RoleModel>();
            var serviceModel = Fixture.Create<RoleModel>();
            var identityResult = new EntityResult<RoleModel>(serviceModel, true, false);
            _mockUserService.Setup(_ => _.AssignUserRoleAsync(userId, model))
                .ReturnsAsync(identityResult);

            const string url = "http://testlink.com";

            // Matcher for determining if route params match
            Func<Dictionary<string, object>, bool> routeDictionaryMatcher = routeDictionary =>
            {
                routeDictionary.ContainsKey("UserId").Should().BeTrue();
                routeDictionary["UserId"].ToString().Should().Be(userId);
                routeDictionary.ContainsKey("RoleId").Should().BeTrue();
                routeDictionary["RoleId"].ToString().Should().Be(serviceModel.Id);
                return true;
            };

            _mockUrlHelper.Setup(_ => _.Link("GetUserRoleById", It.Is<Dictionary<string, object>>(arg => routeDictionaryMatcher(arg))))
                .Returns(url);

            // act
            var result = await _userController.AssignRole(userId, model);

            // assert
            var message = await result.ExecuteAsync(new CancellationToken());
            message.Headers.Location.Should().Be(url);
            message.StatusCode.Should().Be(HttpStatusCode.Created);
            Mock.VerifyAll();
        }

        [Fact]
        public async void Assign_Should_Call_User_Service_And_Return_BadRequest_When_Role_Assignment_Fails()
        {
            // arrange
            var userId = Fixture.Create<string>();
            var model = Fixture.Create<RoleModel>();
            var identityResult = new EntityResult<RoleModel>(null, false, false);
            _mockUserService.Setup(_ => _.AssignUserRoleAsync(userId, model))
                .ReturnsAsync(identityResult);

            // act
            var result = await _userController.AssignRole(userId, model);

            // assert
            Mock.VerifyAll();

            var message = await result.ExecuteAsync(new CancellationToken());

            message.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void GetUsers_Should_Be_Decorated_With_HttpGetAttribute()
        {
            ReflectionHelper.GetMethod<UserController>(_ => _.GetUsers())
                .Should().BeDecoratedWith<HttpGetAttribute>();
        }

        [Fact]
        public void AssignRole_Should_Be_Decorated_With_HttpGetAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            ReflectionHelper.GetMethod<UserController>(_ => _.AssignRole("userId", new RoleModel()))
                .Should().BeDecoratedWith<HttpPostAttribute>();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void AssignRole_Should_Be_Decorated_With_RouteAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            ReflectionHelper.GetMethod<UserController>(_ => _.AssignRole("userId", new RoleModel()))
                .Should().BeDecoratedWith<RouteAttribute>(_ => _.Template == "users/{userId}/roles");
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
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
            typeof(UserController).Should()
                .BeDecoratedWith<RoutePrefixAttribute>(
                _ => _.Prefix.Equals(RoutePrefixes.V1));
        }

        [Fact]
        public void Class_Should_Have_Authorize_Attribute()
        {
            typeof(UserController).Should()
                .BeDecoratedWith<AuthorizeAttribute>();
        }

        [Fact]
        public void GetClaims_Should_Have_HttpGetAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            ReflectionHelper.GetMethod<UserController>(_ => _.GetClaims("abc"))
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                .Should()
                .BeDecoratedWith<HttpGetAttribute>();
        }

        [Fact]
        public void GetClaims_Should_Have_RouteAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            ReflectionHelper.GetMethod<UserController>(_ => _.GetClaims("abc"))
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
              .Should()
              .BeDecoratedWith<RouteAttribute>(_ => _.Template == "users/{userId}/claims");
        }

        [Fact]
        public void RemoveRole_Should_Have_HttpPostAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _userController.AssertAttribute<UserController, HttpDeleteAttribute>(_ => _.RemoveRole("userId", "roleId"));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void RemoveRole_Should_Have_RouteAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _userController.AssertRoute(_ => _.RemoveRole("userId", "roleId"), "users/{userId}/roles/{roleId}");
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void RevokeRole_Should_Have_ClaimAuthorizeAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _userController.AssertClaim(_ => _.RemoveRole("userId", "roleId"), LssClaims.RevokeRole);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public async void RemoveRole_Should_Call_UserService()
        {
            var userId = Fixture.Create<string>();
            var roleId = Fixture.Create<string>();

            var entityResult = new EntityResult(true, false);

            _mockUserService.Setup(_ => _.RemoveUserFromRoleAsync(userId, roleId))
                .ReturnsAsync(entityResult);

            var result = await _userController.RemoveRole(userId, roleId);

            var message = await result.ExecuteAsync(new CancellationToken());

            message.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public void GetUser_Should_Have_HttpGetAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _userController.AssertAttribute<UserController, HttpGetAttribute>(_ => _.GetUser("id"));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void GetUser_Should_Have_ClaimAuthorizeAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _userController.AssertClaim(_ => _.GetUser("id"), LssClaims.ViewUser);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void GetUser_Should_Have_RouteAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _userController.AssertRoute(_ => _.GetUser("id"), "users/{userId}");
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public async void GetUser_Should_Call_User_Service()
        {
            // ARRANGE
            var id = Fixture.Create<string>();
            var user = Fixture.Create<UserModel>();
            var entityResult = new EntityResult<UserModel>(user, true, false);
            _mockUserService.Setup(_ => _.GetUserAsync(id))
                .ReturnsAsync(entityResult);

            // ACT
            var result = await _userController.GetUser(id);

            // ASSERT
            var message = await result.ExecuteAsync(new CancellationToken());

            message.StatusCode.Should().Be(HttpStatusCode.OK);

            UserModel messageModel;
            message.TryGetContentValue(out messageModel).Should().BeTrue();
            messageModel.ShouldBeEquivalentTo(user);
        }
    }
}
