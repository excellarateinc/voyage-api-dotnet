using FluentAssertions;
using Launchpad.Models;
using Launchpad.Web.IntegrationTests.Extensions;
using Launchpad.Web.IntegrationTests.Fixture;
using System.Linq;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Launchpad.Web.IntegrationTests
{  
    [Trait("Category", "Self-Hosted")]
    [Collection(OwinCollectionFixture.Name)]
    public class WidgetTests : BaseEndpointTest
    {     
        public WidgetTests(OwinFixture owin) : base(owin)
        {
        }

        [Fact]
        public async void CreateWidget_Should_Return_400_When_Body_Invalid()
        {
            using (var instance = OwinFixture.Start())
            {
                await OwinFixture.Init();
                //ARRANGE
                var model = new WidgetModel();

                var httpRequestMessage = OwinFixture
                                            .CreateSecureRequest(HttpMethod.Post, "/api/v1/widgets")
                                            .WithJson(model);

                //ACT
                var response = await OwinFixture.Client.SendAsync(httpRequestMessage);


                //ASSERT
                response.Should()
                    .HaveStatusCode(HttpStatusCode.BadRequest);

                var errors = await response.ReadBody<BadRequestErrorModel[]>();
                errors.Should().NotBeNullOrEmpty();
                errors.Any(error => error.Code == Models.Constants.ErrorCodes.MissingField && error.Field == "widget.Name").Should().BeTrue();

            }
        }

        [Fact]
        public async void CreateWidget_Should_Return_201()
        {
            using (var instance = OwinFixture.Start())
            {
                //ARRANGE
                await OwinFixture.Init();
                var model = new WidgetModel { Name = "Some test model", Color = "Very green" };
                var httpRequestMessage = OwinFixture.CreateSecureRequest(HttpMethod.Post, "/api/v1/widgets").WithJson(model);


                //ACT
                var response = await OwinFixture.Client.SendAsync(httpRequestMessage);


                //ASSERT
                response.Should()
                    .HaveStatusCode(HttpStatusCode.Created)
                    .And
                    .HaveHeader("location");

                WidgetModel responseModel = await response.ReadBody<WidgetModel>();
                responseModel.Should().NotBeNull();
            }
        }

        [Fact()]
        public async void GetWidgets_Should_Return_Models()
        {
            using (var instance = OwinFixture.Start())
            {
                await OwinFixture.Init();
                //ARRANGE
                var httpRequestMessage = OwinFixture.CreateSecureRequest(HttpMethod.Get, "/api/v1/widgets");

                //ACT
                var response = await OwinFixture.Client.SendAsync(httpRequestMessage);

                //ASSERT
                response.Should()
                    .HaveStatusCode(HttpStatusCode.OK);

                WidgetModel[] models = await response.ReadBody<WidgetModel[]>();
                models.Should().NotBeNullOrEmpty();
            }

        }
    }
}
