using FluentAssertions;
using Launchpad.Web.IntegrationTests.Client;
using Launchpad.Web.IntegrationTests.Extensions;
using Launchpad.Web.IntegrationTests.Hosting;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Launchpad.Web.IntegrationTests.Tests.Widgets
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class DeleteWidgetTests : ApiTest
    {
        private readonly WidgetHelper _widgetHelper;

        public DeleteWidgetTests(HostFixture hostFixture) : base(hostFixture)
        {
            _widgetHelper = new WidgetHelper();
        }

        public override HttpMethod Method => HttpMethod.Delete;

        public override string PathUnderTest => "/api/v1/widgets/{0}";

        [Fact]
        public async Task DeleteWidget_Should_Return_Status_204()
        {
            var widget = await _widgetHelper.CreateWidgetAsync();

            var request = CreateSecureRequest(Method, PathUnderTest, widget.Id);
            var response = await Client.SendAsync(request);

            response.Should().HaveStatusCode(HttpStatusCode.NoContent);
        }
    }
}
