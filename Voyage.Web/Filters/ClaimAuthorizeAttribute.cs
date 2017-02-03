﻿using System.Collections.Generic;
using Voyage.Core;
using Serilog;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using Voyage.Models;

namespace Voyage.Web.Filters
{
    public class ClaimAuthorizeAttribute : AuthorizeAttribute
    {
        public string ClaimValue { get; set; }

        public string ClaimType { get; set; }

        public ClaimAuthorizeAttribute()
        {
            ClaimType = Constants.AppClaims.Type;
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

        public override Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
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

            return Task.FromResult<object>(null);
        }
    }
}
