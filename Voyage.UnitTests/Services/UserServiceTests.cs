using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Voyage.Core.Exceptions;
using Voyage.Data.Repositories.UserPhone;
using Voyage.Models;
using Voyage.Models.Entities;
using Voyage.Services.IdentityManagers;
using Voyage.Services.Role;
using Voyage.Services.User;
using Voyage.UnitTests.Common;
using Voyage.UnitTests.Common.AutoMapperFixture;
using Microsoft.AspNet.Identity;
using Moq;
using Ploeh.AutoFixture;
using Xunit;

namespace Voyage.UnitTests.Services
{
    [Trait("Category", "User.Service")]
    [Collection(AutoMapperCollection.CollectionName)]
    public class UserServiceTests : BaseUnitTest
    {
        private readonly UserService _userService;
        private readonly ApplicationUserManager _userManager;
        private readonly Mock<IUserStore<ApplicationUser>> _mockStore;
        private readonly Mock<IRoleService> _mockRoleService;
        private readonly Mock<IUserPhoneRepository> _mockPhoneRepository;
        private readonly AutoMapperFixture _mapperFixture;

        public UserServiceTests(AutoMapperFixture mapperFixture)
        {
            _mockStore = Mock.Create<IUserStore<ApplicationUser>>();
            _mockStore.As<IUserPasswordStore<ApplicationUser>>();
            _mockStore.As<IQueryableUserStore<ApplicationUser>>();
            _mockStore.As<IUserRoleStore<ApplicationUser>>();
            _mockStore.As<IUserClaimStore<ApplicationUser>>();

            _mockRoleService = Mock.Create<IRoleService>();
            _mockPhoneRepository = Mock.Create<IUserPhoneRepository>();
            _mapperFixture = mapperFixture;

            // Cannot moq the interface directly, consider creating a facade around the manager class
            _userManager = new ApplicationUserManager(_mockStore.Object);
            _userService = new UserService(_userManager, _mapperFixture.MapperInstance, _mockRoleService.Object, _mockPhoneRepository.Object);
        }

        [Fact]
        public async void CreateUserAsync_Should_Call_UserManager()
        {
            var userModel = Fixture.Build<UserModel>()
                                .With(_ => _.Username, "bob@bob.com")
                                .Create();

            _mockStore.Setup(_ => _.CreateAsync(It.Is<ApplicationUser>(appUser => appUser.UserName == userModel.Username)))
                .Returns(Task.Delay(0));
            _mockStore.As<IUserPasswordStore<ApplicationUser>>()
                .Setup(_ => _.SetPasswordHashAsync(It.Is<ApplicationUser>(appUser => appUser.UserName == userModel.Username), It.IsAny<string>()))
                .Returns(Task.Delay(0));
            _mockStore.Setup(_ => _.FindByNameAsync("bob@bob.com"))
                .ReturnsAsync(null);

            var entityResult = await _userService.CreateUserAsync(userModel);

            entityResult.Username.Should().Be(userModel.Username);
        }

        [Fact]
        public void DeleteUser_Should_Return_Failed_Result_When_Not_Found()
        {
            var id = Fixture.Create<string>();
            _mockStore.Setup(_ => _.FindByIdAsync(id))
                .ReturnsAsync(null);

            // Assert
            Assert.ThrowsAsync<NotFoundException>(async () => { await _userService.DeleteUserAsync(id); });
        }

        [Fact]
        public async void DeleteUser_Should_Call_UserManager()
        {
            // Arrange
            var id = Fixture.Create<string>();
            var appUser = new ApplicationUser
            {
                UserName = "bob@bob.com",
                Email = "bob@bob.com",
                Id = id
            };

            _mockStore.Setup(_ => _.FindByIdAsync(id))
                .ReturnsAsync(appUser);

            _mockStore.Setup(_ => _.FindByNameAsync(appUser.UserName))
                    .ReturnsAsync(appUser);

            _mockStore.Setup(_ => _.UpdateAsync(appUser))
                .Callback<ApplicationUser>(user => user.IsActive.Should().BeFalse())
                .Returns(Task.Delay(0));

            // Act
            var result = await _userService.DeleteUserAsync(id);

            // Assert
            result.Succeeded.Should().BeTrue();

            Mock.VerifyAll();
        }

