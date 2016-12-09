using FluentAssertions;
using Launchpad.Models;
using Launchpad.Web.IntegrationTests.Extensions;
using Launchpad.Web.IntegrationTests.Fixture;
using System.Net;
using Xunit;

namespace Launchpad.Web.IntegrationTests
{
    [Trait("Category", "Self-Hosted")]
    [Collection(OwinCollectionFixture.Name)]
    public class StatusTests : BaseEndpointTest
    {
        public StatusTests(OwinFixture owin) : base(owin)
        {
        }

        [Fact]
        public async void GetRoles_Should_Return_Models()
        {
            using (var instance = OwinFixture.Start())
            {
                var response = await OwinFixture
                                    .DefaultClient
                                    .GetAsync(GetEndpoint("/api/v1/statuses"));

                //ASSERT
                ApplicationInfoModel model = await response.ShouldHaveStatusAndPayload<ApplicationInfoModel>(HttpStatusCode.OK);
                model.Should().NotBeNull();
            }
        }
    }
}
