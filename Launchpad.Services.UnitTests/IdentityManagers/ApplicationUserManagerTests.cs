using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Ploeh.AutoFixture;
using Moq;
using Launchpad.UnitTests.Common;
using Launchpad.Services.IdentityManagers;
using Launchpad.Models.EntityFramework;
using Microsoft.AspNet.Identity;

namespace Launchpad.Services.UnitTests.IdentityManagers
{
    public class ApplicationUserManagerTests : BaseUnitTest
    {
        private ApplicationUserManager _manager;
        private Mock<IUserStore<ApplicationUser>> _mockStore;
        private Mock<IUserTokenProvider<ApplicationUser, string>> _mockProvider;

        public ApplicationUserManagerTests()
        {
            _mockProvider = Mock.Create<IUserTokenProvider<ApplicationUser, string>>();
            _mockStore = Mock.Create<IUserStore<ApplicationUser>>();

            _manager = new ApplicationUserManager(_mockStore.Object, _mockProvider.Object);
        }

        [Fact]
        public void PasswordValidator_Should_Be_Configured_With_Known_Values()
        {
            var validator = _manager.PasswordValidator as PasswordValidator;
            validator.Should().NotBeNull();
            
            validator.RequiredLength.Should().Be(6);
            validator.RequireNonLetterOrDigit.Should().BeTrue();
            validator.RequireDigit.Should().BeTrue();
            validator.RequireLowercase.Should().BeTrue();
            validator.RequireUppercase.Should().BeTrue();

        }

        [Fact]
        public void UserValidator_Should_Be_Configured_With_Known_Values()
        {
            var validator = _manager.UserValidator as UserValidator<ApplicationUser>;

            validator.AllowOnlyAlphanumericUserNames.Should().BeFalse();
            validator.RequireUniqueEmail.Should().BeTrue();

        }

        [Fact]
        public void UserTokenProvider_Should_Be_Injected_Value()
        {
            _manager.UserTokenProvider.Should().Be(_mockProvider.Object);
        }


    }
}
