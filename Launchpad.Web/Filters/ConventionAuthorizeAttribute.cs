using Serilog;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Launchpad.Web.Filters
{
    public class ConventionAuthorizeAttribute : AuthorizeAttribute
    {
        private const string PATTERN = @"^api(/v[0-9a-zA-Z]+)/";

        private static Regex matcher = new Regex(PATTERN);

        private string GetClaimValue(HttpActionContext actionContext)
        {

            var routeTemplate = actionContext.ControllerContext.RouteData.Route.RouteTemplate;

            if (matcher.IsMatch(routeTemplate))
            {
                var regExResult = matcher.Match(routeTemplate);
                routeTemplate = routeTemplate.Replace(regExResult.Groups[1].Value, string.Empty);
            }

            return $"{actionContext.Request.Method.Method}=>{routeTemplate}";
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {

            var identity = actionContext.RequestContext.Principal.Identity as ClaimsIdentity;
            if (!identity.IsAuthenticated)
            {
                actionContext.Response = actionContext.Request.CreateResponse(statusCode: HttpStatusCode.Unauthorized);
            }
            else if (!identity.HasClaim(Constants.LssClaims.Type, GetClaimValue(actionContext))) //Check if the user has the correct claim
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
            }
        }

        public override Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var identity = actionContext.RequestContext.Principal.Identity as ClaimsIdentity;
            var claimValue = GetClaimValue(actionContext);

            //User has not authenticated / signed in
            if (!identity.IsAuthenticated)
            {

                actionContext.Response = actionContext.Request.CreateResponse(statusCode: HttpStatusCode.Unauthorized);
            }
            else if (!identity.HasClaim(Constants.LssClaims.Type, claimValue)) //Check if the user has the correct claim
            {
                Log.Logger.Information("{statusCode}::{user} missing required claim {claimType}->{claimValue}", HttpStatusCode.Forbidden, identity.Name, Constants.LssClaims.Type, claimValue);
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            return Task.FromResult<object>(null);

        }
    }
}