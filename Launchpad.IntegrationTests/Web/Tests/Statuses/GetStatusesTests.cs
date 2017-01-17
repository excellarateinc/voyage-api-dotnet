using System.Net;
using System.Net.Http;

using FluentAssertions;

using Launchpad.IntegrationTests.Web.Extensions;
using Launchpad.IntegrationTests.Web.Hosting;
using Launchpad.Models;

using Xunit;

namespace Launchpad.IntegrationTests.Web.Tests.Statuses
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class GetStatusesTests : ApiTest
    {
        public override HttpMethod Method => HttpMethod.Get;

        public override string PathUnderTest => "/api/v1/statuses";

        public GetStatusesTests(HostFixture hostFixture)
            : base(hostFixture)
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
