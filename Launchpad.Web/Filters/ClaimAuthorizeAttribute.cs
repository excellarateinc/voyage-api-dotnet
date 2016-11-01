using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Launchpad.Web.Filters
{
    public class ClaimAuthorizeAttribute : AuthorizeAttribute
    {
        public string ClaimValue { get; set; }

        public string ClaimType { get; set; }

        public ClaimAuthorizeAttribute()
        {
            ClaimType = Constants.LssClaims.Type;
 
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var identity = actionContext.RequestContext.Principal.Identity as ClaimsIdentity;
            if (!identity.IsAuthenticated)
            {
                actionContext.Response = actionContext.Request.CreateResponse(statusCode: HttpStatusCode.Unauthorized);
            }
            else if (!identity.HasClaim(ClaimType, ClaimValue)) //Check if the user has the correct claim
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
            }
        }

        public override Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var identity = actionContext.RequestContext.Principal.Identity as ClaimsIdentity;

            //User has not authenticated / signed in
            if (!identity.IsAuthenticated)
            {
            
                actionContext.Response = actionContext.Request.CreateResponse(statusCode: HttpStatusCode.Unauthorized);
            }
            else if (!identity.HasClaim(ClaimType, ClaimValue)) //Check if the user has the correct claim
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            return Task.FromResult<object>(null);

        }

    }
}