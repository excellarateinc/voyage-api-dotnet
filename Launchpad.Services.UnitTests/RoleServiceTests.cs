using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Ploeh.AutoFixture;
using Launchpad.UnitTests.Common;
using Moq;
using Launchpad.Services.IdentityManagers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Launchpad.Models;
using Launchpad.Models.EntityFramework;
using Launchpad.Data.Interfaces;
using Launchpad.Services.Fixture;

namespace Launchpad.Services.UnitTests
{
    [Collection(AutoMapperCollection.CollectionName)]
    public class RoleServiceTests : BaseUnitTest
    {
        private RoleService _roleService;
        private ApplicationRoleManager _roleManager;
        private Mock<IRoleClaimRepository> _mockRepository;
        private Mock<IRoleStore<ApplicationRole>> _mockRoleStore;
        private AutoMapperFixture _mapperFixture;

        public RoleServiceTests(AutoMapperFixture mapperFixture)
        {
            _mockRoleStore = Mock.Create<IRoleStore<ApplicationRole>>();
            _mockRoleStore.As<IQueryableRoleStore<ApplicationRole>>();

            _mockRepository = Mock.Create<IRoleClaimRepository>();

            _roleManager = new ApplicationRoleManager(_mockRoleStore.Object);

            _mapperFixture = mapperFixture;

            _roleService = new RoleService(_roleManager, _mockRepository.Object, _mapperFixture.MapperInstance);
            
        }


        [Fact]
        public void GetRoleClaims_Should_Call_Repository()
        {
            var roleName = "admin";
            var claims = new[] { new RoleClaim { ClaimType = "type", ClaimValue = "value" } };

            _mockRepository.Setup(_ => _.GetClaimsByRole(roleName))
                .Returns(claims.AsQueryable());

            var results = _roleService.GetRoleClaims(roleName);

            Mock.VerifyAll();

            results.ShouldBeEquivalentTo(claims);
        }

        [Fact]
        public async void AddClaim_Should_Call_Manager_And_Repository()
        {
            var model = Fixture.Create<RoleModel>();

            var appRole = new ApplicationRole();
            appRole.Id = "abc";

            var claim = Fixture.Create<ClaimModel>();

            _mockRoleStore.Setup(_ => _.FindByNameAsync(model.Name))
              .ReturnsAsync(appRole);

            _mockRepository.Setup(_ => _.Add(It.Is<RoleClaim>(
                value => value.RoleId == appRole.Id &&
                  value.ClaimType == claim.ClaimType &&
                  value.ClaimValue == claim.ClaimValue
            )))
            .Returns(new RoleClaim());

            await _roleService.AddClaimAsync(model, claim);

            Mock.VerifyAll();
        }

        [Fact]
        public async void AddClaim_Should_Call_Manager_And_Not_Repository_When_Role_Does_Not_Exist()
        {
            var model = Fixture.Create<RoleModel>();
            
            var claim = Fixture.Create<ClaimModel>();
         
            _mockRoleStore.Setup(_ => _.FindByNameAsync(model.Name))
              .ReturnsAsync(null);
          

            await _roleService.AddClaimAsync(model, claim);

            Mock.VerifyAll();
        }


        [Fact]
        public async void CreateRoleAsync_Should_Call_RoleManager()
        {
            var model = Fixture.Create<RoleModel>();

            _mockRoleStore.Setup(_ => _.FindByNameAsync(model.Name))
                .ReturnsAsync(null);

            _mockRoleStore.Setup(_ => _.CreateAsync(It.Is<ApplicationRole>(value => value.Name == model.Name)))
                .Returns(Task.FromResult(0));

            var result = await _roleService.CreateRoleAsync(model);

            Mock.VerifyAll();
            result.Should().NotBeNull();
        }

        [Fact]
        public void Ctor_Should_Throw_Null_Argument_Exception_When_RoleManager_Is_Null()
        {
            Action throwAction = () => new RoleService(null, null, null);

            throwAction.ShouldThrow<ArgumentNullException>()
                .And
                .ParamName.Should().Be("roleManager");
        }

        [Fact]
        public void Ctor_Should_Throw_Null_Argument_Exception_When_RoleClaimRepository_Is_Null()
        {
            Action throwAction = () => new RoleService(_roleManager, null, null);

            throwAction.ShouldThrow<ArgumentNullException>()
                .And
                .ParamName.Should().Be("roleClaimRepository");
        }

        [Fact]
        public void Ctor_Should_Throw_Null_Argument_Exception_when_Mapper_Is_Null()
        {
            Action throwAction = () => new RoleService(_roleManager, _mockRepository.Object, null);

            throwAction.ShouldThrow<ArgumentNullException>()
                .And
                .ParamName.Should().Be("mapper");
        }

        [Fact]
        public void GetRoles_Should_Return_All_Roles()
        {
            var roles = new ApplicationRole[] {
                    new ApplicationRole() {Name="Role1" },
                    new ApplicationRole() {Name="Role2" }
                };


            _mockRoleStore.As<IQueryableRoleStore<ApplicationRole>>()
                .Setup(_ => _.Roles)
                .Returns(roles.AsQueryable());        

            var result = _roleService.GetRoles();

            Mock.Verify();
            result.Should().NotBeNull();
            result.Should().HaveCount(roles.Length);

            roles.All(_ => result.Any(r => r.Name == _.Name))
                .Should()
                .BeTrue();


        }
    }
}
