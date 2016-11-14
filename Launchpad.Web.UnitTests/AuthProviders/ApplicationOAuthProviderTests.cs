using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using FluentAssertions;
using Xunit;
using Moq;
using Launchpad.UnitTests.Common;
using Launchpad.Web.AuthProviders;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin;
using Launchpad.Services.Interfaces;
using System.Security.Claims;
using Autofac;
using Autofac.Core;
using Microsoft.Owin.Security;

namespace Launchpad.Web.UnitTests.AuthProviders
{
    [Trait("Category", "OAuthProvider")]
    public class ApplicationOAuthProviderTests : BaseUnitTest
    {
        const string _pathString = "/api/v1/login";
        private string _clientId;
        ApplicationOAuthProvider _provider;
        private Mock<IOwinContext> _mockOwinContext;
        private Mock<ILoginOrchestrator> _mockLoginOrchestrator;

        public ApplicationOAuthProviderTests()
        {
            _clientId = Fixture.Create<string>();
            _mockOwinContext = Mock.Create<IOwinContext>();
            _mockLoginOrchestrator = Mock.Create<ILoginOrchestrator>();

            //Setup login delegate
            _mockLoginOrchestrator.Setup(_ => _.TokenPath).Returns(_pathString);
            _provider = new ApplicationOAuthProvider(_clientId, new[] { _mockLoginOrchestrator.Object });
        }

        [Fact]
        public void MatchEndPoint_Should_Call_MatchesTokenEndpoint_When_LoginOrchestrator_Matches()
        {
            //OAuthValidateTokenRequestContext ctx = new OAuthValidateTokenRequestContext(
            //    _mockOwinContext.Object, 
            //    new OAuthAuthorizationServerOptions(), 
            //    new Microsoft.Owin.Security.OAuth.Messages.TokenEndpointRequest(), 
            //    new BaseValidatingClientContext());

            //_provider.MatchEndpoint()
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_PublicClientId_Null()
        {
            Action throwAction = () => new ApplicationOAuthProvider(null, null);
            throwAction.ShouldThrow<ArgumentNullException>()
                .And
                .ParamName
                .Should()
                .Be("publicClientId");
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_LoginOrchestrators_Null()
        {
            Action throwAction = () => new ApplicationOAuthProvider(_clientId, null);
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


            

            //Setup the request
            var mockRequest = Mock.Create<IOwinRequest>();

            mockRequest.Setup(_ => _.Path)
              .Returns(new PathString(_pathString));

            _mockOwinContext.Setup(_ => _.Request)
              .Returns(mockRequest.Object);

            //Setup the login orchestrator
            _mockLoginOrchestrator.Setup(_ => _.ValidateCredential(oAuthContext))
                .ReturnsAsync(false);
                
          

            //ACT
            await _provider.GrantResourceOwnerCredentials(oAuthContext);


            //ASSERT
            Mock.VerifyAll();
            oAuthContext.HasError.Should().BeTrue();
        }

        [Fact]
        public async void GrantResourceOwnerCredentials_Should_Call_User_Service()
        {
            var user = "bob@bob.com";
            var password = "giraffe";
            OAuthGrantResourceOwnerCredentialsContext oAuthContext = new OAuthGrantResourceOwnerCredentialsContext(_mockOwinContext.Object, new OAuthAuthorizationServerOptions(), _clientId, user, password, new List<string>());

            //Identity fake

            var identity = new ClaimsIdentity();

            //Setup login
            //Setup the login orchestrator
            _mockLoginOrchestrator.Setup(_ => _.ValidateCredential(oAuthContext))
                .ReturnsAsync(true);

            //Setup the user service

            var mockUserService = Mock.Create<IUserService>();
         
            mockUserService.Setup(_ => _.CreateClaimsIdentityAsync(user, OAuthDefaults.AuthenticationType))
                .ReturnsAsync(identity);
            mockUserService.Setup(_ => _.CreateClaimsIdentityAsync(user, CookieAuthenticationDefaults.AuthenticationType))
                .ReturnsAsync(identity);


            //Skip mocking out autofac, just build the container to use
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Register(c => mockUserService.Object);
            var container = containerBuilder.Build();


            //Property used for sign in
            var mockAuthenticationManager = Mock.Create<IAuthenticationManager>();
            mockAuthenticationManager.Setup(_ => _.SignIn(identity));
            _mockOwinContext.Setup(_ => _.Authentication).Returns(mockAuthenticationManager.Object);

            //Configure the context properties
            var mockOwinRequest = Mock.Create<IOwinRequest>();
            mockOwinRequest.Setup(_ => _.Path)
                .Returns(new PathString(_pathString));
          
            mockOwinRequest.Setup(_ => _.Context).Returns(_mockOwinContext.Object);
            _mockOwinContext.Setup(_ => _.Request).Returns(mockOwinRequest.Object);    
            _mockOwinContext.Setup(_ => _.Get<ILifetimeScope>(It.IsAny<string>()))
                .Returns(container);

    

            //ACT
            await _provider.GrantResourceOwnerCredentials(oAuthContext);

          
            //ASSERT
            Mock.VerifyAll();
        }

    }
}
