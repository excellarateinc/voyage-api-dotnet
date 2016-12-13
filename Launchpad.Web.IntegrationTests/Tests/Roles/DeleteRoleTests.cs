using FluentAssertions;
using Launchpad.Web.IntegrationTests.Client;
using Launchpad.Web.IntegrationTests.Extensions;
using Launchpad.Web.IntegrationTests.Hosting;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Launchpad.Web.IntegrationTests.Tests.Roles
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
        public async Task DeleteRole_Should_Return_Status_204()
        {
            // Arrange - Create Role to Delete

            var responseModel = await _roleHelper.CreateRoleAsync();

            // Act
            var deleteRequest = CreateSecureRequest(Method, PathUnderTest, responseModel.Id);
            var deleteResponse = await Client.SendAsync(deleteRequest);

            // Assert
            deleteResponse.Should().HaveStatusCode(HttpStatusCode.NoContent);
        }
    }
}
