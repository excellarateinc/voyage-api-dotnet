using System;
using System.Collections.Generic;
using System.Security.Claims;
using Autofac;
using FluentAssertions;
using Voyage.Services.User;
using Voyage.UnitTests.Common;
using Voyage.Web.AuthProviders;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.OAuth.Messages;
using Moq;
using Ploeh.AutoFixture;
using Xunit;

namespace Voyage.UnitTests.Web.AuthProviders
{
    [Trait("Category", "OAuthProvider")]
    public class ApplicationOAuthProviderTests : BaseUnitTest
    {
        private const string PathString = "/api/v1/login";
        private readonly string _clientId;
        private readonly ApplicationOAuthProvider _provider;
        private readonly Mock<IOwinContext> _mockOwinContext;
        private readonly Mock<ILoginOrchestrator> _mockLoginOrchestrator;

        public ApplicationOAuthProviderTests()
        {
            _clientId = Fixture.Create<string>();
            _mockOwinContext = Mock.Create<IOwinContext>();
            _mockLoginOrchestrator = Mock.Create<ILoginOrchestrator>();

            // Setup login delegate
            _mockLoginOrchestrator.Setup(_ => _.TokenPath).Returns(PathString);
            _provider = new ApplicationOAuthProvider(new[] { _mockLoginOrchestrator.Object });
        }

        [Fact]
        public async void MatchEndPoint_Should_Call_MatchesTokenEndpoint_When_LoginOrchestrator_Matches()
        {
              // Setup the request
            var mockRequest = Mock.Create<IOwinRequest>();

            mockRequest.Setup(_ => _.Path)
              .Returns(new PathString(PathString));

            _mockOwinContext.Setup(_ => _.Request)
              .Returns(mockRequest.Object);

            OAuthMatchEndpointContext ctx = new OAuthMatchEndpointContext(_mockOwinContext.Object, new OAuthAuthorizationServerOptions());

            await _provider.MatchEndpoint(ctx);

            ctx.IsTokenEndpoint.Should().BeTrue();
        }

        [Fact]
        public async void MatchEndPoint_Should_Not_Call_MatchesTokenEndpoint_When_LoginOrchestrator_DoesNotMatch()
        {
            // Setup the request
            var mockRequest = Mock.Create<IOwinRequest>();

            mockRequest.Setup(_ => _.Path)
              .Returns(new PathString(PathString + "!"));

            _mockOwinContext.Setup(_ => _.Request)
              .Returns(mockRequest.Object);

            OAuthMatchEndpointContext ctx = new OAuthMatchEndpointContext(_mockOwinContext.Object, new OAuthAuthorizationServerOptions());

            await _provider.MatchEndpoint(ctx);

            ctx.IsTokenEndpoint.Should().BeFalse();
        }

        [Fact]
        public async void ValidateTokenRequest_Should_Call_Validated_When_LoginOrchestrator_Returns_True()
        {
            var pairs = new Dictionary<string, string[]>();
            var readableCollection = new ReadableStringCollection(pairs);
            Mock.Create<BaseValidatingClientContext>();

            var validatingCtx = new OAuthValidateClientAuthenticationContext(
                context: _mockOwinContext.Object,
                options: new OAuthAuthorizationServerOptions(),
                parameters: readableCollection);

            _mockLoginOrchestrator.Setup(_ => _.ValidateRequest(readableCollection))
                .Returns(true);

            // Setup the request
            var mockRequest = Mock.Create<IOwinRequest>();

            mockRequest.Setup(_ => _.Path)
              .Returns(new PathString(PathString));

            _mockOwinContext.Setup(_ => _.Request)
              .Returns(mockRequest.Object);

            var ctx = new OAuthValidateTokenRequestContext(
                context: _mockOwinContext.Object,
                options: new OAuthAuthorizationServerOptions(),
                tokenRequest: new TokenEndpointRequest(readableCollection),
                clientContext: validatingCtx);

            await _provider.ValidateTokenRequest(ctx);

            ctx.HasError.Should().BeFalse();
            ctx.IsValidated.Should().BeTrue();
        }

