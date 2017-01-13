using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using FluentAssertions;

using Launchpad.IntegrationTests.Web.Client;
using Launchpad.IntegrationTests.Web.Extensions;
using Launchpad.IntegrationTests.Web.Hosting;
using Launchpad.Models;

using Xunit;

namespace Launchpad.IntegrationTests.Web.Tests.Users
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class UpdateUserTests : ApiTest
    {
        private readonly UserHelper _userHelper;

        public UpdateUserTests(HostFixture hostFixture) : base(hostFixture)
        {
            _userHelper = new UserHelper();
        }

        public override HttpMethod Method => HttpMethod.Put;

        public override string PathUnderTest => "/api/v1/users/{0}";

        [Fact]
        public async Task UpdateUser_Should_Return_Status_200()
        {
            await _userHelper.Refresh();
            var user = _userHelper.GetSingleEntity();
            user.FirstName = $"UpdatedFirstName{DateTime.Now.Ticks}";

            var request = CreateSecureRequest(Method, PathUnderTest, user.Id).WithJson(user);
            var response = await Client.SendAsync(request);

            response.Should().HaveStatusCode(HttpStatusCode.OK);

            var responseModel = await response.ReadBody<UserModel>();
            responseModel.ShouldBeEquivalentTo(user);
        }
    }
}
