using FluentAssertions;
using Launchpad.Models;
using Launchpad.Web.IntegrationTests.Extensions;
using Launchpad.Web.IntegrationTests.Hosting;
using System.Net;
using Xunit;

namespace Launchpad.Web.IntegrationTests
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class StatusTests : ApiTest
    {
        public StatusTests(HostFixture hostFixture) : base(hostFixture)
        {
        }

        [Fact]
        public async void GetStatuses_Should_Return_Models()
        {
            var response = await Client.GetAsync(GetUrl("/api/v1/statuses"));

            // ASSERT
            response.Should()
                .HaveStatusCode(HttpStatusCode.OK);

            ApplicationInfoModel model = await response.ReadBody<ApplicationInfoModel>();
            model.Should().NotBeNull();
        }
    }
}
