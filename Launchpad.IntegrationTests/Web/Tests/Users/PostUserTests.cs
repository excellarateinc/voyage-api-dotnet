using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using FluentAssertions;

using Launchpad.IntegrationTests.Web.Extensions;
using Launchpad.IntegrationTests.Web.Hosting;
using Launchpad.Models;

using Xunit;

namespace Launchpad.IntegrationTests.Web.Tests.Users
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class PostUserTests : ApiTest
    {
        public PostUserTests(HostFixture hostFixture)
            : base(hostFixture)
        {
        }

        public override HttpMethod Method => HttpMethod.Post;

        public override string PathUnderTest => "/api/v1/users";

        [Fact]
        public async Task CreateUser_Should_Return_Status_201_With_Location_Header()
        {
            var userModel = new UserModel
            {
                Username = $"createUser{DateTime.Now.Ticks}@test.com",
                Email = $"createUser{DateTime.Now.Ticks}@test.com",
                FirstName = "Theodore",
                LastName = "TestsALot",
                IsActive = true
            };

            var request = CreateSecureRequest(Method, PathUnderTest).WithJson(userModel);
            var response = await Client.SendAsync(request);

            response.Should().HaveStatusCode(HttpStatusCode.Created)
                .And
                .HaveHeader("Location");

            var responseModel = await response.ReadBody<UserModel>();
            responseModel.ShouldBeEquivalentTo(userModel, opt => opt.Excluding(u => u.Id).Excluding(u => u.Phones));
            response.Should()
                .HaveHeaderValue("Location", GetUrl($"/api/v1/users/{responseModel.Id}"));
        }
    }
}
