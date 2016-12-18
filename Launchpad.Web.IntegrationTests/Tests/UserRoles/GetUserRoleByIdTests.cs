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

namespace Launchpad.Web.IntegrationTests.Tests.UserRoles
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class GetUserRoleByIdTests : ApiTest
    {
        private readonly UserHelper _userHelper;
        private readonly RoleHelper _roleHelper;

        public GetUserRoleByIdTests(HostFixture hostFixture) : base(hostFixture)
        {
            _userHelper = new UserHelper();
            _roleHelper = new RoleHelper();
        }

        public override HttpMethod Method => HttpMethod.Get;

        public override string PathUnderTest => "/api/v1/users/{0}/roles/{1}";

        [Fact]
        public async Task GetUserRoleById_Should_Return_Status_200()
        {
            // Arrange               
            await _userHelper.Refresh();
            await _roleHelper.Refresh();

            var user = _userHelper.GetAllEntities().First(item => "admin@admin.com".Equals(item.Username));
            var role = _roleHelper.GetAllEntities().First(item => "Administrator".Equals(item.Name));

            var request = CreateSecureRequest(Method, PathUnderTest, user.Id, role.Id);
            var response = await Client.SendAsync(request);

            response.Should().HaveStatusCode(HttpStatusCode.OK);

            var responseModel = await response.ReadBody<RoleModel>();
            responseModel.ShouldBeEquivalentTo(role);
        }
    }
}
