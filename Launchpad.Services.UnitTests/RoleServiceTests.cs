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

namespace Launchpad.Services.UnitTests
{
    public class RoleServiceTests : BaseUnitTest
    {
        private RoleService _roleService;
        private ApplicationRoleManager _roleManager;
        private Mock<IRoleStore<ApplicationRole>> _mockRoleStore;

        public RoleServiceTests()
        {
            _mockRoleStore = Mock.Create<IRoleStore<ApplicationRole>>();
            _roleManager = new ApplicationRoleManager(_mockRoleStore.Object);
            _roleService = new RoleService(_roleManager);
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
            Action throwAction = () => new RoleService(null);

            throwAction.ShouldThrow<ArgumentNullException>()
                .And
                .ParamName.Should().Be("roleManager");
        }
    }
}
