using System;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using Launchpad.Core;
using Launchpad.IntegrationTests.Web.Client;
using Launchpad.IntegrationTests.Web.Extensions;
using Launchpad.IntegrationTests.Web.Hosting;
using Launchpad.Models;
using Xunit;

namespace Launchpad.IntegrationTests.Web.Tests.RoleClaims
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class PostClaimTests : ApiTest
    {
        private readonly RoleHelper _roleHelper;

        public PostClaimTests(HostFixture hostFixture) : base(hostFixture)
        {
            _roleHelper = new RoleHelper();
        }

        public override HttpMethod Method => HttpMethod.Post;

        public override string PathUnderTest => "/api/v1/roles/{0}/claims";

        public static object[] InvalidClaimModels => new object[]
            {
                new object[] { new ClaimModel { ClaimType = "ClaimType" }, "Claim value is a required field", Constants.ErrorCodes.MissingField },
                new object[] { new ClaimModel { ClaimValue = "ClaimValue" }, "Claim type is a required field", Constants.ErrorCodes.MissingField }
            };

        [Theory]
        [MemberData("InvalidClaimModels")]
        public async void CreateClaim_Should_Return_Status_400_When_Bad_Request(ClaimModel model, string expectedFailureDescription, string expectedFailureCode)
        {
            await _roleHelper.Refresh();

            var roleId = _roleHelper.GetSingleEntity().Id;

            var request = CreateSecureRequest(Method, PathUnderTest, roleId).WithJson(model);

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            response.Should().HaveStatusCode(HttpStatusCode.BadRequest);

            var responseModel = await response.ReadBody<RequestErrorModel[]>();
            responseModel.Should()
                .NotBeNullOrEmpty()
                .And
                .ContainErrorFor(expectedFailureDescription, expectedFailureCode);
        }

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
