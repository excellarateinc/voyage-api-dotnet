using FluentAssertions;
using Launchpad.Models;
using Launchpad.Web.IntegrationTests.Extensions;
using Launchpad.Web.IntegrationTests.Fixture;
using System;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Launchpad.Web.IntegrationTests
{
    [Trait("Category", "Self-Hosted")]
    [Collection(OwinCollectionFixture.Name)]
    public class RoleTests : BaseEndpointTest
    {
        public RoleTests(OwinFixture owin) : base(owin)
        {
        }

        [Fact]
        public async void CreateRole_Should_Return_201_And_Location_Header()
        {
            using (var instance = OwinFixture.Start())
            {
                await OwinFixture.Init();

                var roleModel = new RoleModel { Name = DateTime.Now.ToString("s") };

                var httpRequestMessage = OwinFixture.CreateSecureRequest(HttpMethod.Post, $"/api/v1/roles")
                    .WithJson(roleModel);

                var httpResponseMessage = await OwinFixture.Client.SendAsync(httpRequestMessage);

                httpResponseMessage.Should()
                    .HaveStatusCode(HttpStatusCode.Created)
                    .And
                    .HaveHeader("Location");


                var responseModel = await httpResponseMessage.ReadBody<RoleModel>();
                responseModel.Name.Should().Be(roleModel.Name);

                httpResponseMessage.Should()
                    .HaveHeaderValue("Location", $"{OwinFixture.BaseAddress}/api/v1/roles/{responseModel.Id}");
            }
        }

        [Fact]
        public async void GetRoleById_Should_Return_404_When_Id_Not_Found()
        {
            using (var instance = OwinFixture.Start())
            {
                await OwinFixture.Init();
                var httpRequestMessage = OwinFixture.CreateSecureRequest(HttpMethod.Get, $"/api/v1/roles/{Guid.Empty}");

                // ACT
                var response = await OwinFixture.Client.SendAsync(httpRequestMessage);

                response.Should()
                    .HaveStatusCode(HttpStatusCode.NotFound);
            }
        }

        [Fact]
        public async void GetRoleById_Should_Return_Status_200()
        {
            using (var instance = OwinFixture.Start())
            {
                await OwinFixture.Init();

                // Arrange               
                var roleId = await OwinFixture.GetRoleId();
                var httpRequestMessage = OwinFixture.CreateSecureRequest(HttpMethod.Get, $"/api/v1/roles/{roleId}");

                // ACT
                var response = await OwinFixture.Client.SendAsync(httpRequestMessage);

                // ASSERT
                response.Should()
                    .HaveStatusCode(HttpStatusCode.OK);

                RoleModel model = await response.ReadBody<RoleModel>();
                model.Should().NotBeNull();
                model.Id.Should().Be(roleId);
            }
        }

        [Fact]
        public async void GetRoles_Should_Return_Models()
        {
            using (var instance = OwinFixture.Start())
            {
                await OwinFixture.Init();

                // ARRANGE
                var httpRequestMessage = OwinFixture.CreateSecureRequest(HttpMethod.Get, "/api/v1/roles");

                // ACT
                var response = await OwinFixture.Client.SendAsync(httpRequestMessage);

                // ASSERT
                response.Should()
                    .HaveStatusCode(HttpStatusCode.OK);
                RoleModel[] models = await response.ReadBody<RoleModel[]>();
                models.Should().NotBeNullOrEmpty();
            }
        }
    }
}
