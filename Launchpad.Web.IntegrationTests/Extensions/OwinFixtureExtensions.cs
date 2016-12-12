using Launchpad.Models;
using Launchpad.Web.IntegrationTests.Fixture;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Launchpad.Web.IntegrationTests.Extensions
{
    public static class OwinFixtureExtensions
    {
        public static string GetEndpoint(this OwinFixture fixture, string path)
        {
            return $"{fixture.BaseAddress}{path}";
        }

        public static HttpRequestMessage CreateSecureRequest(this OwinFixture fixture, HttpMethod method, string path)
        {
            var message = new HttpRequestMessage(method, fixture.GetEndpoint(path));
            message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", fixture.DefaultToken);
            return message;
        }

        public static HttpRequestMessage CreateUnauthorizedRequest(this OwinFixture fixture, HttpMethod method, string path)
        {
            var message = new HttpRequestMessage(method, fixture.GetEndpoint(path));
            message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Guid.NewGuid().ToString());
            return message;
        }

        public static async Task<string> GetRoleId(this OwinFixture fixture)
        {
            var httpRequestMessage = fixture.CreateSecureRequest(HttpMethod.Get, "/api/v1/roles");
            var response = await fixture.Client.SendAsync(httpRequestMessage);

            response.Should().HaveStatusCode(HttpStatusCode.OK);

            RoleModel[] models = await response.ReadBody<RoleModel[]>();
            return models.First().Id;
        }
    }
}
