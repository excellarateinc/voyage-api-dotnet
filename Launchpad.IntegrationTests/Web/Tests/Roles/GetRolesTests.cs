using System.Net;
using System.Net.Http;

using FluentAssertions;

using Voyage.IntegrationTests.Web.Extensions;
using Voyage.IntegrationTests.Web.Hosting;
using Voyage.Models;

using Xunit;

namespace Voyage.IntegrationTests.Web.Tests.Roles
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class GetRolesTests : ApiTest
    {
        public override HttpMethod Method => HttpMethod.Get;

        public override string PathUnderTest => "/api/v1/roles";

        public GetRolesTests(HostFixture hostFixture)
            : base(hostFixture)
        {
        }

        [Fact]
        public async void GetRoles_Should_Return_Status_200()
        {
            // ARRANGE
            var httpRequestMessage = CreateSecureRequest(Method, PathUnderTest);

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
