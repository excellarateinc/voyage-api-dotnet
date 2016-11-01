﻿using System;
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

namespace Launchpad.Services.UnitTests
{
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

        public async void AssignUserRoleAsync_Should_Call_User_Manager_Return_IdentityResult_Success_When_Sucessful()
        {
            //arrange

            var userModel = Fixture.Build<UserModel>()
                .With(_ => _.Name, "bob@bob.com")
                .Create(); ;

        

            var roleModel = Fixture.Create<RoleModel>();
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

         

            //act
            var result = await _userService.AssignUserRoleAsync(roleModel, userModel);


            //assert
            Mock.VerifyAll();
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();

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

            var model = Fixture.Build<RegistrationModel>()
                .With(_ => _.Email, "test@test.com")
                .With(_ => _.Password, "cool1Password!!")
                .Create();

            var identityResult = new IdentityResult();

            _mockStore.As<IUserPasswordStore<ApplicationUser>>()
                .Setup(_ => _.SetPasswordHashAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
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