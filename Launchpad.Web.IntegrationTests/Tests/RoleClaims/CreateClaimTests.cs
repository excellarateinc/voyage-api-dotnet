using FluentAssertions;
using Launchpad.Models;
using Launchpad.Web.IntegrationTests.Client;
using Launchpad.Web.IntegrationTests.Extensions;
using Launchpad.Web.IntegrationTests.Hosting;
using System;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Launchpad.Web.IntegrationTests.Tests.RoleClaims
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class CreateClaimTests : ApiTest
    {
        private readonly RoleHelper _roleHelper;

        public CreateClaimTests(HostFixture hostFixture) : base(hostFixture)
        {
            _roleHelper = new RoleHelper();
        }

        public override HttpMethod Method => HttpMethod.Post;

        public override string PathUnderTest => "/api/v1/roles/{0}/claims";

        [Fact]
        public async void CreateClaim_Should_Return_Status_201_And_Location_Header()
        {
            await _roleHelper.Refresh();

            var claimModel = new ClaimModel { ClaimValue = DateTime.Now.ToString("s"), ClaimType = DateTime.Now.ToString("s") };
            var roleId = _roleHelper.GetSingleEntity().Id;

            var request = CreateSecureRequest(Method, PathUnderTest, roleId).WithJson(claimModel);

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
    }
}
