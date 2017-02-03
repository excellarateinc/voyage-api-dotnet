using System;
using System.Security.Claims;
using System.Security.Principal;

using FluentAssertions;

using Voyage.UnitTests.Common;
using Voyage.Web.AuthProviders;

using Microsoft.Owin;
using Microsoft.Owin.Security;

using Moq;

using Xunit;

namespace Voyage.UnitTests.Web.AuthProviders
{
    public class IdentityProviderTests : BaseUnitTest
    {
        private readonly IdentityProvider _provider;
        private readonly Mock<IOwinContext> _mockContext;

        public IdentityProviderTests()
        {
            _mockContext = Mock.Create<IOwinContext>();
            _provider = new IdentityProvider(_mockContext.Object);
        }

        [Fact]
        public void Ctor_Should_Throw_Argument_Null_Exception_When_Context_Null()
        {
            Action throwAction = () => new IdentityProvider(null);
            throwAction.ShouldThrow<ArgumentNullException>()
                .And
                .ParamName
                .Should()
                .Be("context");
        }

        [Fact]
        public void GetUserName_Should_Call_Mock_Context_And_Return_Name()
        {
            // Arrange
            const string name = "admin@admin.com";
            var mockAuthManager = Mock.Create<IAuthenticationManager>();
            _mockContext.Setup(_ => _.Authentication).Returns(mockAuthManager.Object);
            mockAuthManager.Setup(_ => _.User).Returns(new ClaimsPrincipal(new GenericIdentity(name)));

            // Act
            var value = _provider.GetUserName();

            // Assert
            Mock.VerifyAll();
            value.Should().Be(name);
        }
    }
}
