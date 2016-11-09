using Launchpad.Services.Interfaces;
using Launchpad.UnitTests.Common;
using Launchpad.Web.Controllers.API.V1;
using Moq;
using System;
using System.Collections.Generic;
using Ploeh.AutoFixture;
using Launchpad.Models;
using FluentAssertions;
using System.Net;
using System.Web.Http;
using Xunit;
using System.Net.Http;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Linq;
using static Launchpad.Web.Constants;
using System.Web.Http.Routing;

namespace Launchpad.Web.UnitTests.Controllers.API.V1
{
    [Trait("Category", "User.Controller")]
    public class UserControllerTests : BaseUnitTest
    {
        private UserController _userController;
        private Mock<UrlHelper> _mockUrlHelper;

        private Mock<IUserService> _mockUserService;
        private ClaimsIdentity _claimIdentity;
        public UserControllerTests()
        {
            _mockUserService = Mock.Create<IUserService>();
            _mockUrlHelper = Mock.Create<UrlHelper>();

            _userController = new UserController(_mockUserService.Object);
            _userController.Request = new System.Net.Http.HttpRequestMessage();
            _userController.Configuration = new System.Web.Http.HttpConfiguration();

            _claimIdentity = new ClaimsIdentity();
            _claimIdentity.AddClaim(new Claim("fakeClaim", "fake"));
            _claimIdentity.AddClaim(new Claim(Constants.LssClaims.Type, "view.widget"));
            _userController.User = new ClaimsPrincipal(_claimIdentity);
            _userController.Url = _mockUrlHelper.Object;
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
            var identityResult = new IdentityResult<UserModel>(IdentityResult.Success
                , returnModel);
            var id = Fixture.Create<string>();

            _mockUserService.Setup(_ => _.UpdateUser(id, userModel))
                .ReturnsAsync(identityResult);

            var result = await _userController.UpdateUser(id, userModel);

            var message = await result.ExecuteAsync(new System.Threading.CancellationToken());
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
            _userController.AssertRoute<UserController>(_ => _.GetUserRoleById("userId", "roleId"), "users/{userId}/roles/{roleId}");
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

            _mockUserService.Setup(_ => _.GetUserRoleById(userId, roleId))
                .Returns(model);

         


            var result = _userController.GetUserRoleById(userId, roleId);

            var message = await result.ExecuteAsync(new System.Threading.CancellationToken());
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
            _userController.AssertClaim(_ => _.GetUsers(), 
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
            var fakeClaims = Fixture.CreateMany<ClaimModel>();

            _mockUserService.Setup(_ => _.GetUserClaimsAsync(id))
                .ReturnsAsync(fakeClaims);

            var result = await _userController.GetClaims(id);

            var message = await result.ExecuteAsync(new System.Threading.CancellationToken());

            message.StatusCode.Should().Be(HttpStatusCode.OK);

            IEnumerable<ClaimModel> models;
            message.TryGetContentValue(out models).Should().BeTrue();

            models.Should().BeEquivalentTo(fakeClaims);
        }

        [Fact]
        public async void GetUsers_Should_Call_UserService()
        {
            //Arrange 
            var users = Fixture.CreateMany<UserModel>();

            _mockUserService.Setup(_ => _.GetUsers())
                .Returns(users);

            var result = _userController.GetUsers();


            var message = await result.ExecuteAsync(new System.Threading.CancellationToken());
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
            //arrange
            var userId = Fixture.Create<string>();
            var model = Fixture.Create<RoleModel>();
            var serviceModel = Fixture.Create<RoleModel>();
            var identityResult = new IdentityResult<RoleModel>(IdentityResult.Success, serviceModel);
            _mockUserService.Setup(_ => _.AssignUserRoleAsync(userId, model))
                .ReturnsAsync(identityResult);

            const string url = "http://testlink.com";

            //Matcher for determining if route params match
            Func<Dictionary<string, object>, bool> routeDictionaryMatcher = routeDictionary =>
            {
                routeDictionary.ContainsKey("UserId").Should().BeTrue();
                routeDictionary["UserId"].ToString().Should().Be(userId);
                routeDictionary.ContainsKey("RoleId").Should().BeTrue();
                routeDictionary["RoleId"].ToString().Should().Be(serviceModel.Id);
                return true;
            };


            _mockUrlHelper.Setup(_ => _.Link("GetUserRoleById", It.Is<Dictionary<string,object>>(arg=>routeDictionaryMatcher(arg))))
                .Returns(url);
            

            //act
            var result = await _userController.AssignRole(userId, model);

            

            //assert
          

            var message = await result.ExecuteAsync(new System.Threading.CancellationToken());
            message.Headers.Location.Should().Be(url);
            message.StatusCode.Should().Be(HttpStatusCode.Created);
            Mock.VerifyAll();
        }

        [Fact]
        public async void Assign_Should_Call_User_Service_And_Return_BadRequest_When_Role_Assignment_Fails()
        {
            //arrange
            var userId = Fixture.Create<string>();
            var model = Fixture.Create<RoleModel>();
            var identityResult = new IdentityResult<RoleModel>(new IdentityResult("error"), null);
            _mockUserService.Setup(_ => _.AssignUserRoleAsync(userId, model))
                .ReturnsAsync(identityResult);


            //act
            var result = await _userController.AssignRole(userId, model);


            //assert
            Mock.VerifyAll();

            var message = await result.ExecuteAsync(new System.Threading.CancellationToken());

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
                _ => _.Prefix.Equals(Constants.RoutePrefixes.V1));
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
              .BeDecoratedWith<RouteAttribute>(_=>_.Template == "users/{userId}/claims");
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
          

            _mockUserService.Setup(_ => _.RemoveUserFromRoleAsync(userId, roleId))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _userController.RemoveRole(userId, roleId);

            var message = await result.ExecuteAsync(new System.Threading.CancellationToken());

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
            //ARRANGE
            var id = Fixture.Create<string>();
            var user = Fixture.Create<UserModel>();
            _mockUserService.Setup(_ => _.GetUser(id))
                .ReturnsAsync(user);

            //ACT
            var result = await _userController.GetUser(id);

            //ASSERT
            var message = await result.ExecuteAsync(new System.Threading.CancellationToken());

            message.StatusCode.Should().Be(HttpStatusCode.OK);

            UserModel messageModel;
            message.TryGetContentValue(out messageModel).Should().BeTrue();
            messageModel.ShouldBeEquivalentTo(user);

        }
    }
}
