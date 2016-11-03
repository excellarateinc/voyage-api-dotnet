using FluentAssertions;
using Launchpad.Web.Filters;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using Xunit;

namespace Launchpad.Web.UnitTests.Filters
{
    public class CoventionAuthrorizeAttributeTests
    {
        [Fact]
        public void OnAuthorize_Should_Not_Set_Response_When_Claim_Exists()
        {
            var attribute = new ConventionAuthorizeAttribute();


            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim("lss.permission", "GET=>api/widget")
            }, "Password");


            var controllerContext = new HttpControllerContext();
            var route = new HttpRoute("api/v2/widget");
            var routeData = new HttpRouteData(route);

            controllerContext.Request = new System.Net.Http.HttpRequestMessage() { Method = new System.Net.Http.HttpMethod("GET") };
            controllerContext.RequestContext = new HttpRequestContext();
            controllerContext.RequestContext.Principal = new ClaimsPrincipal(claimsIdentity);
            controllerContext.RouteData = routeData;

            var actionDescriptor = new Mock<HttpActionDescriptor>() { CallBase = true };
            var actionContext = new HttpActionContext(controllerContext, actionDescriptor.Object);


            attribute.OnAuthorization(actionContext);

            actionContext.Response.Should().BeNull();
        }

        [Fact]
        public void OnAuthorize_Should_Set_Response_When_Claim_Does_Not_Exist()
        {
            var attribute = new ConventionAuthorizeAttribute();


            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim("lss.permission", "GET=>api/v1/widget")
            }, "Password");


            var controllerContext = new HttpControllerContext();
            var route = new HttpRoute("api/v2/widget");
            var routeData = new HttpRouteData(route);

            controllerContext.Request = new System.Net.Http.HttpRequestMessage() { Method = new System.Net.Http.HttpMethod("GET") };
            controllerContext.RequestContext = new HttpRequestContext();
            controllerContext.RequestContext.Principal = new ClaimsPrincipal(claimsIdentity);
            controllerContext.RouteData = routeData;

            var actionDescriptor = new Mock<HttpActionDescriptor>() { CallBase = true };
            var actionContext = new HttpActionContext(controllerContext, actionDescriptor.Object);


            attribute.OnAuthorization(actionContext);

            actionContext.Response.Should().NotBeNull();
            actionContext.Response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public void OnAuthorize_Should_Set_Response_When_User_Is_Not_Authenticated()
        {

            var attribute = new ConventionAuthorizeAttribute();
            

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
            var attribute = new ConventionAuthorizeAttribute();
            

            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim("lss.permission", "GET=>api/widget")
            }, "Password");


            var controllerContext = new HttpControllerContext();
            var route = new HttpRoute("api/v2/widget");
            var routeData = new HttpRouteData(route);
           
            controllerContext.Request = new System.Net.Http.HttpRequestMessage() { Method = new System.Net.Http.HttpMethod("GET")};
            controllerContext.RequestContext = new HttpRequestContext();
            controllerContext.RequestContext.Principal = new ClaimsPrincipal(claimsIdentity);
            controllerContext.RouteData = routeData;

            var actionDescriptor = new Mock<HttpActionDescriptor>() { CallBase = true };
            var actionContext = new HttpActionContext(controllerContext, actionDescriptor.Object);


            await attribute.OnAuthorizationAsync(actionContext, new System.Threading.CancellationToken());

            actionContext.Response.Should().BeNull();
        }

        [Fact]
        public void OnAuthorizeAsync_Should_Set_Response_When_Claim_Does_Not_Exist()
        {
            var attribute = new ConventionAuthorizeAttribute();


            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim("lss.permission", "GET=>api/v1/widget")
            }, "Password");


            var controllerContext = new HttpControllerContext();
            var route = new HttpRoute("api/v2/widget");
            var routeData = new HttpRouteData(route);

            controllerContext.Request = new System.Net.Http.HttpRequestMessage() { Method = new System.Net.Http.HttpMethod("GET") };
            controllerContext.RequestContext = new HttpRequestContext();
            controllerContext.RequestContext.Principal = new ClaimsPrincipal(claimsIdentity);
            controllerContext.RouteData = routeData;

            var actionDescriptor = new Mock<HttpActionDescriptor>() { CallBase = true };
            var actionContext = new HttpActionContext(controllerContext, actionDescriptor.Object);


            attribute.OnAuthorizationAsync(actionContext, new System.Threading.CancellationToken());

            actionContext.Response.Should().NotBeNull();
            actionContext.Response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public void OnAuthorizeAsync_Should_Set_Response_When_User_Is_Not_Authenticated()
        {
            var attribute = new ConventionAuthorizeAttribute();


            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim("lss.permission", "GET=>api/widget")
            });


            var controllerContext = new HttpControllerContext();
            var route = new HttpRoute("api/v2/widget");
            var routeData = new HttpRouteData(route);

            controllerContext.Request = new System.Net.Http.HttpRequestMessage() { Method = new System.Net.Http.HttpMethod("GET") };
            controllerContext.RequestContext = new HttpRequestContext();
            controllerContext.RequestContext.Principal = new ClaimsPrincipal(claimsIdentity);
            controllerContext.RouteData = routeData;

            var actionDescriptor = new Mock<HttpActionDescriptor>() { CallBase = true };
            var actionContext = new HttpActionContext(controllerContext, actionDescriptor.Object);


            attribute.OnAuthorizationAsync(actionContext, new System.Threading.CancellationToken());

            actionContext.Response.Should().NotBeNull();
            actionContext.Response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

    }
}
