using FluentAssertions;
using Launchpad.Models;
using Launchpad.Web.IntegrationTests.Extensions;
using Launchpad.Web.IntegrationTests.Hosting;
using System.Linq;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Launchpad.Web.IntegrationTests
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class WidgetTests : ApiTest
    {
        public WidgetTests(HostFixture hostFixture) : base(hostFixture)
        {
        }

        [Fact]
        public async void CreateWidget_Should_Return_400_When_Body_Invalid()
        {
            // ARRANGE
            var model = new WidgetModel();

            var httpRequestMessage = CreateSecureRequest(HttpMethod.Post, "/api/v1/widgets")
                                        .WithJson(model);

            // ACT
            var response = await Client.SendAsync(httpRequestMessage);

            // ASSERT
            response.Should()
                .HaveStatusCode(HttpStatusCode.BadRequest);

            var errors = await response.ReadBody<BadRequestErrorModel[]>();
            errors.Should().NotBeNullOrEmpty();
            errors.Any(error => error.Code == Models.Constants.ErrorCodes.MissingField && error.Field == "widget.Name").Should().BeTrue();
        }

        [Fact]
        public async void CreateWidget_Should_Return_201()
        {
            var model = new WidgetModel { Name = "Some test model", Color = "Very green" };
            var httpRequestMessage = CreateSecureRequest(HttpMethod.Post, "/api/v1/widgets").WithJson(model);

            // ACT
            var response = await Client.SendAsync(httpRequestMessage);

            // ASSERT
            response.Should()
                .HaveStatusCode(HttpStatusCode.Created)
                .And
                .HaveHeader("location");

            WidgetModel responseModel = await response.ReadBody<WidgetModel>();
            responseModel.Should().NotBeNull();
        }

        [Fact]
        public async void GetWidgets_Should_Return_Models()
        {
            // ARRANGE
            var httpRequestMessage = CreateSecureRequest(HttpMethod.Get, "/api/v1/widgets");

            // ACT
            var response = await Client.SendAsync(httpRequestMessage);

            // ASSERT
            response.Should()
                .HaveStatusCode(HttpStatusCode.OK);

            WidgetModel[] models = await response.ReadBody<WidgetModel[]>();
            models.Should().NotBeNullOrEmpty();
        }
    }
}
