using Autofac;
using FluentAssertions;
using Launchpad.Services.Interfaces;
using Launchpad.UnitTests.Common;
using Launchpad.Web.AuthProviders;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Launchpad.Web.UnitTests.AuthProviders
{
    [Trait("Category", "OAuthProvider")]
    public class LoginOrchestratorTests : BaseUnitTest
    {
        private readonly LoginOrchestrator _orchestrator;
        private readonly Mock<IOwinContext> _mockOwinContext;
        public LoginOrchestratorTests()
        {
            _orchestrator = new LoginOrchestrator();
            _mockOwinContext = Mock.Create<IOwinContext>();
        }

        [Fact]
        public void TokenPath_Should_Be_V1()
        {
            _orchestrator.TokenPath.Should().Be("/api/v1/login");
        }

        [Fact]
        public async void ValidateCredential_Should_ResolveUserService_And_Call_IsValidCredential()
        { 
            //Setup the user service
            var mockUserService = Mock.Create<IUserService>();
            mockUserService.Setup(_ => _.IsValidCredential("userName", "password"))
                .ReturnsAsync(true);
            

            //Skip mocking out autofac, just build the container to use
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Register(c => mockUserService.Object);

            var container = containerBuilder.Build();

            _mockOwinContext.Setup(_ => 
                _.Get<ILifetimeScope>(It.IsAny<string>()))
               .Returns(container);

            var ctx = new OAuthGrantResourceOwnerCredentialsContext(
                _mockOwinContext.Object, 
                new OAuthAuthorizationServerOptions(), 
                "clientId", 
                "userName", 
                "password", 
                new string[0]);

            

            var result = await _orchestrator.ValidateCredential(ctx);

            result.Should().BeTrue();
        }

        [Fact]
        public void Validate_Request_Should_Return_False_When_There_Are_Too_Many_Parameters()
        {
            var pairs = new Dictionary<string, string[]>();
            pairs.Add("grant_type", new[] { "password" });
            pairs.Add("username", new[] { "username" });
            pairs.Add("password", new[] { "password" });
            pairs.Add("foo", new[] { "bar" });

            var readableCollection = new ReadableStringCollection(pairs);

            _orchestrator.ValidateRequest(readableCollection)
                .Should()
                .BeFalse();
        }

        [Fact]
        public void Validate_Request_Should_Return_False_When_There_Are_Too_Few_Parameters()
        {
            var pairs = new Dictionary<string, string[]>();
            pairs.Add("grant_type", new[] { "password" });
            pairs.Add("username", new[] { "username" });
             

            var readableCollection = new ReadableStringCollection(pairs);

            _orchestrator.ValidateRequest(readableCollection)
                .Should()
                .BeFalse();
        }

        [Fact]
        public void Validate_Request_Should_Return_False_When_Missing_Password()
        {
            var pairs = new Dictionary<string, string[]>();
            pairs.Add("grant_type", new[] { "password" });
            pairs.Add("username", new[] { "username" });
            pairs.Add("password1", new[] { "password" });

            var readableCollection = new ReadableStringCollection(pairs);

            _orchestrator.ValidateRequest(readableCollection)
                .Should()
                .BeFalse();
        }

        [Fact]
        public void Validate_Request_Should_Return_False_When_Missing_Username()
        {
            var pairs = new Dictionary<string, string[]>();
            pairs.Add("grant_type", new[] { "password" });
            pairs.Add("username1", new[] { "username" });
            pairs.Add("password", new[] { "password" });

            var readableCollection = new ReadableStringCollection(pairs);

            _orchestrator.ValidateRequest(readableCollection)
                .Should()
                .BeFalse();
        }

        [Fact]
        public void Validate_Request_Should_Return_False_When_Missing_GrantType()
        {
            var pairs = new Dictionary<string, string[]>();
            pairs.Add("grant_type1", new[] { "password" });
            pairs.Add("username", new[] { "username" });
            pairs.Add("password", new[] { "password" });

            var readableCollection = new ReadableStringCollection(pairs);

            _orchestrator.ValidateRequest(readableCollection)
                .Should()
                .BeFalse();
        }

        [Fact]
        public void Validate_Request_Should_Return_False_When_Empty_GrantType()
        {
            var pairs = new Dictionary<string, string[]>();
            pairs.Add("grant_type", new[] { "" });
            pairs.Add("username", new[] { "username" });
            pairs.Add("password", new[] { "password" });

            var readableCollection = new ReadableStringCollection(pairs);

            _orchestrator.ValidateRequest(readableCollection)
                .Should()
                .BeFalse();
        }

        [Fact]
        public void Validate_Request_Should_Return_False_When_Empty_Username()
        {
            var pairs = new Dictionary<string, string[]>();
            pairs.Add("grant_type", new[] { "grant_type" });
            pairs.Add("username", new[] { "" });
            pairs.Add("password", new[] { "password" });

            var readableCollection = new ReadableStringCollection(pairs);

            _orchestrator.ValidateRequest(readableCollection)
                .Should()
                .BeFalse();
        }

        [Fact]
        public void Validate_Request_Should_Return_False_When_Empty_Password()
        {
            var pairs = new Dictionary<string, string[]>();
            pairs.Add("grant_type", new[] { "grant_type" });
            pairs.Add("username", new[] { "username" });
            pairs.Add("password", new[] { "" });

            var readableCollection = new ReadableStringCollection(pairs);

            _orchestrator.ValidateRequest(readableCollection)
                .Should()
                .BeFalse();
        }

        [Fact]
        public void Validate_Request_Should_Return_False_When_Grant_Type_Not_Password()
        {
            var pairs = new Dictionary<string, string[]>();
            pairs.Add("grant_type", new[] { "grant_type" });
            pairs.Add("username", new[] { "username" });
            pairs.Add("password", new[] { "credential" });

            var readableCollection = new ReadableStringCollection(pairs);

            _orchestrator.ValidateRequest(readableCollection)
                .Should()
                .BeFalse();
        }

        [Fact]
        public void Validate_Request_Should_Return_False_When_Username_Too_Long()
        {
            var pairs = new Dictionary<string, string[]>();
            pairs.Add("grant_type", new[] { "grant_type" });
            pairs.Add("username", new[] { new string('a', 51) });
            pairs.Add("password", new[] { "password" });

            var readableCollection = new ReadableStringCollection(pairs);

            _orchestrator.ValidateRequest(readableCollection)
                .Should()
                .BeFalse();
        }

        [Fact]
        public void Validate_Request_Should_Return_False_When_Password_Too_Long()
        {
            var pairs = new Dictionary<string, string[]>();
            pairs.Add("grant_type", new[] { "grant_type" });
            pairs.Add("username", new[] {  "username" });
            pairs.Add("password", new[] { new string('a', 251) });

            var readableCollection = new ReadableStringCollection(pairs);

            _orchestrator.ValidateRequest(readableCollection)
                .Should()
                .BeFalse();
        }

        [Fact]
        public void Validate_Request_Should_Return_True_When_ValidInput()
        {
            var pairs = new Dictionary<string, string[]>();
            pairs.Add("grant_type", new[] { "password" });
            pairs.Add("username", new[] { "username" });
            pairs.Add("password", new[] { "password" });

            var readableCollection = new ReadableStringCollection(pairs);

            _orchestrator.ValidateRequest(readableCollection)
                .Should()
                .BeTrue();
        }



    }
}
