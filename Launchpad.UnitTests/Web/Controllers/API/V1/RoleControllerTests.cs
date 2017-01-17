using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Routing;
using FluentAssertions;
using Launchpad.Core.Exceptions;
using Launchpad.Models;
using Launchpad.Services.Role;
using Launchpad.UnitTests.Common;
using Launchpad.Web.Controllers.API.V1;
using Microsoft.AspNet.Identity;
using Moq;
using Ploeh.AutoFixture;
using Xunit;
using Constants = Launchpad.Web.Constants;

namespace Launchpad.UnitTests.Web.Controllers.API.V1
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
        public void GetClaims_Should_Have_RouteAttribute()
        {
            _roleController.AssertRoute(_ => _.GetClaims("id"), "roles/{roleId}/claims");
        }

        [Fact]
        public void GetClaims_Should_Have_HttpGetAttribute()
        {
            _roleController.AssertAttribute<RoleController, HttpGetAttribute>(_ => _.GetClaims("Id"));
        }

        [Fact]
        public void GetClaims_Should_Have_ClaimAuthorizeAttribute()
        {
            _roleController.AssertClaim(_ => _.GetClaims("id"), Constants.LssClaims.ListRoleClaims);
        }

        [Fact]
        public async void GetClaims_Should_Call_RoleService_And_Return_Ok_On_Success()
        {
            var id = Fixture.Create<string>();
            var claims = Fixture.CreateMany<ClaimModel>();

            _mockRoleService.Setup(_ => _.GetRoleClaimsByRoleId(id))
                .Returns(claims);

            var result = _roleController.GetClaims(id);

            Mock.VerifyAll();
            var message = await result.ExecuteAsync(CreateCancelToken());
            message.StatusCode.Should().Be(HttpStatusCode.OK);

            IEnumerable<ClaimModel> messageModels;
            message.TryGetContentValue(out messageModels).Should().BeTrue("Message is readable");
            messageModels.ShouldBeEquivalentTo(claims);
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
        public void RemoveClaim_Should_Have_ClaimAuthorizeAttribute()
        {
            _roleController.AssertClaim(_ => _.RemoveClaim("a", 1), Constants.LssClaims.DeleteRoleClaim);
        }

        [Fact]
        public void GetRoles_Should_Have_RouteAttribute()
        {
            _roleController.AssertRoute(_ => _.GetRoles(), "roles");
        }

        [Fact]
        public void RemoveRole_Should_Have_ClaimAuthorizeAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _roleController.AssertClaim(_ => _.RemoveRole("id"), Constants.LssClaims.DeleteRole);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void GetRoles_Should_Have_ClaimAuthorizeAttribute()
        {
            _roleController.AssertClaim(_ => _.GetRoles(), Constants.LssClaims.ListRoles);
        }

        [Fact]
        public void CreateRole_Should_Have_ClaimAuthorizeAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _roleController.AssertClaim(_ => _.CreateRole(new RoleModel()), Constants.LssClaims.CreateRole);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void AddClaim_Should_Have_ClaimAuthorizeAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _roleController.AssertClaim(_ => _.AddClaim("id", new ClaimModel()), Constants.LssClaims.CreateClaim);
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
        public void GetRoleById_Should_Have_ClaimAuthorizeAttribute()
        {
            _roleController.AssertClaim(_ => _.GetRoleById("id"), Constants.LssClaims.ViewRole);
        }

        [Fact]
        public void AddClaim_Should_Have_HttpPost_Attribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            ReflectionHelper.GetMethod<RoleController>(_ => _.AddClaim("id", new ClaimModel()))
                .Should().BeDecoratedWith<HttpPostAttribute>();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void AddClaim_Should_Have_Known_Route()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            ReflectionHelper.GetMethod<RoleController>(_ => _.AddClaim("id", new ClaimModel()))
               .Should().BeDecoratedWith<RouteAttribute>(value => value.Template == "roles/{roleId}/claims");
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
        public void GetClaimById_Should_Have_HttpGetAttribute()
        {
            _roleController.AssertAttribute<RoleController, HttpGetAttribute>(_ => _.GetClaimById("roleId", 0));
        }

        [Fact]
        public void GetClaimById_Should_Have_RouteAttribute()
        {
            _roleController.AssertRoute(_ => _.GetClaimById("roleId", 0), "roles/{roleId}/claims/{claimId}");
        }

        [Fact]
        public void GetClaimById_Should_Have_ClaimAuthorizeAttribute()
        {
            _roleController.AssertClaim(_ => _.GetClaimById("roleId", 0), Constants.LssClaims.ViewClaim);
        }

        [Fact]
        public async void GetClaimById_Should_Call_RoleService_And_Return_Ok()
        {
            // ARRANGE
            var roleId = Fixture.Create<string>();
            var claimId = Fixture.Create<int>();
            var claim = Fixture.Create<ClaimModel>();

            _mockRoleService
                .Setup(_ => _.GetClaimById(roleId, claimId))
                .Returns(claim);

            // ACT
            var result = _roleController.GetClaimById(roleId, claimId);

            // ASSERT
            Mock.VerifyAll();

            var message = await result.ExecuteAsync(new CancellationToken());
            message.StatusCode.Should().Be(HttpStatusCode.OK);

            ClaimModel resultModel;
            message.TryGetContentValue(out resultModel).Should().BeTrue();

            resultModel.ShouldBeEquivalentTo(claim);
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
        public async void AddClaim_Should_Call_RoleService_And_Return_Created()
        {
            // Arrange
            var role = Fixture.Create<RoleModel>();
            var claim = Fixture.Create<ClaimModel>();
            var serviceResult = Fixture.Create<ClaimModel>();
            _mockRoleService.Setup(_ => _.AddClaimAsync(role.Id, claim))
                .ReturnsAsync(serviceResult);

            const string link = "http://fakelink.com";

            // Matcher for determining if route params match
            Func<Dictionary<string, object>, bool> routeDictionaryMatcher = routeDictionary =>
            {
                routeDictionary.ContainsKey("RoleId").Should().BeTrue();
                routeDictionary["RoleId"].ToString().Should().Be(role.Id);
                routeDictionary.ContainsKey("ClaimId").Should().BeTrue();
                routeDictionary["ClaimId"].As<int>().Should().Be(serviceResult.Id);
                return true;
            };

            _mockUrlHelper.Setup(_ => _.Link(
                "GetClaimById",
                It.Is<Dictionary<string, object>>(args => routeDictionaryMatcher(args))))
                .Returns(link);

            // Act
            var result = await _roleController.AddClaim(role.Id, claim);

            // Assert
            var message = await result.ExecuteAsync(new CancellationToken());

            message.StatusCode.Should().Be(HttpStatusCode.Created);
            message.Headers.Location.Should().Be(link);

            ClaimModel messageModel;
            message.TryGetContentValue(out messageModel).Should().BeTrue();

            messageModel.ShouldBeEquivalentTo(serviceResult);
        }

        [Fact]
        public void RemoveClaim_Should_Have_HttpDeleteAttribute()
        {
            _roleController.AssertAttribute<RoleController, HttpDeleteAttribute>(_ => _.RemoveClaim("roleId", 1));
        }

        [Fact]
        public void RemoveRole_Should_Have_HttpDeleteAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _roleController.AssertAttribute<RoleController, HttpDeleteAttribute>(_ => _.RemoveRole("roleId"));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void RemoveClaim_Should_Have_RouteAttribute()
        {
            _roleController.AssertRoute(_ => _.RemoveClaim("roleId", 1), "roles/{roleId}/claims/{claimId}");
        }

        [Fact]
        public async void RemoveClaim_Should_Call_RoleService()
        {
            // Arrange
            var roleId = Fixture.Create<string>();
            var claimId = Fixture.Create<int>();

            _mockRoleService.Setup(_ => _.RemoveClaim(roleId, claimId));

            // Act
            var result = _roleController.RemoveClaim(roleId, claimId);

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
            message.StatusCode.Should().Be(HttpStatusCode.OK);
            Mock.VerifyAll();
        }
    }
}
