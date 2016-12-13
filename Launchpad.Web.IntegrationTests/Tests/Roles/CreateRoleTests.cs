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
    public class CreateRoleTests : ApiTest
    {
        public CreateRoleTests(HostFixture hostFixture) : base(hostFixture)
        {
        }

        public override HttpMethod Method => HttpMethod.Post;

        public override string PathUnderTest => "/api/v1/roles";

        [Fact]
        public async void CreateRole_Should_Return_Status_201_And_Location_Header()
        {
            var roleModel = new RoleModel { Name = DateTime.Now.ToString("s") };

            var httpRequestMessage = CreateSecureRequest(Method, PathUnderTest)
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
    }
}
