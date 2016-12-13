using FluentAssertions;
using Launchpad.Models;
using Launchpad.Web.IntegrationTests.Client;
using Launchpad.Web.IntegrationTests.Extensions;
using Launchpad.Web.IntegrationTests.Hosting;
using System.Linq;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Launchpad.Web.IntegrationTests.Tests.UserRoles
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class GetUserRolesTests : ApiTest
    {
        private readonly UserHelper _userHelper;

        public GetUserRolesTests(HostFixture hostFixture) : base(hostFixture)
        {
            _userHelper = new UserHelper();
        }

        public override HttpMethod Method => HttpMethod.Get;

        public override string PathUnderTest => "/api/v1/users/{0}/roles";

        [Fact]
        public async void GetUserRoles_Should_Return_Status_200()
        {
            // Arrange               
            await _userHelper.Refresh();
            var user = _userHelper.GetAllEntities().First(item => "admin@admin.com".Equals(item.Username));
            var httpRequestMessage = CreateSecureRequest(Method, PathUnderTest, user.Id);

            // ACT
            var response = await Client.SendAsync(httpRequestMessage);

            // ASSERT
            response.Should()
                .HaveStatusCode(HttpStatusCode.OK);

            var roles = await response.ReadBody<RoleModel[]>();
            roles.Should().NotBeNullOrEmpty();
        }
    }
}
