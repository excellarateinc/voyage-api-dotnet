using Launchpad.Services.Interfaces;
using Launchpad.UnitTests.Common;
using Launchpad.Web.Controllers.API;
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

namespace Launchpad.Web.UnitTests.Controllers.API
{
    public class UserControllerTests : BaseUnitTest
    {
        private UserController _userController;
        private Mock<IUserService> _mockUserService;

        public UserControllerTests()
        {
            _mockUserService = Mock.Create<IUserService>();
            _userController = new UserController(_mockUserService.Object);
            _userController.Request = new System.Net.Http.HttpRequestMessage();
            _userController.Configuration = new System.Web.Http.HttpConfiguration();
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


            _mockUserService.Setup(_ => _.ConfigureUserClaims(model.User));


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
            ReflectionHelper.GetMethod<UserController>(_ => _.AssignRole(new UserRoleModel()))
                .Should().BeDecoratedWith<HttpPostAttribute>();
        }

        [Fact]
        public void AssignRole_Should_Be_Decorated_With_RouteAttribute()
        {
            ReflectionHelper.GetMethod<UserController>(_ => _.AssignRole(new UserRoleModel()))
                .Should().BeDecoratedWith<RouteAttribute>(_ => _.Template == "user/assign");
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
                _ => _.Prefix.Equals(Constants.RoutePrefixes.User));
        }
    }
}