        [Fact]
        public async void ValidateTokenRequest_Should_Call_SetError_When_LoginOrchestrator_Returns_False()
        {
            var pairs = new Dictionary<string, string[]>();
            var readableCollection = new ReadableStringCollection(pairs);
            Mock.Create<BaseValidatingClientContext>();

            var validatingCtx = new OAuthValidateClientAuthenticationContext(
                context: _mockOwinContext.Object,
                options: new OAuthAuthorizationServerOptions(),
                parameters: readableCollection);

            _mockLoginOrchestrator.Setup(_ => _.ValidateRequest(readableCollection))
                .Returns(false);

            // Setup the request
            var mockRequest = Mock.Create<IOwinRequest>();

            mockRequest.Setup(_ => _.Path)
              .Returns(new PathString(PathString));

            _mockOwinContext.Setup(_ => _.Request)
              .Returns(mockRequest.Object);

            var ctx = new OAuthValidateTokenRequestContext(
                context: _mockOwinContext.Object,
                options: new OAuthAuthorizationServerOptions(),
                tokenRequest: new TokenEndpointRequest(readableCollection),
                clientContext: validatingCtx);

            await _provider.ValidateTokenRequest(ctx);

            ctx.HasError.Should().BeTrue();
            ctx.IsValidated.Should().BeFalse();
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_PublicClientId_Null()
        {
            Action throwAction = () => new ApplicationOAuthProvider(null);
            throwAction.ShouldThrow<ArgumentNullException>()
                .And
                .ParamName
                .Should()
                .Be("publicClientId");
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_LoginOrchestrators_Null()
        {
            Action throwAction = () => new ApplicationOAuthProvider(null);
            throwAction.ShouldThrow<ArgumentNullException>()
                .And
                .ParamName
                .Should()
                .Be("loginOrchestrators");
        }

        [Fact]
        public async void GrantResourceOwnerCredentials_Should_SetError_When_Invalid_User()
        {
            var user = "bob@bob.com";
            var password = "giraffe";
            OAuthGrantResourceOwnerCredentialsContext oAuthContext = new OAuthGrantResourceOwnerCredentialsContext(_mockOwinContext.Object, new OAuthAuthorizationServerOptions(), _clientId, user, password, new List<string>());

            // Setup the request
            var mockRequest = Mock.Create<IOwinRequest>();

            mockRequest.Setup(_ => _.Path)
              .Returns(new PathString(PathString));

            _mockOwinContext.Setup(_ => _.Request)
              .Returns(mockRequest.Object);

            // Setup the login orchestrator
            _mockLoginOrchestrator.Setup(_ => _.ValidateCredential(oAuthContext))
                .ReturnsAsync(false);

            // ACT
            await _provider.GrantResourceOwnerCredentials(oAuthContext);

            // ASSERT
            Mock.VerifyAll();
            oAuthContext.HasError.Should().BeTrue();
        }

        [Fact]
        public async void GrantResourceOwnerCredentials_Should_Call_User_Service()
        {
            var user = "bob@bob.com";
            var password = "giraffe";
            OAuthGrantResourceOwnerCredentialsContext oAuthContext = new OAuthGrantResourceOwnerCredentialsContext(_mockOwinContext.Object, new OAuthAuthorizationServerOptions(), _clientId, user, password, new List<string>());

            // Identity fake
            var identity = new ClaimsIdentity();

            // Setup login
            // Setup the login orchestrator
            _mockLoginOrchestrator.Setup(_ => _.ValidateCredential(oAuthContext))
                .ReturnsAsync(true);

            // Setup the user service
            var mockUserService = Mock.Create<IUserService>();

            mockUserService.Setup(_ => _.CreateClaimsIdentityAsync(user, OAuthDefaults.AuthenticationType))
                .ReturnsAsync(identity);
            mockUserService.Setup(_ => _.CreateClaimsIdentityAsync(user, CookieAuthenticationDefaults.AuthenticationType))
                .ReturnsAsync(identity);

            // Skip mocking out autofac, just build the container to use
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Register(c => mockUserService.Object);
            var container = containerBuilder.Build();

            // Property used for sign in
            var mockAuthenticationManager = Mock.Create<IAuthenticationManager>();
            mockAuthenticationManager.Setup(_ => _.SignIn(identity));
            _mockOwinContext.Setup(_ => _.Authentication).Returns(mockAuthenticationManager.Object);

            // Configure the context properties
            var mockOwinRequest = Mock.Create<IOwinRequest>();
            mockOwinRequest.Setup(_ => _.Path)
                .Returns(new PathString(PathString));

            mockOwinRequest.Setup(_ => _.Context).Returns(_mockOwinContext.Object);
            _mockOwinContext.Setup(_ => _.Request).Returns(mockOwinRequest.Object);
            _mockOwinContext.Setup(_ => _.Get<ILifetimeScope>(It.IsAny<string>()))
                .Returns(container);

            // ACT
            await _provider.GrantResourceOwnerCredentials(oAuthContext);

            // ASSERT
            Mock.VerifyAll();
        }
    }
}
