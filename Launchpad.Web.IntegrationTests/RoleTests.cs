using FluentAssertions;
using Launchpad.Models;
using Launchpad.Web.IntegrationTests.Client;
using Launchpad.Web.IntegrationTests.Extensions;
using Launchpad.Web.IntegrationTests.Hosting;
using System;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Launchpad.Web.IntegrationTests
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class RoleTests : ApiTest
    {
        private readonly RoleHelper _roleHelper;

        public RoleTests(HostFixture hostFixture) : base(hostFixture)
        {
            _roleHelper = new RoleHelper();
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
            var deleteRequest = CreateSecureRequest(HttpMethod.Delete, $"/api/v1/roles/{responseModel.Id}");
            var deleteResponse = await Client.SendAsync(deleteRequest);

            // Assert
            deleteResponse.Should().HaveStatusCode(HttpStatusCode.NoContent);
        }

        [Fact]
        public async void CreateClaim_Should_Return_Status_201_And_Location_Header()
        {
            await _roleHelper.Refresh();

            var claimModel = new ClaimModel { ClaimValue = DateTime.Now.ToString("s"), ClaimType = DateTime.Now.ToString("s") };
            var roleId = _roleHelper.GetSingleEntity().Id;

            var request = CreateSecureRequest(HttpMethod.Post, $"/api/v1/roles/{roleId}/claims").WithJson(claimModel);

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            response.Should().HaveStatusCode(HttpStatusCode.Created)
                .And
                .HaveHeader("Location");

            var responseModel = await response.ReadBody<ClaimModel>();
            responseModel.Should().NotBeNull();
            responseModel.ClaimType.Should().Be(claimModel.ClaimType);
            responseModel.ClaimType.Should().Be(claimModel.ClaimValue);

            var expectedLocationValue = GetUrl($"/api/v1/roles/{roleId}/claims/{responseModel.Id}");
            response.Should().HaveHeaderValue("Location", expectedLocationValue);
        }

        [Fact]
        public async void CreateRole_Should_Return_Status_201_And_Location_Header()
        {
            var roleModel = new RoleModel { Name = DateTime.Now.ToString("s") };

            var httpRequestMessage = CreateSecureRequest(HttpMethod.Post, $"/api/v1/roles")
                .WithJson(roleModel);

            var httpResponseMessage = await Client.SendAsync(httpRequestMessage);

            httpResponseMessage.Should()
                .HaveStatusCode(HttpStatusCode.Created)
                .And
                .HaveHeader("Location");

            var responseModel = await httpResponseMessage.ReadBody<RoleModel>();
            responseModel.Name.Should().Be(roleModel.Name);

            httpResponseMessage.Should()
                .HaveHeaderValue("Location", GetUrl($"/api/v1/roles/{responseModel.Id}"));
        }

        [Fact]
        public async void GetRoleById_Should_Return_Status_404_When_Id_Not_Found()
        {
            var httpRequestMessage = CreateSecureRequest(HttpMethod.Get, $"/api/v1/roles/{Guid.Empty}");

            // ACT
            var response = await Client.SendAsync(httpRequestMessage);

            response.Should()
                .HaveStatusCode(HttpStatusCode.NotFound);
        }

        [Fact]
        public async void GetRoleById_Should_Return_Status_200()
        {
            await _roleHelper.Refresh();

            // Arrange               
            var roleId = _roleHelper.GetSingleEntity().Id;
            var httpRequestMessage = CreateSecureRequest(HttpMethod.Get, $"/api/v1/roles/{roleId}");

            // ACT
            var response = await Client.SendAsync(httpRequestMessage);

            // ASSERT
            response.Should()
                .HaveStatusCode(HttpStatusCode.OK);

            RoleModel model = await response.ReadBody<RoleModel>();
            model.Should().NotBeNull();
            model.Id.Should().Be(roleId);
        }

        [Fact]
        public async void GetRoles_Should_Return_Status_200()
        {
            // ARRANGE
            var httpRequestMessage = CreateSecureRequest(HttpMethod.Get, "/api/v1/roles");

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
