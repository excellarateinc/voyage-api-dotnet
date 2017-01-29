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

using Launchpad.Core.Exceptions;
using Launchpad.Models;
using Launchpad.Services.User;
using Launchpad.UnitTests.Common;
using Launchpad.Web.Controllers.API.V1;
using Microsoft.AspNet.Identity;
using Moq;
using Ploeh.AutoFixture;
using Xunit;
using Constants = Launchpad.Web.Constants;

namespace Launchpad.UnitTests.Web.Controllers.API.V1
{
    [Trait("Category", "User.Controller")]
    public class UserControllerTests : BaseUnitTest
    {
        private readonly UsersController _usersController;
        private readonly Mock<UrlHelper> _mockUrlHelper;
        private readonly Mock<IUserService> _mockUserService;

        public UserControllerTests()
        {
            _mockUserService = Mock.Create<IUserService>();
            _mockUrlHelper = Mock.Create<UrlHelper>();

            _usersController = new UsersController(_mockUserService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var claimIdentity = new ClaimsIdentity();
            claimIdentity.AddClaim(new Claim("fakeClaim", "fake"));
            claimIdentity.AddClaim(new Claim(Constants.LssClaims.Type, "view.test"));
            _usersController.User = new ClaimsPrincipal(claimIdentity);
            _usersController.Url = _mockUrlHelper.Object;
        }

        [Fact]
        public void CreateUser_Should_Have_ClaimAuthorizeAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _usersController.AssertClaim(_ => _.CreateUser(new UserModel()), Constants.LssClaims.CreateUser);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void CreateUser_Should_Have_HttpPostAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _usersController.AssertAttribute<UsersController, HttpPostAttribute>(_ => _.CreateUser(new UserModel()));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void CreateUser_Should_Have_RouteAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _usersController.AssertRoute(_ => _.CreateUser(new UserModel()), "users");
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public async void CreateUser_Should_Call_UserService_And_Return_Created_On_Success()
        {
            // ARRANGE
            var inputModel = Fixture.Create<UserModel>();
            var returnModel = Fixture.Create<UserModel>();

            _mockUserService.Setup(_ => _.CreateUserAsync(inputModel))
                .ReturnsAsync(returnModel);

            const string url = "http://testlink.com";

            // Matcher for determining if route params match
            Func<Dictionary<string, object>, bool> routeDictionaryMatcher = routeDictionary =>
            {
                routeDictionary.ContainsKey("UserId").Should().BeTrue();
                routeDictionary["UserId"].ToString().Should().Be(returnModel.Id);
                return true;
            };

            _mockUrlHelper.Setup(_ => _.Link("GetUserAsync", It.Is<Dictionary<string, object>>(arg => routeDictionaryMatcher(arg))))
                .Returns(url);

            // ACT
            var result = await _usersController.CreateUser(inputModel);

            // ASSERT
            var message = await result.ExecuteAsync(CreateCancelToken());
            message.StatusCode.Should().Be(HttpStatusCode.Created);
            message.Headers.Location.Should().Be(url);
            UserModel messageModel;
            message.TryGetContentValue(out messageModel).Should().BeTrue();
            messageModel.ShouldBeEquivalentTo(returnModel);
        }

        [Fact]
        public void CreateUser_Should_Call_UserService_And_Return_BadRequest_On_Failure()
        {
            // ARRANGE
            var inputModel = Fixture.Create<UserModel>();

            _mockUserService.Setup(_ => _.CreateUserAsync(inputModel)).Throws<BadRequestException>();

            // ASSERT
            Assert.ThrowsAsync<BadRequestException>(async () => await _usersController.CreateUser(inputModel));
        }

        [Fact]
        public void DeleteUser_Should_Have_ClaimAuthorizeAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _usersController.AssertClaim(_ => _.DeleteUser("id"), Constants.LssClaims.DeleteUser);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void DeleteUser_Should_Have_HttpDeleteAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _usersController.AssertAttribute<UsersController, HttpDeleteAttribute>(_ => _.DeleteUser("Id"));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void DeleteUser_Should_Have_RouteAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _usersController.AssertRoute(_ => _.DeleteUser("id"), "users/{userId}");
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public async void DeleteUser_Should_Call_Service_And_Return_OK_On_Success()
        {
            var id = Fixture.Create<string>();

            _mockUserService.Setup(_ => _.DeleteUserAsync(id))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _usersController.DeleteUser(id);

            var message = await result.ExecuteAsync(CreateCancelToken());
            message.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async void DeleteUser_Should_Call_Service_And_Return_BadRequest_On_Failure()
        {
            var id = Fixture.Create<string>();

            _mockUserService.Setup(_ => _.DeleteUserAsync(id))
                .ReturnsAsync(new IdentityResult());

            var result = await _usersController.DeleteUser(id);

            var message = await result.ExecuteAsync(CreateCancelToken());

            message.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void UpdateUser_Should_Have_ClaimAuthorizeAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _usersController.AssertClaim(_ => _.UpdateUser("id", new UserModel()), Constants.LssClaims.UpdateUser);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void UpdateUser_Should_Have_HttpPutAtribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _usersController.AssertAttribute<UsersController, HttpPutAttribute>(_ => _.UpdateUser("id", new UserModel()));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void UpdateUser_Should_Have_RouteAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _usersController.AssertRoute(_ => _.UpdateUser("id", new UserModel()), "users/{userId}");
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public async void UpdateUser_Should_Call_UserService()
        {
            var userModel = Fixture.Create<UserModel>();
            var returnModel = Fixture.Create<UserModel>();
            var id = Fixture.Create<string>();

            _mockUserService.Setup(_ => _.UpdateUserAsync(id, userModel))
                .ReturnsAsync(returnModel);

            var result = await _usersController.UpdateUser(id, userModel);

            var message = await result.ExecuteAsync(new CancellationToken());
            message.StatusCode.Should().Be(HttpStatusCode.OK);

            UserModel messageModel;
            message.TryGetContentValue(out messageModel).Should().BeTrue();

            messageModel.ShouldBeEquivalentTo(returnModel);
        }

        [Fact]
        public void GetUserRoleById_Should_Have_HttpGetAttribute()
        {
            _usersController.AssertAttribute<UsersController, HttpGetAttribute>(_ => _.GetUserRoleById("userId", "roleId"));
        }

        [Fact]
        public void GetUserRoleById_Should_Have_RouteAttribute()
        {
            _usersController.AssertRoute(_ => _.GetUserRoleById("userId", "roleId"), "users/{userId}/roles/{roleId}");
        }

        [Fact]
        public void GetUserRoleById_Should_Have_ClaimAuthorizeAttribute()
        {
            _usersController.AssertClaim(_ => _.GetUserRoleById("userId", "roleId"), Constants.LssClaims.ViewRole);
        }

        [Fact]
        public async void GetUserRoleById_Should_Call_UserService()
        {
            var model = Fixture.Create<RoleModel>();
            var userId = Fixture.Create<string>();
            var roleId = Fixture.Create<string>();

            _mockUserService.Setup(_ => _.GetUserRoleById(userId, roleId))
                .Returns(model);

            var result = _usersController.GetUserRoleById(userId, roleId);

            var message = await result.ExecuteAsync(new CancellationToken());
            message.StatusCode.Should().Be(HttpStatusCode.OK);

            RoleModel messageModel;
            message.TryGetContentValue(out messageModel).Should().BeTrue();

            messageModel.ShouldBeEquivalentTo(model);
        }

        [Fact]
        public void GetUsers_Should_Have_RouteAttribute()
        {
            _usersController.AssertRoute(_ => _.GetUsers(), "users");
        }

        [Fact]
        public void GetUsers_Should_Have_ClaimAuthorizeAttribute()
        {
            _usersController.AssertClaim(
                _ => _.GetUsers(),
                Constants.LssClaims.ListUsers);
        }

        [Fact]
        public void GetClaims_Should_Have_ClaimAuthorizeAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _usersController.AssertClaim(_ => _.GetClaims("abc"), Constants.LssClaims.ListUserClaims);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void AssignRole_Should_Have_ClaimAuthorizeAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _usersController.AssertClaim(_ => _.AssignRole("userId", new RoleModel()), Constants.LssClaims.AssignRole);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public async void GetClaims_Should_Call_User_Service()
        {
            var id = "abc";
            var fakeClaims = Fixture.CreateMany<ClaimModel>().ToList();

            _mockUserService.Setup(_ => _.GetUserClaimsAsync(id))
                .ReturnsAsync(fakeClaims);

            var result = await _usersController.GetClaims(id);

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

            _mockUserService.Setup(_ => _.GetUsers())
                .Returns(users);

            var result = _usersController.GetUsers();

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
            _mockUserService.Setup(_ => _.AssignUserRoleAsync(userId, model))
                .ReturnsAsync(serviceModel);

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
            var result = await _usersController.AssignRole(userId, model);

            // assert
            var message = await result.ExecuteAsync(new CancellationToken());
            message.Headers.Location.Should().Be(url);
            message.StatusCode.Should().Be(HttpStatusCode.Created);
            Mock.VerifyAll();
        }

        [Fact]
        public void Assign_Should_Call_User_Service_And_Return_BadRequest_When_Role_Assignment_Fails()
        {
            // arrange
            var userId = Fixture.Create<string>();
            var model = Fixture.Create<RoleModel>();
            _mockUserService.Setup(_ => _.AssignUserRoleAsync(userId, model)).Throws<BadRequestException>();

            // assert
            Assert.ThrowsAsync<BadRequestException>(async () => { await _usersController.AssignRole(userId, model); });
        }

        [Fact]
        public void GetUsers_Should_Be_Decorated_With_HttpGetAttribute()
        {
            ReflectionHelper.GetMethod<UsersController>(_ => _.GetUsers())
                .Should().BeDecoratedWith<HttpGetAttribute>();
        }

        [Fact]
        public void AssignRole_Should_Be_Decorated_With_HttpGetAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            ReflectionHelper.GetMethod<UsersController>(_ => _.AssignRole("userId", new RoleModel()))
                .Should().BeDecoratedWith<HttpPostAttribute>();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void AssignRole_Should_Be_Decorated_With_RouteAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            ReflectionHelper.GetMethod<UsersController>(_ => _.AssignRole("userId", new RoleModel()))
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
            typeof(UsersController).Should()
                .BeDecoratedWith<RoutePrefixAttribute>(
                _ => _.Prefix.Equals(Constants.RoutePrefixes.V1));
        }

        [Fact]
        public void GetClaims_Should_Have_HttpGetAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            ReflectionHelper.GetMethod<UsersController>(_ => _.GetClaims("abc"))
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                .Should()
                .BeDecoratedWith<HttpGetAttribute>();
        }

        [Fact]
        public void GetClaims_Should_Have_RouteAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            ReflectionHelper.GetMethod<UsersController>(_ => _.GetClaims("abc"))
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
              .Should()
              .BeDecoratedWith<RouteAttribute>(_ => _.Template == "users/{userId}/claims");
        }

