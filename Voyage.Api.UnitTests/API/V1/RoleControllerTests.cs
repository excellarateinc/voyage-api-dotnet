using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Routing;
using FluentAssertions;
using Microsoft.AspNet.Identity;
using Moq;
using Ploeh.AutoFixture;
using Voyage.Api.API.V1;
using Voyage.Api.UnitTests.Common;
using Voyage.Core.Exceptions;
using Voyage.Models;
using Voyage.Services.Role;
using Xunit;

namespace Voyage.Api.UnitTests.API.V1
{
    [Trait("Category", "Role.Controller")]
    public class RoleControllerTests : BaseUnitTest
    {
        private readonly RoleController _roleController;
        private readonly Mock<IRoleService> _mockRoleService;
        private readonly Mock<UrlHelper> _mockUrlHelper;

        public RoleControllerTests()
        {
            _mockRoleService = Mock.Create<IRoleService>();
            _roleController = new RoleController(_mockRoleService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            _mockUrlHelper = Mock.Create<UrlHelper>();
            _roleController.Url = _mockUrlHelper.Object;
        }

        [Fact]
        public void GetPermissions_Should_Have_RouteAttribute()
        {
            _roleController.AssertRoute(_ => _.GetPermissions("id"), "roles/{roleId}/Permissions");
        }

        [Fact]
        public void GetPermissions_Should_Have_HttpGetAttribute()
        {
            _roleController.AssertAttribute<RoleController, HttpGetAttribute>(_ => _.GetPermissions("Id"));
        }

        [Fact]
        public void GetPermissions_Should_Have_PermissionAuthorizeAttribute()
        {
            _roleController.AssertPermission(_ => _.GetPermissions("id"), Constants.AppPermissions.ListRolePermissions);
        }

        [Fact]
        public async void GetPermissions_Should_Call_RoleService_And_Return_Ok_On_Success()
        {
            var id = Fixture.Create<string>();
            var Permissions = Fixture.CreateMany<PermissionModel>();

            _mockRoleService.Setup(_ => _.GetRolePermissionsByRoleId(id))
                .Returns(Permissions);

            var result = _roleController.GetPermissions(id);

            Mock.VerifyAll();
            var message = await result.ExecuteAsync(CreateCancelToken());
            message.StatusCode.Should().Be(HttpStatusCode.OK);

            IEnumerable<PermissionModel> messageModels;
            message.TryGetContentValue(out messageModels).Should().BeTrue("Message is readable");
            messageModels.ShouldBeEquivalentTo(Permissions);
        }

        [Fact]

        public void CreateRole_Should_Have_RouteAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _roleController.AssertRoute(_ => _.CreateRole(new RoleModel()), "roles");
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void RemoveRole_Should_Have_RouteAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _roleController.AssertRoute(_ => _.RemoveRole("roleId"), "roles/{roleId}");
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void RemovePermission_Should_Have_PermissionAuthorizeAttribute()
        {
            _roleController.AssertPermission(_ => _.RemovePermission("a", 1), Constants.AppPermissions.DeleteRolePermission);
        }

        [Fact]
        public void GetRoles_Should_Have_RouteAttribute()
        {
            _roleController.AssertRoute(_ => _.GetRoles(), "roles");
        }

        [Fact]
        public void RemoveRole_Should_Have_PermissionAuthorizeAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _roleController.AssertPermission(_ => _.RemoveRole("id"), Constants.AppPermissions.DeleteRole);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void GetRoles_Should_Have_PermissionAuthorizeAttribute()
        {
            _roleController.AssertPermission(_ => _.GetRoles(), Constants.AppPermissions.ListRoles);
        }

        [Fact]
        public void CreateRole_Should_Have_PermissionAuthorizeAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _roleController.AssertPermission(_ => _.CreateRole(new RoleModel()), Constants.AppPermissions.CreateRole);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void AddPermission_Should_Have_PermissionAuthorizeAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _roleController.AssertPermission(_ => _.AddPermission("id", new PermissionModel()), Constants.AppPermissions.CreatePermission);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public async void CreateRole_Should_Call_RoleService_And_Return_Created()
        {
            // Arrange
            const string url = "http://thisisalink.com/";

            // Create input model
            var model = new RoleModel
            {
                Name = "Great Role",
                Id = Guid.NewGuid().ToString()
            };

            // Matcher for determining if route params match
            Func<Dictionary<string, object>, bool> routeDictionaryMatcher = routeDictionary =>
            {
                routeDictionary.ContainsKey("roleId").Should().BeTrue();
                routeDictionary["roleId"].ToString().Should().Be(model.Id);
                return true;
            };
            _mockUrlHelper.Setup(_ => _.Link("GetRoleById", It.Is<Dictionary<string, object>>(arg => routeDictionaryMatcher(arg))))
                .Returns(url);

            _mockRoleService.Setup(_ => _.CreateRoleAsync(model))
                .ReturnsAsync(model);

            // ACT
            var result = await _roleController.CreateRole(model);

            // ASSERT
            var message = await result.ExecuteAsync(new CancellationToken());

            // should have the location header
            message.Headers.Location.Should().Be(url);

            // should have created status code
            message.StatusCode.Should().Be(HttpStatusCode.Created);

            // should have model returned
            RoleModel contentModel;
            message.TryGetContentValue(out contentModel).Should().BeTrue();
            contentModel.Should().NotBeNull();
            contentModel.Name.Should().Be(model.Name);
        }

        [Fact]
        public async void GetRoleById_Should_Call_Role_Service()
        {
            var id = Fixture.Create<string>();
            var model = Fixture.Create<RoleModel>();

            _mockRoleService.Setup(_ => _.GetRoleById(id))
                .Returns(model);

            var result = _roleController.GetRoleById(id);

            var message = await result.ExecuteAsync(new CancellationToken());

            message.StatusCode.Should().Be(HttpStatusCode.OK);

            RoleModel role;
            message.TryGetContentValue(out role).Should().BeTrue();

            role.ShouldBeEquivalentTo(model);
        }

        [Fact]
        public void CreateRole_Should_Call_RoleService_And_Return_BadRequest_On_Failure()
        {
            var model = new RoleModel
            {
                Name = "Great Role"
            };

            _mockRoleService.Setup(_ => _.CreateRoleAsync(model)).Throws<BadRequestException>();

            // ASSERT
            Assert.ThrowsAsync<BadRequestException>(async () => await _roleController.CreateRole(model));
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_RoleService_Is_Null()
        {
            Action throwAction = () => new RoleController(null);

            throwAction.ShouldThrow<ArgumentNullException>()
                .And
                .ParamName
                .Should()
                .Be("roleService");
        }

        [Fact]
        public void Class_Should_Have_RoutePrefix_Attribute()
        {
            typeof(RoleController)
                .Should()
                .BeDecoratedWith<RoutePrefixAttribute>(value => value.Prefix == Constants.RoutePrefixes.V1);
        }

        [Fact]
        public void CreateRole_Should_Have_HttpPost_Attribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            ReflectionHelper.GetMethod<RoleController>(_ => _.CreateRole(new RoleModel()))
                .Should().BeDecoratedWith<HttpPostAttribute>();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void GetRoleById_Should_Have_HttpGetAttribute()
        {
            ReflectionHelper.GetMethod<RoleController>(_ => _.GetRoleById("id"))
                .Should()
                .BeDecoratedWith<HttpGetAttribute>();
        }

        [Fact]
        public void GetRoleById_Should_Have_RouteAttribute()
        {
            _roleController.AssertRoute(_ => _.GetRoleById("id"), "roles/{roleId}");
        }

        [Fact]
        public void GetRoleById_Should_Have_PermissionAuthorizeAttribute()
        {
            _roleController.AssertPermission(_ => _.GetRoleById("id"), Constants.AppPermissions.ViewRole);
        }

        [Fact]
        public void AddPermission_Should_Have_HttpPost_Attribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            ReflectionHelper.GetMethod<RoleController>(_ => _.AddPermission("id", new PermissionModel()))
                .Should().BeDecoratedWith<HttpPostAttribute>();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void AddPermission_Should_Have_Known_Route()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            ReflectionHelper.GetMethod<RoleController>(_ => _.AddPermission("id", new PermissionModel()))
               .Should().BeDecoratedWith<RouteAttribute>(value => value.Template == "roles/{roleId}/Permissions");
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void GetRoles_Should_Have_HttpGetAttribute()
        {
            ReflectionHelper.GetMethod<RoleController>(_ => _.GetRoles())
                .Should()
                .BeDecoratedWith<HttpGetAttribute>();
        }

        [Fact]
        public void GetPermissionById_Should_Have_HttpGetAttribute()
        {
            _roleController.AssertAttribute<RoleController, HttpGetAttribute>(_ => _.GetPermissionById("roleId", 0));
        }

        [Fact]
        public void GetPermissionById_Should_Have_RouteAttribute()
        {
            _roleController.AssertRoute(_ => _.GetPermissionById("roleId", 0), "roles/{roleId}/Permissions/{PermissionId}");
        }

        [Fact]
        public void GetPermissionById_Should_Have_PermissionAuthorizeAttribute()
        {
            _roleController.AssertPermission(_ => _.GetPermissionById("roleId", 0), Constants.AppPermissions.ViewPermission);
        }

        [Fact]
        public async void GetPermissionById_Should_Call_RoleService_And_Return_Ok()
        {
            // ARRANGE
            var roleId = Fixture.Create<string>();
            var PermissionId = Fixture.Create<int>();
            var Permission = Fixture.Create<PermissionModel>();

            _mockRoleService
                .Setup(_ => _.GetPermissionById(roleId, PermissionId))
                .Returns(Permission);

            // ACT
            var result = _roleController.GetPermissionById(roleId, PermissionId);

            // ASSERT
            Mock.VerifyAll();

            var message = await result.ExecuteAsync(new CancellationToken());
            message.StatusCode.Should().Be(HttpStatusCode.OK);

            PermissionModel resultModel;
            message.TryGetContentValue(out resultModel).Should().BeTrue();

            resultModel.ShouldBeEquivalentTo(Permission);
        }

        [Fact]
        public async void GetRoles_Should_Call_RoleService()
        {
            var roles = Fixture.CreateMany<RoleModel>();

            _mockRoleService.Setup(_ => _.GetRoles())
                .Returns(roles);

            var result = _roleController.GetRoles();

            var message = await result.ExecuteAsync(new CancellationToken());

            message.StatusCode.Should().Be(HttpStatusCode.OK);

            IEnumerable<RoleModel> models;
            message.TryGetContentValue(out models).Should().BeTrue();
            models.ShouldBeEquivalentTo(roles);

            Mock.VerifyAll();
        }

        [Fact]
        public async void AddPermission_Should_Call_RoleService_And_Return_Created()
        {
            // Arrange
            var role = Fixture.Create<RoleModel>();
            var Permission = Fixture.Create<PermissionModel>();
            var serviceResult = Fixture.Create<PermissionModel>();
            _mockRoleService.Setup(_ => _.AddPermissionAsync(role.Id, Permission))
                .ReturnsAsync(serviceResult);

            const string link = "http://fakelink.com";

            // Matcher for determining if route params match
            Func<Dictionary<string, object>, bool> routeDictionaryMatcher = routeDictionary =>
            {
                routeDictionary.ContainsKey("RoleId").Should().BeTrue();
                routeDictionary["RoleId"].ToString().Should().Be(role.Id);
                routeDictionary.ContainsKey("PermissionId").Should().BeTrue();
                routeDictionary["PermissionId"].As<int>().Should().Be(serviceResult.Id);
                return true;
            };

            _mockUrlHelper.Setup(_ => _.Link(
                "GetPermissionById",
                It.Is<Dictionary<string, object>>(args => routeDictionaryMatcher(args))))
                .Returns(link);

            // Act
            var result = await _roleController.AddPermission(role.Id, Permission);

            // Assert
            var message = await result.ExecuteAsync(new CancellationToken());

            message.StatusCode.Should().Be(HttpStatusCode.Created);
            message.Headers.Location.Should().Be(link);

            PermissionModel messageModel;
            message.TryGetContentValue(out messageModel).Should().BeTrue();

            messageModel.ShouldBeEquivalentTo(serviceResult);
        }

        [Fact]
        public void RemovePermission_Should_Have_HttpDeleteAttribute()
        {
            _roleController.AssertAttribute<RoleController, HttpDeleteAttribute>(_ => _.RemovePermission("roleId", 1));
        }

        [Fact]
        public void RemoveRole_Should_Have_HttpDeleteAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _roleController.AssertAttribute<RoleController, HttpDeleteAttribute>(_ => _.RemoveRole("roleId"));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void RemovePermission_Should_Have_RouteAttribute()
        {
            _roleController.AssertRoute(_ => _.RemovePermission("roleId", 1), "roles/{roleId}/Permissions/{PermissionId}");
        }

        [Fact]
        public async void RemovePermission_Should_Call_RoleService()
        {
            // Arrange
            var roleId = Fixture.Create<string>();
            var PermissionId = Fixture.Create<int>();

            _mockRoleService.Setup(_ => _.RemovePermission(roleId, PermissionId));

            // Act
            var result = _roleController.RemovePermission(roleId, PermissionId);

            // Assert
            var message = await result.ExecuteAsync(new CancellationToken());
            message.StatusCode.Should().Be(HttpStatusCode.OK);
            Mock.VerifyAll();
        }

        [Fact]
        public async void RemoveRole_Should_Call_RoleService()
        {
            // Arrange
            var roleId = Fixture.Create<string>();

            _mockRoleService.Setup(_ => _.RemoveRoleAsync(roleId))
                .ReturnsAsync(new IdentityResult());

            // Act
            var result = await _roleController.RemoveRole(roleId);

            // Assert
            var message = await result.ExecuteAsync(new CancellationToken());
            message.StatusCode.Should().Be(HttpStatusCode.NoContent);
            Mock.VerifyAll();
        }
    }
}
