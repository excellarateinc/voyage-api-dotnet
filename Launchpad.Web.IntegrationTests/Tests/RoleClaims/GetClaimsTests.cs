using FluentAssertions;
using Launchpad.Models;
using Launchpad.Web.IntegrationTests.Client;
using Launchpad.Web.IntegrationTests.Extensions;
using Launchpad.Web.IntegrationTests.Hosting;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Launchpad.Web.IntegrationTests.Tests.RoleClaims
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class GetClaimsTests : ApiTest
    {
        private readonly RoleHelper _roleHelper;

        public GetClaimsTests(HostFixture hostFixture) : base(hostFixture)
        {
            _roleHelper = new RoleHelper();
        }

        public override HttpMethod Method => HttpMethod.Get;

        public override string PathUnderTest => "/api/v1/roles/{0}/claims";

        [Fact]
        public async Task GetClaims_Should_Return_Status_200()
        {
            // ARRANGE
            await _roleHelper.Refresh();

            var targetRole = _roleHelper.GetAllEntities().First(role => role.Claims != null && role.Claims.Count() > 0);
            var httpRequestMessage = CreateSecureRequest(Method, PathUnderTest, targetRole.Id);

            // ACT
            var response = await Client.SendAsync(httpRequestMessage);

            // ASSERT
            response.Should()
                .HaveStatusCode(HttpStatusCode.OK);
            ClaimModel[] models = await response.ReadBody<ClaimModel[]>();
            models.ShouldAllBeEquivalentTo(targetRole.Claims);
        }
    }
}
