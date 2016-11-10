using System;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using FluentAssertions;
using Xunit;
using Moq;
using Launchpad.UnitTests.Common;
using Launchpad.Models.EntityFramework;
using Launchpad.Services.IdentityManagers;
using Microsoft.AspNet.Identity;
using Launchpad.Models;
using System.Text;
using Launchpad.Services.Fixture;
using System.Linq;
using Launchpad.Services.Interfaces;
using System.Security.Claims;
using System.Collections.Generic;

namespace Launchpad.Services.UnitTests
{
    [Trait("Category", "User.Service")]
    [Collection(AutoMapperCollection.CollectionName)]
    public class UserServiceTests : BaseUnitTest
    {
        private UserService _userService;
        private ApplicationUserManager _userManager;
        private Mock<IUserStore<ApplicationUser>> _mockStore;
        private Mock<IRoleService> _mockRoleService;
        private AutoMapperFixture _mapperFixture;

        public UserServiceTests(AutoMapperFixture mapperFixture)
        {
            _mockStore = Mock.Create<IUserStore<ApplicationUser>>();
            _mockStore.As<IUserPasswordStore<ApplicationUser>>();
            _mockStore.As<IQueryableUserStore<ApplicationUser>>();
            _mockStore.As<IUserRoleStore<ApplicationUser>>();
            _mockStore.As<IUserClaimStore<ApplicationUser>>();    

            _mockRoleService = Mock.Create<IRoleService>();
            _mapperFixture = mapperFixture;

            //Cannot moq the interface directly, consider creating a facade around the manager class
            _userManager = new ApplicationUserManager(_mockStore.Object);
            _userService = new UserService(_userManager, _mapperFixture.MapperInstance, _mockRoleService.Object);
        }

        [Fact]
        public async void DeleteUser_Should_Call_UserManager()
        {
            //Arrange
            var id = Fixture.Create<string>();
            var appUser = new ApplicationUser
            {
                UserName = "bob@bob.com",
                Email = "bob@bob.com",
                Id = id
            };
            _mockStore.Setup(_ => _.DeleteAsync(appUser))
                .Returns(Task.Delay(0));

            _mockStore.Setup(_ => _.FindByIdAsync(id))
                .ReturnsAsync(appUser);

            //Act
            var result = await _userService.DeleteUser(id);

            //Assert
            result.Succeeded.Should().BeTrue();

            Mock.VerifyAll();
        }

        [Fact]
        public async void UpdateUser_Should_Call_UserManager()
        {
            var id = Fixture.Create<string>();
            var userModel = new UserModel
            {
                Id = id,
                Name = "sally@sally.com"
            };

            var appUser = new ApplicationUser
            {
                Id = id,
                UserName = "sue@sue.com",
                Email = "sue@sue.com"
            };

            _mockStore.Setup(_ => _.FindByIdAsync(id))
                .ReturnsAsync(appUser);

            _mockStore.Setup(_ => _.FindByNameAsync(userModel.Name))
                .ReturnsAsync(appUser);


            _mockStore.Setup(_ => _.UpdateAsync(It.Is<ApplicationUser>(user => user.UserName == userModel.Name)))
                .Returns(Task.Delay(0));

            var result = await _userService.UpdateUser(id, userModel);

            result.Result.Succeeded.Should().BeTrue();
            result.Model.Name.Should().Be(userModel.Name);
        }

        [Fact]
        public async void GetUser_Should_Call_UserManager()
        {
            var id = Fixture.Create<string>();
            var appUser = new ApplicationUser
            {
                Email = "bob@bob.com",
                UserName = "bob@bob.com",
                Id = Guid.NewGuid().ToString()
            };

            _mockStore.Setup(_ => _.FindByIdAsync(id))
                .ReturnsAsync(appUser);

            var result = await _userService.GetUser(id);

            result.Name.Should().Be(appUser.UserName);
            result.Id.Should().Be(appUser.Id);
            Mock.VerifyAll();

        }

        [Fact]
        public async void GetUserRolesAsync_Should_Call_UserManager_And_Role_Service()
        {
            var userId = Fixture.Create<string>();

            var appUser = new ApplicationUser
            {
                UserName = "bob@bob.com",
                Email = "bob@bob.com",
                Id = userId
            };

            

            var fakeRoles = Fixture.CreateMany<RoleModel>();

            var assignedRoles = fakeRoles.Take(1).Select(_=>_.Name).ToList();

            _mockStore.Setup(_ => _.FindByIdAsync(userId))
                .ReturnsAsync(appUser);

            _mockStore.As<IUserRoleStore<ApplicationUser>>()
                .Setup(_ => _.GetRolesAsync(appUser))
                .ReturnsAsync(assignedRoles);

            _mockRoleService.Setup(_ => _.GetRoles())
                .Returns(fakeRoles);

            var roles = await _userService.GetUserRolesAsync(userId);


            Mock.VerifyAll();

            roles.Should().NotBeNullOrEmpty();
            roles.Should().HaveCount(1);
            roles.First().Name.Should().Be(assignedRoles.First());
             
        }

