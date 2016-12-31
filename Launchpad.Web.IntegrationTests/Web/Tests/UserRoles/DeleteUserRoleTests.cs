using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Launchpad.IntegrationTests.Web.Client;
using Launchpad.IntegrationTests.Web.Extensions;
using Launchpad.IntegrationTests.Web.Hosting;

using Xunit;

namespace Launchpad.IntegrationTests.Web.Tests.UserRoles
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class DeleteUserRoleTests : ApiTest
    {
        private readonly UserHelper _userHelper;
        private readonly RoleHelper _roleHelper;

        public DeleteUserRoleTests(HostFixture hostFixture) : base(hostFixture)
        {
            _userHelper = new UserHelper();
            _roleHelper = new RoleHelper();
        }

        public override HttpMethod Method => HttpMethod.Delete;

        public override string PathUnderTest => "/api/v1/users/{0}/roles/{1}";

        [Fact]
        public async Task RemoveRole_Should_Return_Status_200()
        {
            await _userHelper.Refresh();
            var role = await _roleHelper.CreateRoleAsync();
            var user = _userHelper.GetSingleEntity();

            var createRequest = CreateSecureRequest(HttpMethod.Post, "/api/v1/users/{0}/roles", user.Id).WithJson(role);
            var createResponse = await Client.SendAsync(createRequest);
            createResponse.Should().HaveStatusCode(HttpStatusCode.Created);

            var request = CreateSecureRequest(Method, PathUnderTest, user.Id, role.Id);
            var response = await Client.SendAsync(request);

            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }
    }
}
