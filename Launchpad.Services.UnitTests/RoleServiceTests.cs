using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Launchpad.Data.Repositories.RoleClaim;
using Launchpad.Models;
using Launchpad.Models.Entities;
using Launchpad.Services.IdentityManagers;
using Launchpad.Services.Role;
using Launchpad.Services.UnitTests.Fixture;
using Launchpad.UnitTests.Common;
using Microsoft.AspNet.Identity;
using Moq;
using Ploeh.AutoFixture;
using Xunit;

namespace Launchpad.Services.UnitTests
{
    [Trait("Category", "Role.Service")]
    [Collection(AutoMapperCollection.CollectionName)]
    public class RoleServiceTests : BaseUnitTest
    {
        private readonly RoleService _roleService;
        private readonly ApplicationRoleManager _roleManager;
        private readonly Mock<IRoleClaimRepository> _mockRepository;
        private readonly Mock<IRoleStore<ApplicationRole>> _mockRoleStore;
        private readonly AutoMapperFixture _mapperFixture;

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
        public void GetRoleClaimsByRoleId_Should_Call_Repostiory()
        {
            var targetId = Fixture.Create<string>();
            var roleClaim1 = new RoleClaim { RoleId = targetId, ClaimType = "target-type" };
            var roleClaim2 = new RoleClaim { RoleId = Fixture.Create<string>() };
            var repoResult = new[] { roleClaim1, roleClaim2 }.AsQueryable();

            _mockRepository.Setup(_ => _.GetAll())
                .Returns(repoResult);

            var result = _roleService.GetRoleClaimsByRoleId(targetId);

            result.Model.Should().NotBeNullOrEmpty().And.HaveCount(1);
            result.Model.First().ClaimType.Should().Be("target-type");
        }

        [Fact]
        public void GetClaimById_Should_Call_Repository()
        {
            var roleId = Fixture.Create<string>();
            int id = 1;
            RoleClaim repoClaim = new RoleClaim
            {
                ClaimType = Fixture.Create<string>(),
                ClaimValue = Fixture.Create<string>(),
                Id = Fixture.Create<int>()
            };

            _mockRepository.Setup(_ => _.Get(id))
                .Returns(repoClaim);

            var entityResult = _roleService.GetClaimById(roleId, id);

            Mock.VerifyAll();
            entityResult.Model.Id.Should().Be(repoClaim.Id);
            entityResult.Model.ClaimType.Should().Be(repoClaim.ClaimType);
            entityResult.Model.ClaimValue.Should().Be(repoClaim.ClaimValue);
        }

        [Fact]
        public void GetClaimById_Should_Return_Failed_EntityResult_When_Not_Found()
        {
            string roleId = Fixture.Create<string>();
            int id = 1;

            _mockRepository.Setup(_ => _.Get(id))
                .Returns((RoleClaim)null);

            var entityResult = _roleService.GetClaimById(roleId, id);

            Mock.VerifyAll();

            entityResult.Succeeded.Should().BeFalse();
            entityResult.IsEntityNotFound.Should().BeTrue();
        }

        [Fact]
        public void GetRoleById_Should_Call_Role_Manager()
        {
            var id = Fixture.Create<string>();

            var role = new ApplicationRole
            {
                Name = "Admin",
                Id = Guid.NewGuid().ToString()
            };

            _mockRoleStore.Setup(_ => _.FindByIdAsync(id))
                .ReturnsAsync(role);

            // act
            var entityResult = _roleService.GetRoleById(id);

            entityResult.Should().NotBeNull();
        }

        [Fact]
        public void GetRoleById_Should_Return_Failed_Result_When_Role_Not_Found()
        {
            // arrange
            var id = Fixture.Create<string>();

            _mockRoleStore.Setup(_ => _.FindByIdAsync(id))
               .ReturnsAsync(null);

            // act
            var entityResult = _roleService.GetRoleById(id);

            // assert
            entityResult.IsEntityNotFound.Should().BeTrue();
            entityResult.Succeeded.Should().BeFalse();
        }

        [Fact]
        public void GetRoleByName_Should_Call_Role_Manager()
        {
            var name = Fixture.Create<string>();

            var role = new ApplicationRole
            {
                Name = "Admin",
                Id = Guid.NewGuid().ToString()
            };

            _mockRoleStore.Setup(_ => _.FindByNameAsync(name))
                .ReturnsAsync(role);

            // act
            var result = _roleService.GetRoleByName(name);

            result.Should().NotBeNull();
            result.Model.Should().NotBeNull();
            result.Model.Name.Should().Be(role.Name);
        }

        [Fact]
        public void GetRoleByName_Should_Return_Failed_Result_When_Not_Found()
        {
            var name = Fixture.Create<string>();

            _mockRoleStore.Setup(_ => _.FindByNameAsync(name))
               .ReturnsAsync(null);

            var entityResult = _roleService.GetRoleByName(name);

            entityResult.Succeeded.Should().BeFalse();
            entityResult.IsEntityNotFound.Should().BeTrue();
        }

