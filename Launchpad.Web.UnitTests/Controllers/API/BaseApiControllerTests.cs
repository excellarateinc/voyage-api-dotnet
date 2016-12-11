using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Routing;
using FluentAssertions;
using Launchpad.Models;
using Launchpad.UnitTests.Common;
using Launchpad.Web.Controllers.API;
using Moq;
using Ploeh.AutoFixture;
using Xunit;

namespace Launchpad.Web.UnitTests.Controllers.API
{
    public class BaseApiControllerTests : BaseUnitTest
    {
        [RoutePrefix("test")]
        public class TestPassThrough : BaseApiController
        {
            [HttpGet]
            [Route("roles/{id}", Name = "GetRoleById")]
            public IHttpActionResult Get(string id)
            {
                return Ok("ABC");
            }

            public IHttpActionResult InvokeCreatedEntityAt<TModel>(string routeName, Func<object> routeValue, EntityResult<TModel> entityResult)
                      where TModel : class
            {
                return CreatedEntityAt(routeName, routeValue, entityResult);
            }

            public IHttpActionResult InvokeCheckErrorResult(EntityResult entityResult)
            {
                return CheckErrorResult(entityResult);
            }

            public IHttpActionResult InvokeCreateModelResult<TModel>(EntityResult<TModel> entityResult)
            where TModel : class
            {
                return CreateModelResult(entityResult);
            }

            public IHttpActionResult InvokeNoContent(EntityResult entityResult)
            {
                return NoContent(entityResult);
            }
        }

        private readonly TestPassThrough _testPassThrough;
        private readonly Mock<UrlHelper> _mockUrlHelper;

        public BaseApiControllerTests()
        {
            _mockUrlHelper = Mock.Create<UrlHelper>();

            _testPassThrough = new TestPassThrough
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            _testPassThrough.Configuration.MapHttpAttributeRoutes();
            _testPassThrough.Configuration.EnsureInitialized();

            //Add mock URL processor 
            _mockUrlHelper = Mock.Create<UrlHelper>();
            _testPassThrough.Url = _mockUrlHelper.Object;
        }