        [Fact]
        public async void GetUserClaimsAsync_Should_Call_User_Manager()
        {
            var roles = new string[] { "Admin" };
            var id = Fixture.Create<string>();
            var roleClaims = new[] { new ClaimModel { ClaimType = "type1", ClaimValue = "value1" } };
            var appUser = new ApplicationUser
            {
                Id = id,
                UserName = "admin@admin.com"
            };

            _mockStore.Setup(_ => _.FindByIdAsync(id))
                .ReturnsAsync(appUser);

            _mockStore.Setup(_ => _.FindByNameAsync(appUser.UserName))
                .ReturnsAsync(appUser);

            _mockStore.As<IUserRoleStore<ApplicationUser>>()
                .Setup(_ => _.GetRolesAsync(appUser))
                .ReturnsAsync(roles);

            _mockStore.As<IUserClaimStore<ApplicationUser>>()
                .Setup(_ => _.GetClaimsAsync(appUser))
                .ReturnsAsync(new List<Claim>());

            _mockRoleService.Setup(_ => _.GetRoleClaims(roles[0]))
                .Returns(roleClaims);

            //Act
            var result = await _userService.GetUserClaimsAsync(id);

            Mock.VerifyAll();
            result.Should().NotBeNullOrEmpty();
            result.Any(_ => _.ClaimValue == "value1" && _.ClaimType == "type1").Should().BeTrue();

        }

        [Fact]
        public async void RemoveUserFromRoleAsync_Should_Call_User_Manager()
        {
            var userId = "user-id";
            var roleId = "role-id";
            var roleModel = Fixture.Create<RoleModel>();



            var appUser = new ApplicationUser
            {
                UserName = "bob@bob.com",
                Email = "bob@bob.com"
            };

            _mockRoleService.Setup(_ => _.GetRoleById(roleId))
                .Returns(roleModel);

            _mockStore.Setup(_ => _.FindByIdAsync(userId))
                .ReturnsAsync(appUser);

            _mockStore.As<IUserRoleStore<ApplicationUser>>()
                .Setup(_ => _.IsInRoleAsync(appUser, roleModel.Name))
                .ReturnsAsync(true);

            _mockStore.As<IUserRoleStore<ApplicationUser>>()
                .Setup(_ => _.RemoveFromRoleAsync(appUser, roleModel.Name))
                .Returns(Task.Delay(0));

            _mockStore.Setup(_ => _.FindByNameAsync(appUser.UserName))
                .ReturnsAsync(appUser);

            _mockStore.Setup(_ => _.UpdateAsync(appUser))
                .Returns(Task.Delay(0));

            var result = await _userService.RemoveUserFromRoleAsync(userId, roleId);

            Mock.VerifyAll();
            result.Succeeded.Should().BeTrue();
        }

        [Fact]

        public async void AssignUserRoleAsync_Should_Call_User_Manager_Return_IdentityResult_Success_When_Sucessful()
        {
            //arrange

            var userModel = Fixture.Build<UserModel>()
                .With(_ => _.Name, "bob@bob.com")
                .Create(); ;

        

            var roleModel = Fixture.Create<RoleModel>();
            var serviceModel = Fixture.Create<RoleModel>();

            var applicationUser = new ApplicationUser { Id = userModel.Id, UserName = userModel.Name };
            _mockStore.As<IUserRoleStore<ApplicationUser>>()
                .Setup(_ => _.FindByIdAsync(userModel.Id))
                .ReturnsAsync(applicationUser);

            _mockStore.As<IUserRoleStore<ApplicationUser>>()
                .Setup(_ => _.GetRolesAsync(applicationUser))
                .ReturnsAsync(new string[0]);

            _mockStore.As<IUserRoleStore<ApplicationUser>>()
                .Setup(_ => _.AddToRoleAsync(applicationUser, roleModel.Name))
                .Returns(Task.Delay(0));

            _mockStore.Setup(_ => _.FindByNameAsync(userModel.Name))
                .ReturnsAsync(applicationUser);

            _mockStore.Setup(_ => _.UpdateAsync(applicationUser))
                .Returns(Task.Delay(0));

            _mockRoleService.Setup(_ => _.GetRoleByName(roleModel.Name))
                .Returns(serviceModel);
         

            //act
            var result = await _userService.AssignUserRoleAsync(userModel.Id, roleModel);


            //assert
            Mock.VerifyAll();
            result.Should().NotBeNull();
            result.Result.Succeeded.Should().BeTrue();
            result.Model.ShouldBeEquivalentTo(serviceModel);

        }

