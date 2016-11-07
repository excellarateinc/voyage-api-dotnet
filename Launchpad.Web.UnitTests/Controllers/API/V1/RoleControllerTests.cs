using System;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Moq;
using Ploeh.AutoFixture;
using Launchpad.UnitTests.Common;
using Launchpad.Web.Controllers.API.V1;
using System.Web.Http;
using Launchpad.Services.Interfaces;
using Launchpad.Models;
using Microsoft.AspNet.Identity;
using System.Threading;
using System.Net;
using System.Collections.Generic;
using System.Net.Http;
using static Launchpad.Web.Constants;


namespace Launchpad.Web.UnitTests.Controllers.API.V1
{
    public class RoleControllerTests : BaseUnitTest
    {
        private RoleController _roleController;
        private Mock<IRoleService> _mockRoleService;

        public RoleControllerTests()
        {
            _mockRoleService = Mock.Create<IRoleService>();
            _roleController = new RoleController(_mockRoleService.Object);
            _roleController.Request = new System.Net.Http.HttpRequestMessage();
            _roleController.Configuration = new HttpConfiguration();
        }

        public void CreateRole_Should_Have_RouteAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _roleController.AssertRoute(_=>_.CreateRole(new RoleModel()), "role");
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        public void RemoveRole_Should_Have_RouteAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _roleController.AssertRoute(_ => _.RemoveRole("roleId"), "role");
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void RemoveClaim_Should_Have_ClaimAuthorizeAttribute()
        {
            _roleController.AssertClaim(_ => _.RemoveClaim("a", 1), LssClaims.DeleteRoleClaim);
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
            _roleController.AssertClaim(_ => _.RemoveRole("id"), LssClaims.DeleteRole);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void GetRoles_Should_Have_ClaimAuthorizeAttribute()
        {
            _roleController.AssertClaim(_ => _.GetRoles(), LssClaims.ListRoles);
        }

        [Fact]
        public void CreateRole_Should_Have_ClaimAuthorizeAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _roleController.AssertClaim(_=>_.CreateRole(new RoleModel()), LssClaims.CreateRole);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void AddClaim_Should_Have_ClaimAuthorizeAttribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _roleController.AssertClaim(_ => _.AddClaim("id", new ClaimModel()), LssClaims.CreateClaim);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public async void CreateRole_Should_Call_RoleService_And_Return_Created()
        {
            var model = new RoleModel
            {
                Name = "Great Role"
            };

            _mockRoleService.Setup(_ => _.CreateRoleAsync(model))
                .ReturnsAsync(IdentityResult.Success);

            //ACT
            var result = await _roleController.CreateRole(model);

            var message = await result.ExecuteAsync(new CancellationToken());

            message.StatusCode.Should().Be(HttpStatusCode.Created);
        }


        [Fact]
        public async void CreateRole_Should_Call_RoleService_And_Return_BadRequest_On_Failure()
        {
            var model = new RoleModel
            {
                Name = "Great Role"
            };

            _mockRoleService.Setup(_ => _.CreateRoleAsync(model))
                .ReturnsAsync(new IdentityResult());

            //ACT
            var result = await _roleController.CreateRole(model);

            var message = await result.ExecuteAsync(new CancellationToken());

            message.StatusCode.Should().Be(HttpStatusCode.BadRequest);
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
        public void Class_Should_Have_Authorize_Attribute()
        {
            typeof(RoleController)
                .Should()
                .BeDecoratedWith<AuthorizeAttribute>();
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
            ReflectionHelper.GetMethod<RoleController>(_ => _.CreateRole(new Models.RoleModel()))
                .Should().BeDecoratedWith<HttpPostAttribute>();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
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
        public void GetRoles_Should_Have_HttpGetAttribute() {
            ReflectionHelper.GetMethod<RoleController>(_ => _.GetRoles())
                .Should()
                .BeDecoratedWith<HttpGetAttribute>();
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
            var role = Fixture.Create<RoleModel>();
            var claim = Fixture.Create<ClaimModel>();
            
            _mockRoleService.Setup(_ => _.AddClaimAsync(role.Id, claim))
                .Returns(Task.Delay(0));

            var result  = await _roleController.AddClaim(role.Id, claim);

            var message = await result.ExecuteAsync(new CancellationToken());

            message.StatusCode.Should().Be(HttpStatusCode.Created);
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
            //Arrange
            var roleId = Fixture.Create<string>();
            var claimId = Fixture.Create<int>();

            _mockRoleService.Setup(_ => _.RemoveClaim(roleId, claimId));

            //Act
            var result = _roleController.RemoveClaim(roleId, claimId);


            //Assert
            var message = await result.ExecuteAsync(new CancellationToken());
            message.StatusCode.Should().Be(HttpStatusCode.NoContent);
            Mock.VerifyAll();
        }

        [Fact]
        public async void RemoveRole_Should_Call_RoleService()
        {
            //Arrange
            var roleId = Fixture.Create<string>();

            _mockRoleService.Setup(_ => _.RemoveRoleAsync(roleId))
                .ReturnsAsync(IdentityResult.Success);

            //Act
            var result = await _roleController.RemoveRole(roleId);

            //Assert
            var message = await result.ExecuteAsync(new CancellationToken());
            message.StatusCode.Should().Be(HttpStatusCode.NoContent);
            Mock.VerifyAll();
        }
    }
}
