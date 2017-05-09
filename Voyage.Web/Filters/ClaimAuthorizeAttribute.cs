using System.Collections.Generic;
using System.Linq;
using Voyage.Core;
using Serilog;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using Autofac;
using Autofac.Integration.Owin;
using Voyage.Models;
using Voyage.Services.User;

namespace Voyage.Web.Filters
{
    public class ClaimAuthorizeAttribute : AuthorizeAttribute
    {
        public string ClaimValue { get; set; }

        public string ClaimType { get; set; }

        public ClaimAuthorizeAttribute()
        {
            ClaimType = AppClaims.Type;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var identity = actionContext.RequestContext.Principal.Identity as ClaimsIdentity;
            if (identity != null && !identity.IsAuthenticated)
            {
                var requestErrorModel = new ResponseErrorModel
                {
                    Error = Core.Constants.ErrorCodes.Unauthorized,
                    ErrorDescription = "not authorized for request"
                };
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new List<ResponseErrorModel> { requestErrorModel });
            }
            else if (identity != null && !identity.HasClaim(ClaimType, ClaimValue))
            {
                // Check if the user has the correct claim
                Log.Logger
                      .ForContext<ClaimAuthorizeAttribute>()
                      .Information("({eventCode:l}) {user} does not have claim {claimType}.{claimValue}", EventCodes.Authorization, identity.Name, ClaimType, ClaimValue);
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
            }
        }

        public override async Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var identity = actionContext.RequestContext.Principal.Identity as ClaimsIdentity;

            // User has not authenticated / signed in
            if (identity != null && !identity.IsAuthenticated)
            {
                var requestErrorModel = new ResponseErrorModel
                {
                    Error = Core.Constants.ErrorCodes.Unauthorized,
                    ErrorDescription = "not authorized for request"
                };
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new List<ResponseErrorModel> { requestErrorModel });
            }
            else if (identity != null && !identity.HasClaim(ClaimType, ClaimValue))
            {
                // Check if the user has the correct claim
                Log.Logger
                    .ForContext<ClaimAuthorizeAttribute>()
                    .Information("({eventCode:l}) {user} does not have claim {claimType}.{claimValue}", EventCodes.Authorization, identity.Name, ClaimType, ClaimValue);
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            // Implicit authentication will need to verify if user is either locked or need verification
            if (identity?.Name != null)
            {
                await ValidateUser(actionContext, identity);
            }
        }

        private async Task ValidateUser(HttpActionContext context, ClaimsIdentity identity)
        {
            // Check if log in user needs to be verify or inactive
            var container = context.Request.GetOwinContext().GetAutofacLifetimeScope();
            var userService = container.Resolve<IUserService>();
            var user = await userService.GetUserByNameAsync(identity.Name);
            if (!user.IsActive)
            {
                Log.Logger
                    .ForContext<ClaimAuthorizeAttribute>()
                    .Information("({eventCode:l}) {user} is not active.", EventCodes.Authorization, identity.Name);

                var requestErrorModel = new ResponseErrorModel
                {
                    Error = Core.Constants.ErrorCodes.Forbidden,
                    ErrorDescription = "User has been disabled."
                };
                context.Response = context.Request.CreateResponse(HttpStatusCode.Unauthorized, new List<ResponseErrorModel> { requestErrorModel });
            }
            else if (user.IsVerifyRequired)
            {
                Log.Logger
                    .ForContext<ClaimAuthorizeAttribute>()
                    .Information("({eventCode:l}) {user} need a verification.", EventCodes.Authorization, identity.Name);

                var requestErrorModel = new ResponseErrorModel
                {
                    Error = Core.Constants.ErrorCodes.Forbidden,
                    ErrorDescription = "User verification is required."
                };
                context.Response = context.Request.CreateResponse(HttpStatusCode.Unauthorized, new List<ResponseErrorModel> { requestErrorModel });
            }
        }
    }
}