        [Fact]
        public void GetUserRoleById_Should_Call_Role_Service_And_UserManager()
        {
            var userId = Fixture.Create<string>();
            var roleId = Fixture.Create<string>();
            var appUser = new ApplicationUser
            {
                UserName = "bob@bob.com",
                Email = "bob@bob.com"
            };

            var model = Fixture.Create<RoleModel>();
            _mockRoleService.Setup(_ => _.GetRoleById(roleId))
                .Returns(model);

            _mockStore.Setup(_ => _.FindByIdAsync(userId))
                .ReturnsAsync(appUser);

            _mockStore.As<IUserRoleStore<ApplicationUser>>()
                .Setup(_ => _.IsInRoleAsync(appUser, model.Name))
                .ReturnsAsync(true);

            var role = _userService.GetUserRoleById(userId, roleId);


            Mock.VerifyAll();
            role.ShouldBeEquivalentTo(model);

        }


        [Fact]
        public void GetUserRoleById_Should_Call_Role_Service_And_UserManager_And_Return_Null_When_Not_Found()
        {
            var userId = Fixture.Create<string>();
            var roleId = Fixture.Create<string>();
            var appUser = new ApplicationUser
            {
                UserName = "bob@bob.com",
                Email = "bob@bob.com"
            };

            var model = Fixture.Create<RoleModel>();
            _mockRoleService.Setup(_ => _.GetRoleById(roleId))
                .Returns(model);

            _mockStore.Setup(_ => _.FindByIdAsync(userId))
                .ReturnsAsync(appUser);

            _mockStore.As<IUserRoleStore<ApplicationUser>>()
                .Setup(_ => _.IsInRoleAsync(appUser, model.Name))
                .ReturnsAsync(false);

            var role = _userService.GetUserRoleById(userId, roleId);


            Mock.VerifyAll();
            role.Should().BeNull();

        }

        [Fact]
        public async void CreateClaimsIdentity_Should_Return_Identity()
        {
            var roleClaims = new[] { new ClaimModel { ClaimType = "permission", ClaimValue = "delete.widget" } };

            _mockRoleService.Setup(_ => _.GetRoleClaims("Admin"))
             .Returns(roleClaims);

            string user = "bob@bob.com";
            var model = new ApplicationUser() { UserName = user };

            _mockStore.Setup(_ => _.FindByIdAsync(model.Id))
                .ReturnsAsync(model);

            _mockStore.As<IUserRoleStore<ApplicationUser>>()
                .Setup(_ => _.GetRolesAsync(model))
                .ReturnsAsync(new string[] { "Admin" });

            _mockStore.As<IUserClaimStore<ApplicationUser>>()
                .Setup(_ => _.GetClaimsAsync(model))
                .ReturnsAsync(new[] { new Claim("permission", "view.widget") });

            _mockStore.Setup(_ => _.FindByNameAsync(user))
               .ReturnsAsync(model);

            var result = await _userService.CreateClaimsIdentityAsync(user, "OAuth");

            result.Should().NotBeNull();
            result.HasClaim("permission", "view.widget");
            result.HasClaim("permission", "delete.widget");
            result.HasClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin");
            result.HasClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", user).Should().BeTrue();

            Mock.VerifyAll();

        }