        [Fact]
        public void UpdateUser_Should_Return_Failed_Result_When_User_NotFound()
        {
            var id = Fixture.Create<string>();
            var userModel = new UserModel
            {
                Id = id,
                Username = "sally@sally.com",
                FirstName = "First1",
                LastName = "Last1"
            };
            _mockStore.Setup(_ => _.FindByIdAsync(id))
                .ReturnsAsync(null);

            Assert.ThrowsAsync<NotFoundException>(async () => { await _userService.UpdateUserAsync(id, userModel); });
        }

        [Fact]
        public async void UpdateUser_Should_Call_UserManager()
        {
            var id = Fixture.Create<string>();
            var userModel = new UserModel
            {
                Id = id,
                Username = "sally@sally.com",
                FirstName = "First1",
                LastName = "Last1",
                Phones = new List<UserPhoneModel>()
            };

            var appUser = new ApplicationUser
            {
                Id = id,
                UserName = "sue@sue.com",
                Email = "sue@sue.com",
                FirstName = "First2",
                LastName = "Last2",
                Phones = new List<UserPhone>()
            };

            _mockStore.Setup(_ => _.FindByIdAsync(id))
                .ReturnsAsync(appUser);

            _mockStore.Setup(_ => _.FindByNameAsync(userModel.Username))
                .ReturnsAsync(appUser);

            _mockStore.Setup(_ => _.UpdateAsync(It.Is<ApplicationUser>(user => user.UserName == userModel.Username)))
                .Returns(Task.Delay(0));

            var entityResult = await _userService.UpdateUserAsync(id, userModel);

            entityResult.Username.Should().Be(userModel.Username);
            entityResult.LastName.Should().Be(userModel.LastName);
            entityResult.FirstName.Should().Be(userModel.FirstName);
        }

        [Fact]
        public async void UpdateUser_Should_Remove_Phone_Numbers_When_Not_In_Source()
        {
            var id = Fixture.Create<string>();

            var phone = Fixture.Build<UserPhone>()
                .With(_ => _.User, null)
                .Create();

            var userModel = new UserModel
            {
                Id = id,
                Username = "sally@sally.com",
                FirstName = "First1",
                LastName = "Last1",
                Phones = new List<UserPhoneModel>()
            };

            var appUser = new ApplicationUser
            {
                Id = id,
                UserName = "sue@sue.com",
                Email = "sue@sue.com",
                FirstName = "First2",
                LastName = "Last2",
                Phones = new List<UserPhone> { phone }
            };

            _mockPhoneRepository.Setup(_ => _.Delete(phone.Id));

            _mockStore.Setup(_ => _.FindByIdAsync(id))
                .ReturnsAsync(appUser);

            _mockStore.Setup(_ => _.FindByNameAsync(userModel.Username))
                .ReturnsAsync(appUser);

            _mockStore.Setup(_ => _.UpdateAsync(It.Is<ApplicationUser>(user => user.UserName == userModel.Username)))
                .Returns(Task.Delay(0));

            var entityResult = await _userService.UpdateUserAsync(id, userModel);

            entityResult.Phones.Should().BeEmpty();
        }

        [Fact]
        public async void UpdateUser_Should_Update_Existing_Phone_Numbers()
        {
            var id = Fixture.Create<string>();
            var phoneModel = Fixture.Create<UserPhoneModel>();

            var phone = Fixture.Build<UserPhone>()
                .With(_ => _.User, null)
                .With(_ => _.Id, phoneModel.Id)
                .Create();

            var userModel = new UserModel
            {
                Id = id,
                Username = "sally@sally.com",
                FirstName = "First1",
                LastName = "Last1",
                Phones = new List<UserPhoneModel> { phoneModel }
            };

            var appUser = new ApplicationUser
            {
                Id = id,
                UserName = "sue@sue.com",
                Email = "sue@sue.com",
                FirstName = "First2",
                LastName = "Last2",
                Phones = new List<UserPhone> { phone }
            };

            _mockStore.Setup(_ => _.FindByIdAsync(id))
                .ReturnsAsync(appUser);

            _mockStore.Setup(_ => _.FindByNameAsync(userModel.Username))
                .ReturnsAsync(appUser);

            _mockStore.Setup(_ => _.UpdateAsync(It.Is<ApplicationUser>(user => user.UserName == userModel.Username)))
                .Returns(Task.Delay(0));

            var entityResult = await _userService.UpdateUserAsync(id, userModel);

            entityResult.Phones
                .Should()
                .NotBeEmpty()
                .And
                .HaveCount(1);
            entityResult.Phones.First().ShouldBeEquivalentTo(phoneModel);
        }

