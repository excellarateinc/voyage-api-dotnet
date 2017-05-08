using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using FluentAssertions;
using Moq;
using Voyage.Web.Filters;
using Voyage.Web.UnitTests.Common;
using Xunit;

namespace Voyage.Web.UnitTests.Filters
{
    public class PermissionAuthorizeAttributeTests : BaseUnitTest
    {
        [Fact]
        public void PermissionType_Should_Default_To_Constant_Value()
        {
            new PermissionAuthorizeAttribute().PermissionType.Should().Be(Constants.AppPermissions.Type);
        }

        [Fact]
        public void OnAuthorize_Should_Not_Set_Response_When_Permission_Exists()
        {
            var attribute = new PermissionAuthorizeAttribute { PermissionValue = "list.test" };

            var claimsIdentity = new ClaimsIdentity(new[] { new Claim("app.permission", "list.test") }, "Password");

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
        public void OnAuthorize_Should_Set_Response_When_Permission_Does_Not_Exist()
        {
            var attribute = new PermissionAuthorizeAttribute { PermissionValue = "create.test" };

            var claimsIdentity = new ClaimsIdentity(new[] { new Claim("app.permission", "list.test") }, "Password");

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
            var attribute = new PermissionAuthorizeAttribute { PermissionValue = "create.test" };

            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim("app.permission", "list.test")
            });

            var controllerContext = new HttpControllerContext
            {
                Request = new HttpRequestMessage(),
                RequestContext = new HttpRequestContext { Principal = new ClaimsPrincipal(claimsIdentity) }
            };

            controllerContext.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var actionContext = new HttpActionContext(controllerContext, new Mock<HttpActionDescriptor> { CallBase = true }.Object);

            attribute.OnAuthorization(actionContext);

            actionContext.Response.Should().NotBeNull();
            actionContext.Response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task OnAuthorizeAsync_Should_Set_Response_When_Permission_Does_Not_Exist()
        {
            var attribute = new PermissionAuthorizeAttribute { PermissionValue = "create.test" };

            var claimsIdentity = new ClaimsIdentity(new[] { new Claim("app.permission", "list.test") }, "Password");

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

            actionContext.Response.Should().NotBeNull();
            actionContext.Response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task OnAuthorizeAsync_Should_Set_Response_When_User_Is_Not_Authenticated()
        {
            var attribute = new PermissionAuthorizeAttribute { PermissionValue = "create.test" };

            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim("app.permission", "list.test")
            });

            var controllerContext = new HttpControllerContext
            {
                Request = new HttpRequestMessage(),
                RequestContext = new HttpRequestContext { Principal = new ClaimsPrincipal(claimsIdentity) }
            };

            controllerContext.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var actionContext = new HttpActionContext(controllerContext, new Mock<HttpActionDescriptor> { CallBase = true }.Object);

            await attribute.OnAuthorizationAsync(actionContext, new CancellationToken());

            actionContext.Response.Should().NotBeNull();
            actionContext.Response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
