using Xunit;
using System.Net.Http;
using FluentAssertions;
using Launchpad.Web.IntegrationTests.Fixture;
using System.Net;
using Launchpad.Models;
using Launchpad.Web.IntegrationTests.Extensions;


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
        public async void GetWidgets_Should_Return_Models()
        {
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
