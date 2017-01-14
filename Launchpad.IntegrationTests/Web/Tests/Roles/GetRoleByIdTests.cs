using System;
using System.Net;
using System.Net.Http;

using FluentAssertions;

using Launchpad.IntegrationTests.Web.Client;
using Launchpad.IntegrationTests.Web.Extensions;
using Launchpad.IntegrationTests.Web.Hosting;
using Launchpad.Models;

using Xunit;

namespace Launchpad.IntegrationTests.Web.Tests.Roles
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class GetRoleByIdTests : ApiTest
    {
        public override HttpMethod Method => HttpMethod.Get;

        public override string PathUnderTest => "/api/v1/roles/{0}";

        private readonly RoleHelper _roleHelper;

        public GetRoleByIdTests(HostFixture hostFixture) : base(hostFixture)
        {
            _roleHelper = new RoleHelper();
        }

        [Fact]
        public async void GetRoleById_Should_Return_Status_404_When_Id_Not_Found()
        {
            var httpRequestMessage = CreateSecureRequest(Method, PathUnderTest, Guid.Empty);

            // ACT
            var response = await Client.SendAsync(httpRequestMessage);

            response.Should()
                .HaveStatusCode(HttpStatusCode.NotFound);
        }

        [Fact]
        public async void GetRoleById_Should_Return_Status_200()
        {
            // Arrange               
            await _roleHelper.Refresh();
            var roleId = _roleHelper.GetSingleEntity().Id;
            var httpRequestMessage = CreateSecureRequest(Method, PathUnderTest, roleId);

            // ACT
            var response = await Client.SendAsync(httpRequestMessage);

            // ASSERT
            response.Should()
                .HaveStatusCode(HttpStatusCode.OK);

            RoleModel model = await response.ReadBody<RoleModel>();
            model.Should().NotBeNull();
            model.Id.Should().Be(roleId);
        }
    }
}
