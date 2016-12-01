using FluentAssertions;
using Launchpad.Models;
using Launchpad.Services.Interfaces;
using Launchpad.UnitTests.Common;
using Launchpad.Web.Controllers.API.V1;
using Moq;
using System;
using System.Web.Http;
using System.Web.Http.Results;
using Xunit;

namespace Launchpad.Web.UnitTests.Controllers.API.V1
{
    public class ApplicationInfoControllerTests : BaseUnitTest
    {
        private ApplicationInfoController _controller;
        private Mock<IApplicationInfoService> _mockApplicationInfoService;

        public ApplicationInfoControllerTests()
        {
            _mockApplicationInfoService = Mock.Create<IApplicationInfoService>();

            _controller = new ApplicationInfoController(_mockApplicationInfoService.Object);
            _controller.Request = new System.Net.Http.HttpRequestMessage();
            _controller.Configuration = new System.Web.Http.HttpConfiguration();
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_when_ApplicationInfoService_Is_Null()
        {
            Action throwAction = () => new ApplicationInfoController(null);

            throwAction.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("applicationInfoService");
        }

        [Fact]
        public void Get_Should_Call_Status()
        {
            //Arrange

            _mockApplicationInfoService.Setup(_ => _.GetApplicationInfo())
                .Returns(new ApplicationInfoModel() { BuildNumber = "some_number" });

            //Act

            var result = _controller.Get();

            //Assert

            result.Should().BeOfType<OkNegotiatedContentResult<ApplicationInfoModel>>();

            var resultContent = result.As<OkNegotiatedContentResult<ApplicationInfoModel>>();
            resultContent.Content.BuildNumber.Should().Be("some_number");

            Mock.VerifyAll();
        }

        [Fact]
        public void Class_Should_Have_V1RoutePrefix_Attribute()
        {
            typeof(ApplicationInfoController).Should()
                .BeDecoratedWith<RoutePrefixAttribute>(
                _ => _.Prefix.Equals(Constants.RoutePrefixes.V1));
        }

        [Fact]
        public void Get_Should_Have_Route_Attribute()
        {
            ReflectionHelper.GetMethod<ApplicationInfoController>(_ => _.Get())
             .Should()
             .BeDecoratedWith<RouteAttribute>(_ => _.Template.Equals("statuses"));
        }
    }
}
