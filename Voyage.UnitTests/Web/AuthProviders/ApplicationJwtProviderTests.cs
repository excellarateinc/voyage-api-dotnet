using System.Collections.Generic;
using System.Security.Claims;
using Autofac;
using Voyage.Services.User;
using Voyage.UnitTests.Common;
using Voyage.Web.AuthProviders;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Moq;
using Ploeh.AutoFixture;
using Xunit;

namespace Voyage.UnitTests.Web.AuthProviders
{
    [Trait("Category", "JwtProvider")]
    public class ApplicationJwtProviderTests : BaseUnitTest
    {
        private readonly string _clientId;
        private readonly ApplicationJwtProvider _provider;
        private readonly Mock<IOwinContext> _mockOwinContext;

        public ApplicationJwtProviderTests()
        {
            _clientId = Fixture.Create<string>();
            _mockOwinContext = Mock.Create<IOwinContext>();
            _provider = new ApplicationJwtProvider();
        }

        [Fact]
        public async void GrantResourceOwnerCredentials_Should_SetError_When_Invalid_User()
        {
            OAuthGrantClientCredentialsContext oAuthContext = new OAuthGrantClientCredentialsContext(_mockOwinContext.Object, new OAuthAuthorizationServerOptions(), _clientId, new List<string>());

            // Setup the user service
            var mockUserService = Mock.Create<IUserService>();
            mockUserService.Setup(x => x.CreateClientClaimsIdentityAsync(It.IsAny<string>())).Returns(new ClaimsIdentity());

            // Skip mocking out autofac, just build the container to use
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Register(c => mockUserService.Object);
            var container = containerBuilder.Build();
            _mockOwinContext.Setup(_ => _.Get<ILifetimeScope>(It.IsAny<string>())).Returns(container);

            // ACT
            await _provider.GrantClientCredentials(oAuthContext);

            // ASSERT
            mockUserService.Verify();
        }
    }
}