        [Fact]
        public async void UpdateUser_Should_Add_New_Phone_Numbers()
        {
            var id = Fixture.Create<string>();
            var phone = Fixture.Create<UserPhoneModel>();

            var userModel = new UserModel
            {
                Id = id,
                Username = "sally@sally.com",
                FirstName = "First1",
                LastName = "Last1",
                Phones = new List<UserPhoneModel> { phone }
            };

            var appUser = new ApplicationUser
            {
                Id = id,
                UserName = "sue@sue.com",
                Email = "sue@sue.com",
                FirstName = "First2",
                LastName = "Last2",
                Phones = new List<UserPhone>()
            };

            _mockStore.Setup(_ => _.FindByIdAsync(id))
                .ReturnsAsync(appUser);

            _mockStore.Setup(_ => _.FindByNameAsync(userModel.Username))
                .ReturnsAsync(appUser);

            _mockStore.Setup(_ => _.UpdateAsync(It.Is<ApplicationUser>(user => user.UserName == userModel.Username)))
                .Returns(Task.Delay(0));

            var entityResult = await _userService.UpdateUserAsync(id, userModel);

            entityResult.Phones
                .Should()
                .NotBeEmpty()
                .And
                .HaveCount(1);
            entityResult.Phones.First().ShouldBeEquivalentTo(phone);
        }

        [Fact]
        public void GetUser_Should_Return_Not_Found_When_User_Is_Deleted()
        {
            var id = Fixture.Create<string>();
            var appUser = new ApplicationUser
            {
                Email = "bob@bob.com",
                UserName = "bob@bob.com",
                Id = Guid.NewGuid().ToString(),
                Deleted = true
            };

            _mockStore.Setup(_ => _.FindByIdAsync(id))
                .ReturnsAsync(appUser);

            Assert.ThrowsAsync<NotFoundException>(async () => await _userService.GetUserAsync(id));
            Mock.VerifyAll();
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

            var entityResult = await _userService.GetUserAsync(id);

            entityResult.Username.Should().Be(appUser.UserName);
            entityResult.Id.Should().Be(appUser.Id);
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

            var fakeRoles = Fixture.CreateMany<RoleModel>().ToList();

            var assignedRoles = fakeRoles.Take(1).Select(_ => _.Name).ToList();

            _mockStore.Setup(_ => _.FindByIdAsync(userId))
                .ReturnsAsync(appUser);

            _mockStore.As<IUserRoleStore<ApplicationUser>>()
                .Setup(_ => _.GetRolesAsync(appUser))
                .ReturnsAsync(assignedRoles);

            _mockRoleService.Setup(_ => _.GetRoles())
                .Returns(fakeRoles);

            var entityResult = await _userService.GetUserRolesAsync(userId);

            Mock.VerifyAll();

            entityResult.Should().NotBeNullOrEmpty();
            entityResult.Should().HaveCount(1);
            entityResult.First().Name.Should().Be(assignedRoles.First());
        }

        [Fact]
        public void GetUserClaimsAsync_Should_Return_Failed_Result_When_Not_Found()
        {
            // Arrange
            var id = Fixture.Create<string>();
            _mockStore.Setup(_ => _.FindByIdAsync(id))
                .ReturnsAsync(null);

            // Assert
            Assert.ThrowsAsync<NotFoundException>(async () => { await _userService.GetUserClaimsAsync(id); });
        }

        [Fact]
        public async void GetUserClaimsAsync_Should_Call_User_Manager()
        {
            var roles = new[] { "Admin" };
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

            // Act
            var entityResult = await _userService.GetUserClaimsAsync(id);

            Mock.VerifyAll();
            entityResult.Should().NotBeNullOrEmpty();
            entityResult.Any(_ => _.ClaimValue == "value1" && _.ClaimType == "type1").Should().BeTrue();
        }