        [Fact]
        public void CreateClaimsIdentity_Should_Throw_ArgumentException_when_User_Is_Not_Found()
        {
            string user = "bob@bob.com";

            _mockStore.Setup(_ => _.FindByNameAsync(user))
                .ReturnsAsync(null);


            Func<Task> throwAction = async () =>
                await _userService.CreateClaimsIdentityAsync(user, "OAuth");

            throwAction.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public async void IsValidCredential_Should_Return_True_When_User_Is_Found()
        {
            string user = "bob@bob.com";
            string password = "giraffe";
            var hpw = new PasswordHasher().HashPassword(password);
            var model = new ApplicationUser();

            _mockStore.Setup(_ => _.FindByNameAsync(user)).ReturnsAsync(model);
            _mockStore.As<IUserPasswordStore<ApplicationUser>>()
                .Setup(_ => _.GetPasswordHashAsync(model))
                .ReturnsAsync(hpw);
            var result = await _userService.IsValidCredential(user, password);

            Mock.VerifyAll();
            result.Should().BeTrue();
        }

        [Fact]
        public async void IsValidCredential_Should_Return_False_When_User_Is_Not_Found()
        {
            string user = "bob@bob.cm";
            string password = "giraffe";
            var hash = Convert.ToBase64String(new byte[] { 1, 2, 3 });
            var model = new ApplicationUser();

            _mockStore.Setup(_ => _.FindByNameAsync(user)).ReturnsAsync(model);
            _mockStore.As<IUserPasswordStore<ApplicationUser>>()
                .Setup(_ => _.GetPasswordHashAsync(model))
                .ReturnsAsync(hash);


            var result = await _userService.IsValidCredential(user, password);


            Mock.VerifyAll();
            result.Should().BeFalse();
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_UserManager_IsNull()
        {
            Action throwAction = () => new UserService(null, null, null);

            throwAction.ShouldThrow<ArgumentNullException>()
                .And
                .ParamName
                .Should()
                .Be("userManager");
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_Mapper_IsNull()
        {
            Action throwAction = () => new UserService(_userManager, null, null);

            throwAction.ShouldThrow<ArgumentNullException>()
                .And
                .ParamName
                .Should()
                .Be("mapper");
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_RoleService_IsNull()
        {
            Action throwAction = () => new UserService(_userManager, _mapperFixture.MapperInstance, null);

            throwAction.ShouldThrow<ArgumentNullException>()
                .And
                .ParamName
                .Should()
                .Be("roleService");
        }

        [Fact]
        public void GetUsers_Should_Call_UserManager()
        {
            var user1 = new ApplicationUser
            {
                UserName = "bob@bob.com",
                Id = "abc"
            };

            var userResults = new[] { user1 };

            _mockStore.As<IQueryableUserStore<ApplicationUser>>()
                .Setup(_ => _.Users)
                .Returns(userResults.AsQueryable());

            var result = _userService.GetUsers();

            Mock.VerifyAll();
            result.Should().HaveSameCount(userResults);

            userResults.All(_ => result.Any(r => r.Name == _.UserName))
               .Should()
               .BeTrue();
        }


       
        [Fact]
        public async Task Register_Should_Call_UserManager()
        {

            //ARRANGE
            string userId = string.Empty;
            var model = Fixture.Build<RegistrationModel>()
                .With(_ => _.Email, "test@test.com")
                .With(_ => _.Password, "cool1Password!!")
                .Create();

            var identityResult = new IdentityResult();

            var applicationUser = new ApplicationUser();

            _mockStore.As<IUserPasswordStore<ApplicationUser>>()
                .Setup(_ => _.SetPasswordHashAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Returns(Task.Delay(0));

            _mockStore.As<IUserRoleStore<ApplicationUser>>()
                .Setup(_ => _.GetRolesAsync(applicationUser))
                .ReturnsAsync(new string[0]);

            _mockStore.Setup(_ => _.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(applicationUser);

            _mockStore.As<IUserRoleStore<ApplicationUser>>()
                .Setup(_ => _.AddToRoleAsync(applicationUser, "Basic"))
                .Returns(Task.Delay(0));

            _mockStore.Setup(_ => _.FindByNameAsync(It.Is<string>(match => match == model.Email)))
                .ReturnsAsync(null);

            _mockStore.Setup(_ => _.CreateAsync(It.Is<ApplicationUser>(match => match.Email == model.Email && match.UserName == model.Email)))
                .Returns(Task.Delay(0));

            //ACT
            var result = await _userService.RegisterAsync(model);

            //ASSERT
            Mock.VerifyAll();
        }

        [Fact]
        public async Task Register_Should_Return_Error_When_Model_Is_Invalid()
        {

            //ARRANGE

            var model = Fixture.Build<RegistrationModel>()
                .With(_ => _.Email, "testtestcom")
                .With(_ => _.Password, "cool1Password!!")
                .Create();

            _mockStore.As<IUserPasswordStore<ApplicationUser>>()
                .Setup(_ => _.SetPasswordHashAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Returns(Task.Delay(0));

            _mockStore.Setup(_ => _.FindByNameAsync(It.Is<string>(match => match == model.Email)))
                .ReturnsAsync(new ApplicationUser());

            //ACT
            var result = await _userService.RegisterAsync(model);


            //ASSERT
            Mock.VerifyAll();
            result.Succeeded.Should().BeFalse();
        }
    }
}
