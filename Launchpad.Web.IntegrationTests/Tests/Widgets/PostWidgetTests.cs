using FluentAssertions;
using Launchpad.Models;
using Launchpad.Web.IntegrationTests.Extensions;
using Launchpad.Web.IntegrationTests.Hosting;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Launchpad.Web.IntegrationTests
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class PostWidgetTests : ApiTest
    {
        public override HttpMethod Method => HttpMethod.Post;

        public override string PathUnderTest => "/api/v1/widgets";

        public PostWidgetTests(HostFixture hostFixture) : base(hostFixture)
        {
        }

        [Fact]
        public async void CreateWidget_Should_Return_Status_400_When_Body_Invalid()
        {
            // ARRANGE
            var model = new WidgetModel();

            var httpRequestMessage = CreateSecureRequest(Method, PathUnderTest)
                                        .WithJson(model);

            // ACT
            var response = await Client.SendAsync(httpRequestMessage);

            // ASSERT
            response.Should()
                .HaveStatusCode(HttpStatusCode.BadRequest);

            var errors = await response.ReadBody<BadRequestErrorModel[]>();
            errors.Should()
                .NotBeNullOrEmpty()
                .And
                .ContainErrorFor("widget.Name", Models.Constants.ErrorCodes.MissingField);
        }

        [Fact]
        public async void CreateWidget_Should_Return_Status_201_And_Location_Header()
        {
            var model = new WidgetModel { Name = "Some test model", Color = "Very green" };
            var httpRequestMessage = CreateSecureRequest(Method, PathUnderTest).WithJson(model);

            // ACT
            var response = await Client.SendAsync(httpRequestMessage);

            // ASSERT
            response.Should()
                .HaveStatusCode(HttpStatusCode.Created)
                .And
                .HaveHeader("location");

            WidgetModel responseModel = await response.ReadBody<WidgetModel>();
            responseModel.Should().NotBeNull();

            response.Should().HaveHeaderValue("Location", GetUrl($"/api/v1/widgets/{responseModel.Id}"));
        }
    }
}
