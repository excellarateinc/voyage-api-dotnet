using FluentAssertions;
using Launchpad.Models;
using Launchpad.Web.IntegrationTests.Extensions;
using Launchpad.Web.IntegrationTests.Fixture;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
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
        public async void CreateWidget_Should_Return_201()
        {
            using (var instance = OwinFixture.Start())
            {
                await OwinFixture.Init();
                //ARRANGE
                var httpRequestMessage = CreateSecureRequest(HttpMethod.Post, "/api/v1/widgets");
                var model = new WidgetModel { Name = "Some test model", Color = "Very green" };
                httpRequestMessage.Content = new ObjectContent<WidgetModel>(model, new JsonMediaTypeFormatter());


                //ACT
                var response = await OwinFixture.DefaultClient.SendAsync(httpRequestMessage);


                //ASSERT
                WidgetModel responseModel = await response.ShouldHaveStatusAndPayload<WidgetModel>(HttpStatusCode.Created);
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
                var httpRequestMessage = CreateSecureRequest(HttpMethod.Get, "/api/v1/widgets");

                //ACT
                var response = await OwinFixture.DefaultClient.SendAsync(httpRequestMessage);

                //ASSERT
                WidgetModel[] models = await response.ShouldHaveStatusAndPayload<WidgetModel[]>(HttpStatusCode.OK);
                models.Should().NotBeNullOrEmpty();
            }

        }
    }
}
