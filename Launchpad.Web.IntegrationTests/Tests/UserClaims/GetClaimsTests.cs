﻿using FluentAssertions;
using Launchpad.Models;
using Launchpad.Web.IntegrationTests.Client;
using Launchpad.Web.IntegrationTests.Extensions;
using Launchpad.Web.IntegrationTests.Hosting;
using System.Linq;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Launchpad.Web.IntegrationTests.Tests.UserClaims
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class GetClaimsTests : ApiTest
    {
        private readonly UserHelper _userHelper;

        public GetClaimsTests(HostFixture hostFixture) : base(hostFixture)
        {
            _userHelper = new UserHelper();
        }

        public override HttpMethod Method => HttpMethod.Get;

        public override string PathUnderTest => "/api/v1/users/{0}/claims";

        [Fact]
        public async void GetUserClaims_Should_Return_Status_200()
        {
            // Arrange               
            await _userHelper.Refresh();
            var user = _userHelper.GetAllEntities().First(item => "admin@admin.com".Equals(item.Username));
            var httpRequestMessage = CreateSecureRequest(Method, PathUnderTest, user.Id);

            // ACT
            var response = await Client.SendAsync(httpRequestMessage);

            // ASSERT
            response.Should()
                .HaveStatusCode(HttpStatusCode.OK);

            var userClaims = await response.ReadBody<ClaimModel[]>();
            userClaims.Should().NotBeNullOrEmpty();
        }
    }
}