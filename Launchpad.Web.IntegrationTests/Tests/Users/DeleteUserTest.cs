using FluentAssertions;
using Launchpad.Web.IntegrationTests.Client;
using Launchpad.Web.IntegrationTests.Extensions;
using Launchpad.Web.IntegrationTests.Hosting;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Launchpad.Web.IntegrationTests.Tests.Users
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class DeleteUserTest : ApiTest
    {
        private readonly UserHelper _userHelper;

        public DeleteUserTest(HostFixture hostFixture) : base(hostFixture)
        {
            _userHelper = new UserHelper();
        }

        public override HttpMethod Method => HttpMethod.Delete;

        public override string PathUnderTest => "/api/v1/users/{0}";

        [Fact]
        public async Task DeleteUser_Should_Return_Status_204()
        {
            // Arrange
            var user = await _userHelper.CreateUserAsync();

            // Act
            var request = CreateSecureRequest(Method, PathUnderTest, user.Id);
            var response = await Client.SendAsync(request);

            // Assert
            response.Should().HaveStatusCode(HttpStatusCode.NoContent);
        }
    }
}
