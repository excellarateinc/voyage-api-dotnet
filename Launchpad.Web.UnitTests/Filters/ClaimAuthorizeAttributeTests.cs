using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web.Http.Controllers;
using FluentAssertions;
using Launchpad.UnitTests.Common;
using Launchpad.Web.Filters;
using Moq;
using Xunit;

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
            var attribute = new ClaimAuthorizeAttribute { ClaimValue = "list.widgets" };

            var claimsIdentity = new ClaimsIdentity(new[] { new Claim("lss.permission", "list.widgets") }, "Password");

            var controllerContext = new HttpControllerContext
            {
                Request = new HttpRequestMessage(),
                RequestContext =
                    new HttpRequestContext
                        {
                            Principal = new ClaimsPrincipal(claimsIdentity)
                        }
            };

            var actionContext = new HttpActionContext(
                controllerContext,
                new Mock<HttpActionDescriptor> { CallBase = true }.Object);

            attribute.OnAuthorization(actionContext);

            actionContext.Response.Should().BeNull();
        }

        [Fact]
        public void OnAuthorize_Should_Set_Response_When_Claim_Does_Not_Exist()
        {
            var attribute = new ClaimAuthorizeAttribute { ClaimValue = "create.widgets" };

            var claimsIdentity = new ClaimsIdentity(new[] { new Claim("lss.permission", "list.widgets") }, "Password");

            var controllerContext = new HttpControllerContext
                                        {
                                            Request = new HttpRequestMessage(),
                                            RequestContext =
                                                new HttpRequestContext
                                                    {
                                                        Principal =
                                                            new ClaimsPrincipal(
                                                            claimsIdentity)
                                                    }
                                        };

            var actionContext = new HttpActionContext(
                controllerContext,
                new Mock<HttpActionDescriptor> { CallBase = true }.Object);

            attribute.OnAuthorization(actionContext);

            actionContext.Response.Should().NotBeNull();
            actionContext.Response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public void OnAuthorize_Should_Set_Response_When_User_Is_Not_Authenticated()
        {
            var attribute = new ClaimAuthorizeAttribute { ClaimValue = "create.widgets" };

            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim("lss.permission", "list.widgets")
            });

            var controllerContext = new HttpControllerContext
            {
                Request = new HttpRequestMessage(),
                RequestContext = new HttpRequestContext { Principal = new ClaimsPrincipal(claimsIdentity) }
            };

            var actionContext = new HttpActionContext(controllerContext, new Mock<HttpActionDescriptor> { CallBase = true }.Object);

            attribute.OnAuthorization(actionContext);

            actionContext.Response.Should().NotBeNull();
            actionContext.Response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async void OnAuthorizationAsync_Should_Not_Set_Response_When_Claim_Exists()
        {
            var attribute = new ClaimAuthorizeAttribute { ClaimValue = "list.widgets" };

            var claimsIdentity = new ClaimsIdentity(new[] { new Claim("lss.permission", "list.widgets") }, "Password");

            var controllerContext = new HttpControllerContext
            {
                Request = new HttpRequestMessage(),
                RequestContext =
                    new HttpRequestContext
                    {
                        Principal = new ClaimsPrincipal(claimsIdentity)
                    }
            };

            var actionContext = new HttpActionContext(
                controllerContext,
                new Mock<HttpActionDescriptor> { CallBase = true }.Object);

            await attribute.OnAuthorizationAsync(actionContext, new CancellationToken());

            actionContext.Response.Should().BeNull();
        }

        [Fact]
        public void OnAuthorizeAsync_Should_Set_Response_When_Claim_Does_Not_Exist()
        {
            var attribute = new ClaimAuthorizeAttribute { ClaimValue = "create.widgets" };

            var claimsIdentity = new ClaimsIdentity(new[] { new Claim("lss.permission", "list.widgets") }, "Password");

            var controllerContext = new HttpControllerContext
            {
                Request = new HttpRequestMessage(),
                RequestContext =
                    new HttpRequestContext
                    {
                        Principal = new ClaimsPrincipal(claimsIdentity)
                    }
            };

            var actionContext = new HttpActionContext(
                controllerContext,
                new Mock<HttpActionDescriptor> { CallBase = true }.Object);

            attribute.OnAuthorizationAsync(actionContext, new CancellationToken());

            actionContext.Response.Should().NotBeNull();
            actionContext.Response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public void OnAuthorizeAsync_Should_Set_Response_When_User_Is_Not_Authenticated()
        {
            var attribute = new ClaimAuthorizeAttribute { ClaimValue = "create.widgets" };

            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim("lss.permission", "list.widgets")
            });

            var controllerContext = new HttpControllerContext
            {
                Request = new HttpRequestMessage(),
                RequestContext = new HttpRequestContext { Principal = new ClaimsPrincipal(claimsIdentity) }
            };

            var actionContext = new HttpActionContext(controllerContext, new Mock<HttpActionDescriptor> { CallBase = true }.Object);

            attribute.OnAuthorizationAsync(actionContext, new CancellationToken());

            actionContext.Response.Should().NotBeNull();
            actionContext.Response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
