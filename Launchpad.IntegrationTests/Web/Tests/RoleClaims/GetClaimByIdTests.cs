using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using FluentAssertions;

using Voyage.IntegrationTests.Web.Client;
using Voyage.IntegrationTests.Web.Extensions;
using Voyage.IntegrationTests.Web.Hosting;
using Voyage.Models;

using Xunit;

namespace Voyage.IntegrationTests.Web.Tests.RoleClaims
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class GetClaimByIdTests : ApiTest
    {
        private readonly RoleHelper _roleHelper;

        public GetClaimByIdTests(HostFixture hostFixture)
            : base(hostFixture)
        {
            _roleHelper = new RoleHelper();
        }

        public override HttpMethod Method => HttpMethod.Get;

        public override string PathUnderTest => "/api/v1/roles/{0}/claims/{1}";

        [Fact]
        public async Task GetClaimById_Should_Return_Status_200()
        {
            await _roleHelper.Refresh();

            var targetRole = _roleHelper.GetAllEntities().First(role => role.Claims != null && role.Claims.Count() > 0);
            var targetClaim = targetRole.Claims.First();

            var request = CreateSecureRequest(Method, PathUnderTest, targetRole.Id, targetClaim.Id);
            var response = await Client.SendAsync(request);

            response.Should().HaveStatusCode(HttpStatusCode.OK);

            var responseModel = await response.ReadBody<ClaimModel>();
            responseModel.Should().NotBeNull();
            responseModel.ShouldBeEquivalentTo(targetClaim);
        }

        [Fact]
        public async Task GetClaimById_Should_Return_Status_404_When_Claim_Not_Found()
        {
            await _roleHelper.Refresh();

            var targetRole = _roleHelper.GetAllEntities().First(role => role.Claims != null && role.Claims.Count() > 0);

            var request = CreateSecureRequest(Method, PathUnderTest, Guid.Empty, -1);
            var response = await Client.SendAsync(request);

            response.Should().HaveStatusCode(HttpStatusCode.NotFound);
        }
    }
}