        public async void RemoveUserFromRoleAsync_Should_Return_Failed_Result_When_Not_Found()
        {
            var roleId = "role-id";
            var userId = "user-id";
            RoleModel model = null;
            var entityResult = new IdentityResult(null);
            _mockRoleService.Setup(_ => _.GetRoleById(roleId))
            .Returns(model);

            var methodResult = await _userService.RemoveUserFromRoleAsync(userId, roleId);
            methodResult.Should().Be(entityResult);
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
            // arrange
            var userModel = Fixture.Build<UserModel>()
                .With(_ => _.Username, "bob@bob.com")
                .Create();

            var roleModel = Fixture.Create<RoleModel>();
            var serviceModel = Fixture.Create<RoleModel>();

            var applicationUser = new ApplicationUser { Id = userModel.Id, UserName = userModel.Username };
            _mockStore.As<IUserRoleStore<ApplicationUser>>()
                .Setup(_ => _.FindByIdAsync(userModel.Id))
                .ReturnsAsync(applicationUser);

            _mockStore.As<IUserRoleStore<ApplicationUser>>()
                .Setup(_ => _.GetRolesAsync(applicationUser))
                .ReturnsAsync(new string[0]);

            _mockStore.As<IUserRoleStore<ApplicationUser>>()
                .Setup(_ => _.AddToRoleAsync(applicationUser, roleModel.Name))
                .Returns(Task.Delay(0));

            _mockStore.Setup(_ => _.FindByNameAsync(userModel.Username))
                .ReturnsAsync(applicationUser);

            _mockStore.Setup(_ => _.UpdateAsync(applicationUser))
                .Returns(Task.Delay(0));

            _mockRoleService.Setup(_ => _.GetRoleByName(roleModel.Name))
                .Returns(serviceModel);

            // act
            var entityResult = await _userService.AssignUserRoleAsync(userModel.Id, roleModel);

            // assert
            Mock.VerifyAll();
            entityResult.Should().NotBeNull();
            entityResult.ShouldBeEquivalentTo(serviceModel);
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

            var entityResult = _userService.GetUserRoleById(userId, roleId);

            Mock.VerifyAll();
            entityResult.ShouldBeEquivalentTo(model);
        }

        [Fact]
        public void GetUserRoleById_Should_Return_Failed_Result_When_User_Not_Found()
        {
            var userId = Fixture.Create<string>();
            var roleId = Fixture.Create<string>();
            _mockRoleService.Setup(_ => _.GetRoleById(roleId))
               .Returns((RoleModel)null);

            Assert.Throws<NotFoundException>(() => { _userService.GetUserRoleById(userId, roleId); });
        }

        [Fact]
        public void GetUserRoleById_Should_Call_Role_Service_And_UserManager_And_Return_Unauthorized()
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

            Assert.Throws<UnauthorizedException>(() => { _userService.GetUserRoleById(userId, roleId); });
        }

        [Fact]
        public async void CreateClaimsIdentity_Should_Return_Identity()
        {
            var roleClaims = new[] { new ClaimModel { ClaimType = "permission", ClaimValue = "delete.test" } };

            _mockRoleService.Setup(_ => _.GetRoleClaims("Admin"))
             .Returns(roleClaims);

            string user = "bob@bob.com";
            var model = new ApplicationUser { UserName = user };

            _mockStore.Setup(_ => _.FindByIdAsync(model.Id))
                .ReturnsAsync(model);

            _mockStore.As<IUserRoleStore<ApplicationUser>>()
                .Setup(_ => _.GetRolesAsync(model))
                .ReturnsAsync(new[] { "Admin" });

            _mockStore.As<IUserClaimStore<ApplicationUser>>()
                .Setup(_ => _.GetClaimsAsync(model))
                .ReturnsAsync(new[] { new Claim("permission", "view.test") });

            _mockStore.Setup(_ => _.FindByNameAsync(user))
               .ReturnsAsync(model);

            var result = await _userService.CreateClaimsIdentityAsync(user, "OAuth");

            result.Should().NotBeNull();
            result.HasClaim("permission", "view.test");
            result.HasClaim("permission", "delete.test");
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

            throwAction.ShouldThrow<NotFoundException>();
        }