        [Fact]
        public void RemoveRole_Should_Have_HttpPostAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _usersController.AssertAttribute<UsersController, HttpDeleteAttribute>(_ => _.RemoveRole("userId", "roleId"));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void RemoveRole_Should_Have_RouteAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _usersController.AssertRoute(_ => _.RemoveRole("userId", "roleId"), "users/{userId}/roles/{roleId}");
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void RevokeRole_Should_Have_ClaimAuthorizeAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _usersController.AssertClaim(_ => _.RemoveRole("userId", "roleId"), Constants.LssClaims.RevokeRole);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public async void RemoveRole_Should_Call_UserService()
        {
            var userId = Fixture.Create<string>();
            var roleId = Fixture.Create<string>();

            _mockUserService.Setup(_ => _.RemoveUserFromRoleAsync(userId, roleId))
                .ReturnsAsync(new IdentityResult());

            var result = await _usersController.RemoveRole(userId, roleId);

            var message = await result.ExecuteAsync(new CancellationToken());

            message.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public void GetUser_Should_Have_HttpGetAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _usersController.AssertAttribute<UsersController, HttpGetAttribute>(_ => _.GetUser("id"));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void GetUser_Should_Have_ClaimAuthorizeAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _usersController.AssertClaim(_ => _.GetUser("id"), Constants.LssClaims.ViewUser);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void GetUser_Should_Have_RouteAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _usersController.AssertRoute(_ => _.GetUser("id"), "users/{userId}");
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public async void GetUser_Should_Call_User_Service()
        {
            // ARRANGE
            var id = Fixture.Create<string>();
            var user = Fixture.Create<UserModel>();
            _mockUserService.Setup(_ => _.GetUserAsync(id))
                .ReturnsAsync(user);

            // ACT
            var result = await _usersController.GetUser(id);

            // ASSERT
            var message = await result.ExecuteAsync(new CancellationToken());

            message.StatusCode.Should().Be(HttpStatusCode.OK);

            UserModel messageModel;
            message.TryGetContentValue(out messageModel).Should().BeTrue();
            messageModel.ShouldBeEquivalentTo(user);
        }
    }
}
