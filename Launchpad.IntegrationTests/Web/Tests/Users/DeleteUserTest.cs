using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Launchpad.IntegrationTests.Web.Client;
using Launchpad.IntegrationTests.Web.Extensions;
using Launchpad.IntegrationTests.Web.Hosting;

using Xunit;

namespace Launchpad.IntegrationTests.Web.Tests.Users
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class DeleteUserTest : ApiTest
    {
        private readonly UserHelper _userHelper;

        public DeleteUserTest(HostFixture hostFixture)
            : base(hostFixture)
        {
            _userHelper = new UserHelper();
        }

        public override HttpMethod Method => HttpMethod.Delete;

        public override string PathUnderTest => "/api/v1/users/{0}";

        [Fact]
        public async Task DeleteUser_Should_Return_Status_200()
        {
            // Arrange
            var user = await _userHelper.CreateUserAsync();

            // Act
            var request = CreateSecureRequest(Method, PathUnderTest, user.Id);
            var response = await Client.SendAsync(request);

            // Assert
            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }
    }
}