        [Fact]
        public async void IsValidCredential_Should_Return_True_When_User_Is_Found()
        {
            string user = "bob@bob.com";
            string password = "giraffe";
            var hpw = new PasswordHasher().HashPassword(password);
            var model = new ApplicationUser { IsActive = true };

            _mockStore.Setup(_ => _.FindByNameAsync(user)).ReturnsAsync(model);
            _mockStore.As<IUserPasswordStore<ApplicationUser>>()
                .Setup(_ => _.GetPasswordHashAsync(model))
                .ReturnsAsync(hpw);
            var result = await _userService.IsValidCredential(user, password);

            Mock.VerifyAll();
            result.Should().BeTrue();
        }

        [Fact]
        public async void IsValidCredential_Should_Return_False_When_User_Is_Not_Active()
        {
            string user = "bob@bob.com";
            string password = "giraffe";
            var hpw = new PasswordHasher().HashPassword(password);
            var model = new ApplicationUser { IsActive = false };

            _mockStore.Setup(_ => _.FindByNameAsync(user)).ReturnsAsync(model);
            _mockStore.As<IUserPasswordStore<ApplicationUser>>()
                .Setup(_ => _.GetPasswordHashAsync(model))
                .ReturnsAsync(hpw);
            var result = await _userService.IsValidCredential(user, password);

            Mock.VerifyAll();
            result.Should().BeFalse();
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
            Action throwAction = () => new UserService(null, _mapperFixture.MapperInstance, null, null);

            throwAction.ShouldThrow<ArgumentNullException>()
                .And
                .ParamName
                .Should()
                .Be("userManager");
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_UserPhoneRepository_IsNull()
        {
            Action throwAction =
                () => new UserService(_userManager, _mapperFixture.MapperInstance, _mockRoleService.Object, null);

            throwAction.ShouldThrow<ArgumentNullException>()
                .And
                .ParamName
                .Should()
                .Be("phoneRepository");
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_Mapper_IsNull()
        {
            Action throwAction = () => new UserService(_userManager, null, null, null);

            throwAction.ShouldThrow<ArgumentNullException>()
                .And
                .ParamName
                .Should()
                .Be("mapper");
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_RoleService_IsNull()
        {
            Action throwAction = () => new UserService(_userManager, _mapperFixture.MapperInstance, null, null);

            throwAction.ShouldThrow<ArgumentNullException>()
                .And
                .ParamName
                .Should()
                .Be("roleService");
        }

        [Fact]
        public void GetUsers_Should_Not_Return_Deleted()
        {
            var user1 = new ApplicationUser
            {
                UserName = "bob@bob.com",
                Id = "abc",
                Deleted = true
            };

            var userResults = new[] { user1 };

            _mockStore.As<IQueryableUserStore<ApplicationUser>>()
                .Setup(_ => _.Users)
                .Returns(userResults.AsQueryable());

            var entityResult = _userService.GetUsers();

            Mock.VerifyAll();
            entityResult.Should().BeEmpty();
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

            var entityResult = _userService.GetUsers();

            Mock.VerifyAll();
            entityResult.Should().HaveSameCount(userResults);

            userResults.All(_ => entityResult.Any(r => r.Username == _.UserName))
               .Should()
               .BeTrue();
        }

        [Fact]
        public async Task Register_Should_Call_UserManager()
        {
            // ARRANGE
            var model = Fixture.Build<RegistrationModel>()
                .With(_ => _.Email, "test@test.com")
                .With(_ => _.Password, "cool1Password!!")
                .Create();

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

            // ACT
            await _userService.RegisterAsync(model);

            // ASSERT
            Mock.VerifyAll();
        }

        [Fact]
        public async Task Register_Should_Return_Error_When_Model_Is_Invalid()
        {
            // ARRANGE
            var model = Fixture.Build<RegistrationModel>()
                .With(_ => _.Email, "testtestcom")
                .With(_ => _.Password, "cool1Password!!")
                .Create();

            _mockStore.As<IUserPasswordStore<ApplicationUser>>()
                .Setup(_ => _.SetPasswordHashAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Returns(Task.Delay(0));

            _mockStore.Setup(_ => _.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser());

            // ACT
            var result = await _userService.RegisterAsync(model);

            // ASSERT
            Mock.VerifyAll();
            result.Succeeded.Should().BeFalse();
        }
    }
}
