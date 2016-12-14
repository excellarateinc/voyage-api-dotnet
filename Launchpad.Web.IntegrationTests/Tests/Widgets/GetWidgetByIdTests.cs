using FluentAssertions;
using Launchpad.Models;
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
    public class GetWidgetByIdTests : ApiTest
    {
        private readonly WidgetHelper _widgetHelper;

        public GetWidgetByIdTests(HostFixture hostFixture) : base(hostFixture)
        {
            _widgetHelper = new WidgetHelper();
        }

        public override HttpMethod Method => HttpMethod.Get;

        public override string PathUnderTest => "/api/v1/widgets/{0}";

        [Fact]
        public async Task GetWidgetById_Should_Return_Status_200()
        {
            await _widgetHelper.Refresh();
            var widget = _widgetHelper.GetSingleEntity();

            var request = CreateSecureRequest(Method, PathUnderTest, widget.Id);
            var response = await Client.SendAsync(request);

            response.Should().HaveStatusCode(HttpStatusCode.OK);
            var responseModel = await response.ReadBody<WidgetModel>();
            responseModel.ShouldBeEquivalentTo(widget);
        }
    }
}
