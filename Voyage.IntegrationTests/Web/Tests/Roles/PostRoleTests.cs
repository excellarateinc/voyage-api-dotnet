﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Voyage.Core;
using Voyage.IntegrationTests.Web.Client;
using Voyage.IntegrationTests.Web.Extensions;
using Voyage.IntegrationTests.Web.Hosting;
using Voyage.Models;
using Xunit;

namespace Voyage.IntegrationTests.Web.Tests.Roles
{
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class PostRoleTests : ApiTest
    {
        private readonly RoleHelper _roleHelper;

        public PostRoleTests(HostFixture hostFixture)
            : base(hostFixture)
        {
            _roleHelper = new RoleHelper();
        }

        public override HttpMethod Method => HttpMethod.Post;

        public override string PathUnderTest => "/api/v1/roles";

        [Fact]
        public async Task CreateRole_Should_Return_Status_400_When_Bad_Request()
        {
            var roleModel = new RoleModel();

            var httpRequestMessage = CreateSecureRequest(Method, PathUnderTest)
                .WithJson(roleModel);

            var httpResponseMessage = await Client.SendAsync(httpRequestMessage);

            httpResponseMessage.Should()
                .HaveStatusCode(HttpStatusCode.BadRequest);

            var responseModel = await httpResponseMessage.ReadBody<ResponseErrorModel[]>();
            responseModel.Should()
                .NotBeNullOrEmpty()
                .And
                .ContainErrorFor("Name is a required field", Constants.ErrorCodes.MissingField);
        }

        [Fact]
        public async Task CreateRole_Should_Return_Status_400_When_Duplicate_Name()
        {
            await _roleHelper.Refresh();

            var roleName = _roleHelper.GetSingleEntity().Name;
            var roleModel = new RoleModel { Name = roleName };
            var httpRequestMessage = CreateSecureRequest(Method, PathUnderTest).WithJson(roleModel);

            var httpResponseMessage = await Client.SendAsync(httpRequestMessage);

            httpResponseMessage.Should()
                .HaveStatusCode(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateRole_Should_Return_Status_201_And_Location_Header()
        {
            var roleModel = new RoleModel { Name = Guid.NewGuid().ToString() };

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