        [Fact]
        public async void NoContent_Should_Return_204_When_No_Error()
        {
            //Arrange
            var entityResult = new EntityResult(true, false);

            //Act
            var result = _testPassThrough.InvokeNoContent(entityResult);

            //Assert
            var message = await result.ExecuteAsync(CancellationToken.None);

            message.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async void NoContent_Should_Return_404_When_MissingEntity_Flag_Set()
        {
            //Arrange
            var entityResult = new EntityResult(false, true, "err1");

            //Act
            var result = _testPassThrough.InvokeNoContent(entityResult);


            //Assert
            var message = await result.ExecuteAsync(CancellationToken.None);

            message.StatusCode.Should().Be(HttpStatusCode.NotFound);

            List<BadRequestErrorModel> messageModel;
            message.TryGetContentValue(out messageModel).Should().BeTrue();
            messageModel.Should()
                .HaveCount(1);

            messageModel.First()
                .Description
                .Should()
                .Be("err1");
        }

        [Fact]
        public async void NoContent_Should_Return_400_When_Succeeded_False_And_MissingEntity_Flag_Not_Set()
        {
            //Arrange

            var entityResult = new EntityResult(false, false, "err1");

            //Act
            var result = _testPassThrough.InvokeNoContent(entityResult);

            //Assert
            var message = await result.ExecuteAsync(CancellationToken.None);

            message.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            List<BadRequestErrorModel> messageModel;
            message.TryGetContentValue(out messageModel).Should().BeTrue();
            messageModel.Should()
                .HaveCount(1);

            messageModel.First()
                .Description
                .Should()
                .Be("err1");
        }

        [Fact]
        public async void CreatedEntityAt_Should_Return_200_When_No_Error()
        {
            //Arrange
            var id = Fixture.Create<string>();

            var widget = new WidgetModel();

            var entityResult = new EntityResult<WidgetModel>(widget, true, false);

            _mockUrlHelper.Setup(_ => _.Link("GetRoleById", It.IsAny<Dictionary<string, object>>()))
               .Returns("http://testlink.com");

            //Act
            var result = _testPassThrough.InvokeCreatedEntityAt("GetRoleById", () => new { Id = id }, entityResult);

            //Assert
            var message = await result.ExecuteAsync(CancellationToken.None);

            message.StatusCode
                .Should()
                .Be(HttpStatusCode.Created);

            message.Headers
                .Location
                .Should()
                .NotBeNull()
                .And
                .Be("http://testlink.com");

            WidgetModel messageModel;
            message.TryGetContentValue(out messageModel)
                .Should()
                .BeTrue();

            messageModel.ShouldBeEquivalentTo(widget);
        }

        [Fact]
        public async void CreatedEntityAt_Should_Return_404_When_MissingEntity_Flag_Set()
        {
            //Arrange
            var entityResult = new EntityResult<WidgetModel>(null, false, true, "err1");

            var result = _testPassThrough.InvokeCreatedEntityAt("route", () => new { }, entityResult);

            //Act
            var message = await result.ExecuteAsync(CancellationToken.None);

            //Assert
            message.StatusCode.Should().Be(HttpStatusCode.NotFound);

            List<BadRequestErrorModel> messageModel;
            message.TryGetContentValue(out messageModel).Should().BeTrue();
            messageModel.Should()
                .HaveCount(1);

            messageModel.First()
                .Description
                .Should()
                .Be("err1");
        }

        [Fact]
        public async void CreatedEntityAt_Should_Return_400_When_Succeeded_False_And_MissingEntity_Flag_Not_Set()
        {
            //Arrange
            var entityResult = new EntityResult<WidgetModel>(null, false, false, "err1");

            var result = _testPassThrough.InvokeCreatedEntityAt("route", () => new { }, entityResult);

            //Act
            var message = await result.ExecuteAsync(CancellationToken.None);

            //Assert
            message.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            List<BadRequestErrorModel> messageModel;

            message.TryGetContentValue(out messageModel)
                .Should()
                .BeTrue();

            messageModel.Should()
                .HaveCount(1);

            messageModel.First()
                .Description
                .Should()
                .Be("err1");
        }

        [Fact]
        public async void CreateModelResult_Should_Return_200_When_No_Error()
        {
            //Arrange
            var widget = new WidgetModel();

            var entityResult = new EntityResult<WidgetModel>(widget, true, false);

            //Act
            var result = _testPassThrough.InvokeCreateModelResult(entityResult);

            //Assert
            var message = await result.ExecuteAsync(CancellationToken.None);

            message.StatusCode.Should().Be(HttpStatusCode.OK);
            WidgetModel messageModel;
            message.TryGetContentValue(out messageModel).Should().BeTrue();
            messageModel.ShouldBeEquivalentTo(widget);
        }

        [Fact]
        public async void CreateModelResult_Should_Return_404_When_MissingEntity_Flag_Set()
        {
            //Arrange
            var entityResult = new EntityResult<WidgetModel>(null, false, true, "err1");

            //Act
            var result = _testPassThrough.InvokeCreateModelResult(entityResult);

            //Assert
            var message = await result.ExecuteAsync(CancellationToken.None);

            message.StatusCode.Should().Be(HttpStatusCode.NotFound);

            List<BadRequestErrorModel> messageModel;
            message.TryGetContentValue(out messageModel)
                .Should()
                .BeTrue();

            messageModel.Should()
                .HaveCount(1);

            messageModel.First()
                .Description
                .Should()
                .Be("err1");
        }

        [Fact]
        public async void CreateModelResult_Should_Return_400_When_Succeeded_False_And_MissingEntity_Flag_Not_Set()
        {
            //Arrange
            var entityResult = new EntityResult<WidgetModel>(null, false, false, "err1");

            //Act
            var result = _testPassThrough.InvokeCreateModelResult(entityResult);

            //Assert
            var message = await result.ExecuteAsync(CancellationToken.None);

            message.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            List<BadRequestErrorModel> messageModel;
            message.TryGetContentValue(out messageModel).Should().BeTrue();
            messageModel.Should()
                .HaveCount(1);

            messageModel.First()
                .Description
                .Should()
                .Be("err1");
        }

        [Fact]
        public async void CheckErrorResult_Should_Return_404_When_MissingEntity_Flag_Set()
        {
            //Arrange
            var entityResult = new EntityResult(false, true, "err1");

            //Act
            var result = _testPassThrough.InvokeCheckErrorResult(entityResult);

            //Assert
            var message = await result.ExecuteAsync(CancellationToken.None);

            message.StatusCode.Should().Be(HttpStatusCode.NotFound);

            List<BadRequestErrorModel> messageModel;
            message.TryGetContentValue(out messageModel).Should().BeTrue();
            messageModel.Should()
                .HaveCount(1);

            messageModel.First()
                .Description
                .Should()
                .Be("err1");
        }

        [Fact]
        public async void CheckErrorResult_Should_Return_400_When_Succeed_False_MissingEntity_False()
        {
            //Arrange
            var entityResult = new EntityResult(false, false, "err1");

            //Act
            var result = _testPassThrough.InvokeCheckErrorResult(entityResult);

            //Assert
            var message = await result.ExecuteAsync(CancellationToken.None);

            message.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            List<BadRequestErrorModel> messageModel;
            message.TryGetContentValue(out messageModel).Should().BeTrue();
            messageModel.Should()
                .HaveCount(1);

            messageModel.First()
                .Description
                .Should()
                .Be("err1");
        }

        [Fact]
        public void CheckErrorResult_Returns_Null_When_EntityResult_Succeeded()
        {
            //Arrange
            var entityResult = new EntityResult(true, false);

            //Act
            var result = _testPassThrough.InvokeCheckErrorResult(entityResult);

            //Assert
            result.Should().BeNull();
        }
    }
}
