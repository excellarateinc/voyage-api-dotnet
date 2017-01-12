using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Launchpad.IntegrationTests.Web.Client;
using Launchpad.IntegrationTests.Web.Extensions;
using Launchpad.IntegrationTests.Web.Hosting;

using Xunit;

namespace Launchpad.IntegrationTests.Web.Tests.Roles
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class DeleteRoleTests : ApiTest
    {
        private RoleHelper _roleHelper;

        public DeleteRoleTests(HostFixture hostFixture) : base(hostFixture)
        {
            _roleHelper = new RoleHelper();
        }

        public override HttpMethod Method => HttpMethod.Delete;

        public override string PathUnderTest => "/api/v1/roles/{0}";

        [Fact]
        public async Task DeleteRole_Should_Return_Status_404_When_Not_Found()
        {
            // Arrange
            var deleteRequest = CreateSecureRequest(Method, PathUnderTest, Guid.Empty);

            // Act
            var deleteResponse = await Client.SendAsync(deleteRequest);

            // Assert
            deleteResponse.Should().HaveStatusCode(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteRole_Should_Return_Status_200()
        {
            // Arrange - Create Role to Delete

            var responseModel = await _roleHelper.CreateRoleAsync();

            // Act
            var deleteRequest = CreateSecureRequest(Method, PathUnderTest, responseModel.Id);
            var deleteResponse = await Client.SendAsync(deleteRequest);

            // Assert
            deleteResponse.Should().HaveStatusCode(HttpStatusCode.OK);
        }
    }
}
