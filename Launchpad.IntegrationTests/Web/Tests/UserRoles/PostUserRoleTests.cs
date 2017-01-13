using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using FluentAssertions;

using Launchpad.IntegrationTests.Web.Client;
using Launchpad.IntegrationTests.Web.Extensions;
using Launchpad.IntegrationTests.Web.Hosting;
using Launchpad.Models;

using Xunit;

namespace Launchpad.IntegrationTests.Web.Tests.UserRoles
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class PostUserRoleTests : ApiTest
    {
        private readonly UserHelper _userHelper;
        private readonly RoleHelper _roleHelper;

        public PostUserRoleTests(HostFixture hostFixture) : base(hostFixture)
        {
            _userHelper = new UserHelper();
            _roleHelper = new RoleHelper();
        }

        public override HttpMethod Method => HttpMethod.Post;

        public override string PathUnderTest => "/api/v1/users/{0}/roles";

        [Fact]
        public async Task AssignRole_Should_Return_Status_201_With_Location_Header()
        {
            await _userHelper.Refresh();
            var role = await _roleHelper.CreateRoleAsync();
            var user = _userHelper.GetSingleEntity();

            var request = CreateSecureRequest(Method, PathUnderTest, user.Id).WithJson(role);
            var response = await Client.SendAsync(request);

            response.Should().HaveStatusCode(HttpStatusCode.Created)
                .And
                .HaveHeader("Location");

            var responseModel = await response.ReadBody<RoleModel>();
            responseModel.ShouldBeEquivalentTo(role);

            response.Should()
                .HaveHeaderValue("Location", GetUrl($"/api/v1/users/{user.Id}/roles/{role.Id}"));
        }
    }
}
