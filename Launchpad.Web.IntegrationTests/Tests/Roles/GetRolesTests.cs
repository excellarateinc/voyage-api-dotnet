using FluentAssertions;
using Launchpad.Models;
using Launchpad.Web.IntegrationTests.Extensions;
using Launchpad.Web.IntegrationTests.Hosting;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Launchpad.Web.IntegrationTests.Tests.Roles
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class GetRolesTests : ApiTest
    {
        public override HttpMethod Method => HttpMethod.Get;

        public override string PathUnderTest => "/api/v1/roles";

        public GetRolesTests(HostFixture hostFixture) : base(hostFixture)
        {
        }

        [Fact]
        public async void GetRoles_Should_Return_Status_200()
        {
            // ARRANGE
            var httpRequestMessage = CreateSecureRequest(HttpMethod.Get, "/api/v1/roles");

            // ACT
            var response = await Client.SendAsync(httpRequestMessage);

            // ASSERT
            response.Should()
                .HaveStatusCode(HttpStatusCode.OK);
            RoleModel[] models = await response.ReadBody<RoleModel[]>();
            models.Should().NotBeNullOrEmpty();
        }
    }
}
