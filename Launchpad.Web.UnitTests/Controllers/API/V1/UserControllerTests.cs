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


namespace Launchpad.Web.UnitTests.Controllers.API.V1
{
    public class UserControllerTests : BaseUnitTest
    {
        private UserController _userController;
        private Mock<IUserService> _mockUserService;
        private ClaimsIdentity _claimIdentity;
        public UserControllerTests()
        {
            _mockUserService = Mock.Create<IUserService>();
            _userController = new UserController(_mockUserService.Object);
            _userController.Request = new System.Net.Http.HttpRequestMessage();
            _userController.Configuration = new System.Web.Http.HttpConfiguration();

            _claimIdentity = new ClaimsIdentity();
            _claimIdentity.AddClaim(new Claim("fakeClaim", "fake"));
            _claimIdentity.AddClaim(new Claim(Constants.LssClaims.Type, "view.widget"));
            _userController.User = new ClaimsPrincipal(_claimIdentity);
        }

        [Fact]
        public void GetUsersWithRoles_Should_Have_HttpGetAttribute()
        {
            _userController.AssertAttribute<UserController, HttpGetAttribute>(_ => _.GetUsersWithRoles());
        }

        [Fact]
        public void GetUsersWithRoles_Should_Have_RouteAttribute()
        {
            _userController.AssertRoute(_ => _.GetUsersWithRoles(), "user/roles");
        }

        [Fact]
        public void GetUsers_Should_Have_RouteAttribute()
        {
            _userController.AssertRoute(_ => _.GetUsers(), "user");
        }

        [Fact]
        public async void GetUsersWithRoles_Should_Call_UserService()
        {
            var users = Fixture.CreateMany<UserWithRolesModel>();

            _mockUserService.Setup(_ => _.GetUsersWithRoles())
                .Returns(users);

            var result = _userController.GetUsersWithRoles();

            var message =await result.ExecuteAsync(new System.Threading.CancellationToken());
            message.StatusCode.Should().Be(HttpStatusCode.OK);

            IEnumerable<UserWithRolesModel> models;
            message.TryGetContentValue(out models).Should().BeTrue();
            models.Should().BeEquivalentTo(users);
        }

        [Fact]
        public void GetUsers_Should_Have_ClaimAuthorizeAttribute()
        {
            _userController.AssertClaim(_ => _.GetUsers(), 
                LssClaims.ListUsers);

        }

        [Fact]
        public void GetUsersWithRoles_Should_Have_ClaimAuthorizeAttribute()
        {
            _userController.AssertClaim(_ => _.GetUsersWithRoles(),
                LssClaims.ListUsers);

        }

        [Fact]
        public void GetClaims_Should_Have_ClaimAuthorizeAttribute()
        {
            _userController.AssertClaim(_ => _.GetClaims(), LssClaims.ListUserClaims);
        }

        [Fact]
        public void AssignRole_Should_Have_ClaimAuthorizeAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _userController.AssertClaim(_ => _.AssignRole(new UserRoleModel()), LssClaims.AssignRole);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

        }




        [Fact]
        public async void GetClaims_Should_Return_Lss_Claims_From_Identity()
        {
            var result = _userController.GetClaims();

            var message = await result.ExecuteAsync(new System.Threading.CancellationToken());

            message.StatusCode.Should().Be(HttpStatusCode.OK);

            IEnumerable<ClaimModel> models;
            message.TryGetContentValue(out models).Should().BeTrue();

            models.Should().HaveCount(1);
            var claim = models.First();
            claim.ClaimType.Should().Be(Constants.LssClaims.Type);
            claim.ClaimValue.Should().Be("view.widget");

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
        public async void Assign_Should_Call_User_Service_And_Return_Ok_When_Successful()
        {
            //arrange
            var model = Fixture.Create<UserRoleModel>();
            _mockUserService.Setup(_ => _.AssignUserRoleAsync(model.Role, model.User))
                .ReturnsAsync(IdentityResult.Success);

            //act
            var result = await _userController.AssignRole(model);

            

            //assert
            Mock.VerifyAll();

            var message = await result.ExecuteAsync(new System.Threading.CancellationToken());

            message.StatusCode.Should().Be(HttpStatusCode.OK);
            
        }

        [Fact]
        public async void Assign_Should_Call_User_Service_And_Return_BadRequest_When_Role_Assignment_Fails()
        {
            //arrange
            var model = Fixture.Create<UserRoleModel>();

            _mockUserService.Setup(_ => _.AssignUserRoleAsync(model.Role, model.User))
                .ReturnsAsync(new IdentityResult());


            //act
            var result = await _userController.AssignRole(model);


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
            ReflectionHelper.GetMethod<UserController>(_ => _.AssignRole(new UserRoleModel()))
                .Should().BeDecoratedWith<HttpPostAttribute>();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

        }

        [Fact]
        public void AssignRole_Should_Be_Decorated_With_RouteAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            ReflectionHelper.GetMethod<UserController>(_ => _.AssignRole(new UserRoleModel()))
                .Should().BeDecoratedWith<RouteAttribute>(_ => _.Template == "user/assign");
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
            ReflectionHelper.GetMethod<UserController>(_ => _.GetClaims())
                .Should()
                .BeDecoratedWith<HttpGetAttribute>();
        }

        [Fact]
        public void GetClaims_Should_Have_RouteAttribute()
        {
            ReflectionHelper.GetMethod<UserController>(_ => _.GetClaims())
              .Should()
              .BeDecoratedWith<RouteAttribute>(_=>_.Template == "user/claims");
        }

        [Fact]
        public void RemoveRole_Should_Have_HttpPostAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _userController.AssertAttribute<UserController, HttpPostAttribute>(_ => _.RemoveRole(new UserRoleModel()));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void RemoveRole_Should_Have_RouteAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _userController.AssertRoute(_ => _.RemoveRole(new UserRoleModel()), "user/revoke");
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]

        public void RevokeRole_Should_Have_ClaimAuthorizeAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _userController.AssertClaim(_ => _.RemoveRole(new UserRoleModel()), LssClaims.RevokeRole);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }
        [Fact]
        public async void RemoveRole_Should_Call_UserService()
        {
            var model = Fixture.Create<UserRoleModel>();

            _mockUserService.Setup(_ => _.RemoveUserFromRoleAsync(model.Role, model.User))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _userController.RemoveRole(model);

            var message = await result.ExecuteAsync(new System.Threading.CancellationToken());

            message.StatusCode.Should().Be(HttpStatusCode.OK);

        }
    }
}
