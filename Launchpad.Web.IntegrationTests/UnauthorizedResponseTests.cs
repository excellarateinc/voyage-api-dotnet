using Launchpad.Web.IntegrationTests.Extensions;
using Launchpad.Web.IntegrationTests.Hosting;
using System;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Launchpad.Web.IntegrationTests
{
    /// <summary>
    /// Contains tests for the cross-cutting concern of authorization. All 
    /// secure services will require a valid user token. If the token is 
    /// invalid then the response should be 401: Unauthorized
    /// </summary>
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class UnauthorizedResponseTests : ApiTest
    {
        public UnauthorizedResponseTests(HostFixture hostFixture) : base(hostFixture)
        {
        }

        /// <summary>
        /// An array of arrays containing the input method and URL for the test 
        /// </summary>
        public static object[] UnauthorizedUrls =>
           new object[]
           {
               // User endpoints
               new object[] { HttpMethod.Get, "/api/v1/users" },
               new object[] { HttpMethod.Post, "/api/v1/users" },
               new object[] { HttpMethod.Get, $"/api/v1/users/{Guid.Empty}" },
               new object[] { HttpMethod.Put, $"/api/v1/users/{Guid.Empty}" },
               new object[] { HttpMethod.Delete, $"/api/v1/users/{Guid.Empty}" },
               new object[] { HttpMethod.Get, $"/api/v1/users/{Guid.Empty}/roles" },
               new object[] { HttpMethod.Post, $"/api/v1/users/{Guid.Empty}/roles" },
               new object[] { HttpMethod.Delete, $"/api/v1/users/{Guid.Empty}/roles/{Guid.Empty}" },
               new object[] { HttpMethod.Get, $"/api/v1/users/{Guid.Empty}/roles/{Guid.Empty}" },
               new object[] { HttpMethod.Get, $"/api/v1/users/{Guid.Empty}/claims" },

               // Claim endpoints
               new object[] { HttpMethod.Post, $"/api/v1/roles/{Guid.Empty}/claims" },
               new object[] { HttpMethod.Get, $"/api/v1/roles/{Guid.Empty}/claims" },
               new object[] { HttpMethod.Post, $"/api/v1/roles/{Guid.Empty}/claims" },
               new object[] { HttpMethod.Delete, $"/api/v1/roles/{Guid.Empty}/claims/{Guid.Empty}" },

               // Role endpoints
               new object[] { HttpMethod.Get, "/api/v1/roles" },
               new object[] { HttpMethod.Get, $"/api/v1/roles/{Guid.Empty}" },
               new object[] { HttpMethod.Delete, $"/api/v1/roles/{Guid.Empty}" },
               new object[] { HttpMethod.Post, "/api/v1/roles" },

               // Widget endpoints
               new object[] { HttpMethod.Get, "/api/v1/widgets" },
               new object[] { HttpMethod.Get, "/api/v1/widgets/1" },
               new object[] { HttpMethod.Delete, "/api/v1/widgets/1" },
               new object[] { HttpMethod.Post, "/api/v1/widgets" },
               new object[] { HttpMethod.Put, "/api/v1/widgets/1" }
           };

        [Theory]
        [MemberData("UnauthorizedUrls")]
        public async void Endpoint_Should_Respond_With_401_When_Unauthorized(HttpMethod method, string path)
        {
            var request = CreateUnauthorizedRequest(method, path);

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            response.Should().HaveStatusCode(HttpStatusCode.Unauthorized, "{0} {1} is secure", method, path);
        }
    }
}