        [Fact]
        public async void RemoveRole_Calls_Role_Manager()
        {
            // Arrange
            var roleId = Fixture.Create<string>();

            var appRole = new ApplicationRole
            {
                Name = "New Role"
            };

            _mockRoleStore.Setup(_ => _.FindByIdAsync(roleId))
                .ReturnsAsync(appRole);
            _mockRoleStore.Setup(_ => _.DeleteAsync(appRole))
                .Returns(Task.Delay(0));

            // Act
            var result = await _roleService.RemoveRoleAsync(roleId);

            // Assert
            Mock.VerifyAll();
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public void RemoveClaim_Calls_Repository()
        {
            var roleId = Fixture.Create<string>();
            var claimId = Fixture.Create<int>();

            _mockRepository.Setup(_ => _.Delete(claimId));

            // act
            _roleService.RemoveClaim(roleId, claimId);

            Mock.VerifyAll();
        }

        [Fact]
        public async void RemoveRole_Calls_Role_Manager_And_Returns_Failed_Result_When_Role_Does_Not_Exist()
        {
            var roleId = Fixture.Create<string>();

            _mockRoleStore.Setup(_ => _.FindByIdAsync(roleId))
                .ReturnsAsync(null);

            var result = await _roleService.RemoveRoleAsync(roleId);

            Mock.VerifyAll();
            result.Succeeded.Should().BeFalse();
            result.IsEntityNotFound.Should().BeTrue();
        }

        [Fact]
        public void GetRoleClaims_Should_Call_Repository()
        {
            var roleName = "admin";
            var claims = new[] { new RoleClaim { ClaimType = "type", ClaimValue = "value" } };

            _mockRepository.Setup(_ => _.GetClaimsByRole(roleName))
                .Returns(claims.AsQueryable());

            var entityResult = _roleService.GetRoleClaims(roleName);

            Mock.VerifyAll();

            entityResult.Model.ShouldBeEquivalentTo(claims);
        }

        [Fact]
        public async void AddClaim_Should_Call_Manager_And_Repository()
        {
            var model = Fixture.Create<RoleModel>();

            var appRole = new ApplicationRole { Id = "abc" };

            var claim = Fixture.Create<ClaimModel>();

            _mockRoleStore.Setup(_ => _.FindByIdAsync(model.Id))
              .ReturnsAsync(appRole);

            _mockRepository.Setup(_ => _.Add(It.Is<RoleClaim>(
                value => value.RoleId == appRole.Id &&
                  value.ClaimType == claim.ClaimType &&
                  value.ClaimValue == claim.ClaimValue)))
            .Returns(new RoleClaim());

            var entityResult = await _roleService.AddClaimAsync(model.Id, claim);

            Mock.VerifyAll();
            entityResult.Model.ClaimValue.Should().Be(claim.ClaimValue);
            entityResult.Model.ClaimType.Should().Be(claim.ClaimType);
        }

        [Fact]
        public async void AddClaim_Should_Call_Manager_And_Not_Repository_When_Role_Does_Not_Exist()
        {
            var model = Fixture.Create<RoleModel>();

            var claim = Fixture.Create<ClaimModel>();

            _mockRoleStore.Setup(_ => _.FindByIdAsync(model.Id))
              .ReturnsAsync(null);

            var entityResult = await _roleService.AddClaimAsync(model.Id, claim);

            Mock.VerifyAll();
            entityResult.Succeeded.Should().BeFalse();
            entityResult.IsEntityNotFound.Should().BeTrue();
        }

        [Fact]
        public async void CreateRoleAsync_Should_Call_RoleManager()
        {
            var model = Fixture.Create<RoleModel>();

            var role = new ApplicationRole
            {
                Name = "Admin",
                Id = Guid.NewGuid().ToString()
            };

            Queue<ApplicationRole> returnQueue = new Queue<ApplicationRole>();
            returnQueue.Enqueue(null);
            returnQueue.Enqueue(role);

            _mockRoleStore.Setup(_ => _.FindByNameAsync(model.Name))
               .Returns(() => Task.FromResult(returnQueue.Dequeue()));

            _mockRoleStore.Setup(_ => _.CreateAsync(It.Is<ApplicationRole>(value => value.Name == model.Name)))
                .Returns(Task.FromResult(0));

            var result = await _roleService.CreateRoleAsync(model);

            Mock.VerifyAll();
            result.Should().NotBeNull();
            result.Model.Name.Should().Be(role.Name);
            result.Model.Id.Should().Be(role.Id);
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public void Ctor_Should_Throw_Null_Argument_Exception_When_RoleManager_Is_Null()
        {
            Action throwAction = () => new RoleService(null, null, _mapperFixture.MapperInstance);

            throwAction.ShouldThrow<ArgumentNullException>()
                .And
                .ParamName.Should().Be("roleManager");
        }

        [Fact]
        public void Ctor_Should_Throw_Null_Argument_Exception_When_RoleClaimRepository_Is_Null()
        {
            Action throwAction = () => new RoleService(_roleManager, null, _mapperFixture.MapperInstance);

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
            var roles = new[] 
            {
                new ApplicationRole { Name = "Role1" },
                new ApplicationRole { Name = "Role2" }
            };

            _mockRoleStore.As<IQueryableRoleStore<ApplicationRole>>()
                .Setup(_ => _.Roles)
                .Returns(roles.AsQueryable());

            var result = _roleService.GetRoles();

            Mock.Verify();
            result.Should().NotBeNull();
            result.Model.Should().HaveCount(roles.Length);

            roles.All(_ => result.Model.Any(r => r.Name == _.Name))
                .Should()
                .BeTrue();
        }
    }
}
