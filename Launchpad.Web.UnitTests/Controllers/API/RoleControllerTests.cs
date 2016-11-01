using System;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Moq;
using Ploeh.AutoFixture;
using Launchpad.UnitTests.Common;
using Launchpad.Web.Controllers.API;
using System.Web.Http;
using Launchpad.Services.Interfaces;
using Launchpad.Models;
using Microsoft.AspNet.Identity;
using System.Threading;
using System.Net;
using System.Collections.Generic;
using System.Net.Http;
using static Launchpad.Web.Constants;


namespace Launchpad.Web.UnitTests.Controllers.API
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

        [Fact]
        public void GetRoles_Should_Have_ClaimAuthorizeAttribute()
        {
            _roleController.AssertClaim(_ => _.GetRoles(), LssClaims.ListRoles);
        }

        [Fact]
        public void CreateRole_Should_Have_ClaimAuthorizeAttribute()
        {
            _roleController.AssertClaim(_=>_.CreateRole(new RoleModel()), LssClaims.CreateRole);
        }

        [Fact]
        public void AddClaim_Should_Have_ClaimAuthorizeAttribute()
        {
            _roleController.AssertClaim(_ => _.AddClaim(new RoleClaimModel()), LssClaims.CreateClaim);
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
                .BeDecoratedWith<RoutePrefixAttribute>(value => value.Prefix == Constants.RoutePrefixes.Role);
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

            ReflectionHelper.GetMethod<RoleController>(_ => _.AddClaim(new RoleClaimModel()))
                .Should().BeDecoratedWith<HttpPostAttribute>();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Fact]
        public void AddClaim_Should_Have_Known_Route()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            ReflectionHelper.GetMethod<RoleController>(_ => _.AddClaim(new RoleClaimModel()))
               .Should().BeDecoratedWith<RouteAttribute>(value => value.Template == "role/claim");
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
            var roleClaim = new RoleClaimModel
            {
                Role = role,
                Claim = claim
            };

            _mockRoleService.Setup(_ => _.AddClaimAsync(role, claim))
                .Returns(Task.Delay(0));

            var result  = await _roleController.AddClaim(roleClaim);

            var message = await result.ExecuteAsync(new CancellationToken());

            message.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}
