using System.Net;
using System.Net.Http;

using FluentAssertions;

using Voyage.IntegrationTests.Web.Extensions;
using Voyage.IntegrationTests.Web.Hosting;
using Voyage.Models;

using Xunit;

namespace Voyage.IntegrationTests.Web.Tests.Users
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class GetUsersTests : ApiTest
    {
        public GetUsersTests(HostFixture hostFixture)
            : base(hostFixture)
        {
        }

        public override HttpMethod Method => HttpMethod.Get;

        public override string PathUnderTest => "/api/v1/users";

        [Fact]
        public async void GetUsers_Should_Return_Status_200()
        {
            // ARRANGE
            var httpRequestMessage = CreateSecureRequest(Method, PathUnderTest);

            // ACT
            var response = await Client.SendAsync(httpRequestMessage);

            // ASSERT
            response.Should()
                .HaveStatusCode(HttpStatusCode.OK);
            UserModel[] models = await response.ReadBody<UserModel[]>();
            models.Should().NotBeNullOrEmpty();
        }
    }
}
