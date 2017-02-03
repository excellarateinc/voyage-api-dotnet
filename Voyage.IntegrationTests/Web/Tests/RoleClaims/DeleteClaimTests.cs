﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Voyage.IntegrationTests.Web.Client;
using Voyage.IntegrationTests.Web.Extensions;
using Voyage.IntegrationTests.Web.Hosting;
using Voyage.Models;

using Xunit;

namespace Voyage.IntegrationTests.Web.Tests.RoleClaims
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class DeleteClaimTests : ApiTest
    {
        private readonly RoleHelper _roleHelper;

        public DeleteClaimTests(HostFixture hostFixture)
            : base(hostFixture)
        {
            _roleHelper = new RoleHelper();
        }

        public override HttpMethod Method => HttpMethod.Delete;

        public override string PathUnderTest => "/api/v1/roles/{0}/claims/{1}";

        [Fact]
        public async Task DeleteClaim_Should_Return_Status_200()
        {
            // Arrange
            await _roleHelper.Refresh();

            var claimModel = new ClaimModel { ClaimValue = DateTime.Now.ToString("s"), ClaimType = DateTime.Now.ToString("s") };
            var roleId = _roleHelper.GetSingleEntity().Id;
            var createRequest = CreateSecureRequest(HttpMethod.Post, "/api/v1/roles/{0}/claims", roleId).WithJson(claimModel);
            var createResponse = await Client.SendAsync(createRequest);
            var targetClaim = await createResponse.ReadBody<ClaimModel>();

            // Act
            var request = CreateSecureRequest(Method, PathUnderTest, roleId, targetClaim.Id + 1000);
            var response = await Client.SendAsync(request);

            // Assert
            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteClaim_Should_Return_Status_200_When_Claim_Not_Found()
        {
            // Arrange
            await _roleHelper.Refresh();
            var roleId = _roleHelper.GetSingleEntity().Id;

            // Act
            var request = CreateSecureRequest(Method, PathUnderTest, roleId, -1);
            var response = await Client.SendAsync(request);

            // Assert
            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }
    }
}
