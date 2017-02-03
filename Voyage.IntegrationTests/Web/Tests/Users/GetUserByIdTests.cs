using System;
using System.Net;
using System.Net.Http;

using FluentAssertions;

using Voyage.IntegrationTests.Web.Client;
using Voyage.IntegrationTests.Web.Extensions;
using Voyage.IntegrationTests.Web.Hosting;
using Voyage.Models;

using Xunit;

namespace Voyage.IntegrationTests.Web.Tests.Users
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class GetUserByIdTests : ApiTest
    {
        private readonly UserHelper _userHelper;

        public GetUserByIdTests(HostFixture hostFixture)
            : base(hostFixture)
        {
            _userHelper = new UserHelper();
        }

        public override HttpMethod Method => HttpMethod.Get;

        public override string PathUnderTest => "/api/v1/users/{0}";

        [Fact]
        public async void GetUserById_Should_Return_Status_404_When_Not_Found()
        {
            // Arrange
            var httpRequestMessage = CreateSecureRequest(Method, PathUnderTest, Guid.Empty);

            // ACT
            var response = await Client.SendAsync(httpRequestMessage);

            // ASSERT
            response.Should()
                .HaveStatusCode(HttpStatusCode.NotFound);
        }

        [Fact]
        public async void GetUserById_Should_Return_Status_200()
        {
            // Arrange
            await _userHelper.Refresh();
            var user = _userHelper.GetSingleEntity();
            var httpRequestMessage = CreateSecureRequest(Method, PathUnderTest, user.Id);

            // ACT
            var response = await Client.SendAsync(httpRequestMessage);

            // ASSERT
            response.Should()
                .HaveStatusCode(HttpStatusCode.OK);

            UserModel model = await response.ReadBody<UserModel>();
            model.Should().NotBeNull();
            model.ShouldBeEquivalentTo(user);
        }
    }
}
