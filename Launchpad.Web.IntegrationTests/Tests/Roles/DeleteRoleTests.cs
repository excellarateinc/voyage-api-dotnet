using FluentAssertions;
using Launchpad.Models;
using Launchpad.Web.IntegrationTests.Extensions;
using Launchpad.Web.IntegrationTests.Hosting;
using System;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Launchpad.Web.IntegrationTests.Tests.Roles
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class DeleteRoleTests : ApiTest
    {
        public DeleteRoleTests(HostFixture hostFixture) : base(hostFixture)
        {
        }

        public override HttpMethod Method => HttpMethod.Delete;

        public override string PathUnderTest => "/api/v1/roles/{0}";

        [Fact]
        public async void DeleteRole_Should_Return_Status_404_When_Not_Found()
        {
            // Arrange
            var deleteRequest = CreateSecureRequest(Method, PathUnderTest, Guid.Empty);

            // Act
            var deleteResponse = await Client.SendAsync(deleteRequest);

            // Assert
            deleteResponse.Should().HaveStatusCode(HttpStatusCode.NotFound);
        }

        [Fact]
        public async void DeleteRole_Should_Return_Status_204()
        {
            // Arrange - Create Role to Delete
            var roleModel = new RoleModel { Name = DateTime.Now.ToString("s") };

            var httpRequestMessage = CreateSecureRequest(HttpMethod.Post, $"/api/v1/roles")
                .WithJson(roleModel);
            var httpResponseMessage = await Client.SendAsync(httpRequestMessage);
            var responseModel = await httpResponseMessage.ReadBody<RoleModel>();

            // Act
            var deleteRequest = CreateSecureRequest(Method, PathUnderTest, responseModel.Id);
            var deleteResponse = await Client.SendAsync(deleteRequest);

            // Assert
            deleteResponse.Should().HaveStatusCode(HttpStatusCode.NoContent);
        }
    }
}
