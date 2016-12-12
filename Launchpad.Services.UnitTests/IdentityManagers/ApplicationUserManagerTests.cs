using FluentAssertions;
using Launchpad.Models.EntityFramework;
using Launchpad.Services.IdentityManagers;
using Launchpad.UnitTests.Common;
using Microsoft.AspNet.Identity;
using Moq;
using Xunit;

namespace Launchpad.Services.UnitTests.IdentityManagers
{
    public class ApplicationUserManagerTests : BaseUnitTest
    {
        private readonly ApplicationUserManager _manager;
        private readonly Mock<IUserTokenProvider<ApplicationUser, string>> _mockProvider;

        public ApplicationUserManagerTests()
        {
            _mockProvider = Mock.Create<IUserTokenProvider<ApplicationUser, string>>();
            var mockStore = Mock.Create<IUserStore<ApplicationUser>>();

            _manager = new ApplicationUserManager(mockStore.Object, _mockProvider.Object);
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
