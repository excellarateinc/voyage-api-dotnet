using FluentAssertions;
using Launchpad.Models;
using Launchpad.Web.IntegrationTests.Extensions;
using Launchpad.Web.IntegrationTests.Fixture;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Launchpad.Web.IntegrationTests
{
    [Trait("Category", "Self-Hosted")]
    [Collection(OwinCollectionFixture.Name)]
    public class RoleTests : BaseEndpointTest
    {
        public RoleTests(OwinFixture owin) : base(owin)
        {
        }

        [Fact]
        public async void GetRoles_Should_Return_Models()
        {
            //ARRANGE
            var httpRequestMessage = CreateSecureRequest(HttpMethod.Get, "/api/v1/roles");

            //ACT
            var response = await OwinFixture.DefaultClient.SendAsync(httpRequestMessage);

            //ASSERT
            RoleModel[] models = await response.ShouldHaveStatusAndPayload<RoleModel[]>(HttpStatusCode.OK);
            models.Should().NotBeNullOrEmpty();
        }
    }
}
