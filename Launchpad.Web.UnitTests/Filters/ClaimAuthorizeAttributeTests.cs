using Launchpad.UnitTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using FluentAssertions;
using Ploeh.AutoFixture;
using Xunit;
using Launchpad.Web.Filters;
using System.Web.Http.Controllers;
using System.Security.Claims;
using System.Security.Principal;
using System.Net;

namespace Launchpad.Web.UnitTests.Filters
{
    public class ClaimAuthorizeAttributeTests : BaseUnitTest
    {
     
        [Fact]
        public void ClaimType_Should_Default_To_Constant_Value()
        {
            new ClaimAuthorizeAttribute().ClaimType.Should().Be(Constants.LssClaims.Type);
        }

        [Fact]
        public void OnAuthorize_Should_Not_Set_Response_When_Claim_Exists()
        {
            var attribute = new ClaimAuthorizeAttribute();
            attribute.ClaimValue = "list.widgets";

            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim("lss.permission", "list.widgets")
            }, "Password");
           

            var controllerContext = new HttpControllerContext();
            controllerContext.Request = new System.Net.Http.HttpRequestMessage();
            controllerContext.RequestContext = new HttpRequestContext();
            controllerContext.RequestContext.Principal = new ClaimsPrincipal(claimsIdentity);

            var actionContext = new HttpActionContext(controllerContext, new Mock<HttpActionDescriptor>() { CallBase = true }.Object);
            

            attribute.OnAuthorization(actionContext);

            actionContext.Response.Should().BeNull();
        }

        [Fact]
        public void OnAuthorize_Should_Set_Response_When_Claim_Does_Not_Exist()
        {
            var attribute = new ClaimAuthorizeAttribute();
            attribute.ClaimValue = "create.widgets";

            var claimsIdentity = new ClaimsIdentity(new[]
                {
                new Claim("lss.permission", "list.widgets")
            }, "Password");


            var controllerContext = new HttpControllerContext();
            controllerContext.Request = new System.Net.Http.HttpRequestMessage();
            controllerContext.RequestContext = new HttpRequestContext();
            controllerContext.RequestContext.Principal = new ClaimsPrincipal(claimsIdentity);

            var actionContext = new HttpActionContext(controllerContext, new Mock<HttpActionDescriptor>() { CallBase = true }.Object);


            attribute.OnAuthorization(actionContext);

            actionContext.Response.Should().NotBeNull();
            actionContext.Response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public void OnAuthorize_Should_Set_Response_When_User_Is_Not_Authenticated()
        {

            var attribute = new ClaimAuthorizeAttribute();
            attribute.ClaimValue = "create.widgets";

            var claimsIdentity = new ClaimsIdentity(new[]
                {
                new Claim("lss.permission", "list.widgets")
            });


            var controllerContext = new HttpControllerContext();
            controllerContext.Request = new System.Net.Http.HttpRequestMessage();
            controllerContext.RequestContext = new HttpRequestContext();
            controllerContext.RequestContext.Principal = new ClaimsPrincipal(claimsIdentity);

            var actionContext = new HttpActionContext(controllerContext, new Mock<HttpActionDescriptor>() { CallBase = true }.Object);


            attribute.OnAuthorization(actionContext);

            actionContext.Response.Should().NotBeNull();
            actionContext.Response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async void OnAuthorizationAsync_Should_Not_Set_Response_When_Claim_Exists()
        {
            var attribute = new ClaimAuthorizeAttribute();
            attribute.ClaimValue = "list.widgets";

            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim("lss.permission", "list.widgets")
            }, "Password");


            var controllerContext = new HttpControllerContext();
            controllerContext.Request = new System.Net.Http.HttpRequestMessage();
            controllerContext.RequestContext = new HttpRequestContext();
            controllerContext.RequestContext.Principal = new ClaimsPrincipal(claimsIdentity);

            var actionContext = new HttpActionContext(controllerContext, new Mock<HttpActionDescriptor>() { CallBase = true }.Object);


            await attribute.OnAuthorizationAsync(actionContext, new System.Threading.CancellationToken());

            actionContext.Response.Should().BeNull();
        }

        [Fact]
        public void OnAuthorizeAsync_Should_Set_Response_When_Claim_Does_Not_Exist()
        {
            var attribute = new ClaimAuthorizeAttribute();
            attribute.ClaimValue = "create.widgets";

            var claimsIdentity = new ClaimsIdentity(new[]
                {
                new Claim("lss.permission", "list.widgets")
            }, "Password");


            var controllerContext = new HttpControllerContext();
            controllerContext.Request = new System.Net.Http.HttpRequestMessage();
            controllerContext.RequestContext = new HttpRequestContext();
            controllerContext.RequestContext.Principal = new ClaimsPrincipal(claimsIdentity);

            var actionContext = new HttpActionContext(controllerContext, new Mock<HttpActionDescriptor>() { CallBase = true }.Object);


            attribute.OnAuthorizationAsync(actionContext, new System.Threading.CancellationToken());

            actionContext.Response.Should().NotBeNull();
            actionContext.Response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public void OnAuthorizeAsync_Should_Set_Response_When_User_Is_Not_Authenticated()
        {

            var attribute = new ClaimAuthorizeAttribute();
            attribute.ClaimValue = "create.widgets";

            var claimsIdentity = new ClaimsIdentity(new[]
                {
                new Claim("lss.permission", "list.widgets")
            });


            var controllerContext = new HttpControllerContext();
            controllerContext.Request = new System.Net.Http.HttpRequestMessage();
            controllerContext.RequestContext = new HttpRequestContext();
            controllerContext.RequestContext.Principal = new ClaimsPrincipal(claimsIdentity);

            var actionContext = new HttpActionContext(controllerContext, new Mock<HttpActionDescriptor>() { CallBase = true }.Object);


            attribute.OnAuthorizationAsync(actionContext, new System.Threading.CancellationToken());

            actionContext.Response.Should().NotBeNull();
            actionContext.Response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

    }
}
