using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;
using FluentAssertions;
using Microsoft.Owin.Security;
using Moq;
using Ploeh.AutoFixture;
using Voyage.Api.API.V1;
using Voyage.Api.UserManager;
using Voyage.Api.UnitTests.Common;
using Voyage.Models;
using Voyage.Services.Profile;
using Voyage.Services.User;
using Xunit;

namespace Voyage.Api.UnitTests.API.V1
{
    public class ProfilesControllerTests : BaseUnitTest
    {
        private readonly ProfilesController _profilesController;
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IProfileService> _mockProfileService;
        private readonly Mock<IAuthenticationManager> _mockAuthenticationManager;
        private readonly Mock<UrlHelper> _mockUrlHelper;

        public ProfilesControllerTests()
        {

            _mockUserService = Mock.Create<IUserService>();
            _mockUrlHelper = Mock.Create<UrlHelper>();
            _mockProfileService = Mock.Create<IProfileService>();
            _mockAuthenticationManager = Mock.Create<IAuthenticationManager>();
            _profilesController = new ProfilesController(_mockAuthenticationManager.Object, _mockProfileService.Object, _mockUserService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
                Url = _mockUrlHelper.Object
            };
        }

        [Fact]
        public async Task Register_Should_Call_UserManager()
        {
            // ARRANGE
            var model = Fixture.Build<RegistrationModel>()
                .With(_ => _.Email, "test@test.com")
                .With(_ => _.Password, "cool1Password!!")
                .Create();

            _mockUserService.Setup(_ => _.RegisterAsync(model))
                .ReturnsAsync(new UserModel());

            _mockUrlHelper.Setup(_ => _.Link("GetUserAsync", It.IsAny<Dictionary<string, object>>()))
                .Returns("http://testlink.com");

            _mockProfileService.Setup(_ => _.GetInitialProfileImageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(0));

            // ACT
            var result = await _profilesController.Register(model);

            var message = await result.ExecuteAsync(new CancellationToken());

            message.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_UserService_IsNull()
        {
            Action throwAction = () => new ProfilesController(_mockAuthenticationManager.Object, _mockProfileService.Object, null);

            throwAction.ShouldThrow<ArgumentNullException>()
                .And
                .ParamName
                .Should()
                .Be("userService");
        }

        [Fact]
        public void Class_Should_Have_RoutePrefix_Attribute()
        {
            typeof(ProfilesController).Should()
                .BeDecoratedWith<RoutePrefixAttribute>(
                _ => _.Prefix.Equals(RoutePrefixConstants.RoutePrefixes.V1));
        }

        [Fact]
        public void Register_Should_Have_Route_Attribute()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            ReflectionHelper.GetMethod<ProfilesController>(_ => _.Register(new RegistrationModel()))
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                .Should()
                .BeDecoratedWith<RouteAttribute>(_ => _.Template.Equals("profiles/register"));
        }
    }
}
