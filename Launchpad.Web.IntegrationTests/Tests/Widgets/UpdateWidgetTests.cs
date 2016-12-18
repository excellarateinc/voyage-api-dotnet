using FluentAssertions;
using Launchpad.Models;
using Launchpad.Web.IntegrationTests.Client;
using Launchpad.Web.IntegrationTests.Extensions;
using Launchpad.Web.IntegrationTests.Hosting;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Launchpad.Web.IntegrationTests.Tests.Widgets
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class UpdateWidgetTests : ApiTest
    {
        private readonly WidgetHelper _widgetHelper;

        public UpdateWidgetTests(HostFixture hostFixture) : base(hostFixture)
        {
            _widgetHelper = new WidgetHelper();
        }

        public override HttpMethod Method => HttpMethod.Put;

        public override string PathUnderTest => "/api/v1/widgets/{0}";

        [Fact]
        public async Task UpdateWidget_Should_Return_Status_200()
        {
            await _widgetHelper.Refresh();
            var widget = _widgetHelper.GetSingleEntity();
            widget.Color = DateTime.Now.Ticks.ToString();

            var request = CreateSecureRequest(Method, PathUnderTest, widget.Id).WithJson(widget);
            var response = await Client.SendAsync(request);

            response.Should().HaveStatusCode(HttpStatusCode.OK);
            var responseBody = await response.ReadBody<WidgetModel>();
            responseBody.ShouldBeEquivalentTo(widget);
        }
    }
}
