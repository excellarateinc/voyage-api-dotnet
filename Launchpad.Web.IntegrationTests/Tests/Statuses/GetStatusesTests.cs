using FluentAssertions;
using Launchpad.Models;
using Launchpad.Web.IntegrationTests.Extensions;
using Launchpad.Web.IntegrationTests.Hosting;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Launchpad.Web.IntegrationTests.Tests.Statuses
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class GetStatusesTests : ApiTest
    {
        public override HttpMethod Method => HttpMethod.Get;

        public override string PathUnderTest => "/api/v1/statuses";

        public GetStatusesTests(HostFixture hostFixture) : base(hostFixture)
        {
        }

        [Fact]
        public async void GetStatuses_Should_Return_Models()
        {
            var response = await Client.GetAsync(GetUrl(PathUnderTest));

            // ASSERT
            response.Should()
                .HaveStatusCode(HttpStatusCode.OK);

            ApplicationInfoModel model = await response.ReadBody<ApplicationInfoModel>();
            model.Should().NotBeNull();
        }
    }
}
