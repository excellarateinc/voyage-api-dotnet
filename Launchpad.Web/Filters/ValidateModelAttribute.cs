using Launchpad.Models;
using Launchpad.Web.Extensions;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Linq;
using System.Collections;

namespace Launchpad.Web.Filters
{
   

    /// <summary>
    /// Returns a BadRequest response if the model is invalid
    /// </summary>
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = actionContext
                    .Request
                    .CreateResponse(HttpStatusCode.BadRequest, 
                        actionContext.ModelState.ConvertToResponseModel());                    
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (!actionExecutedContext.ActionContext.ModelState.IsValid)
            {
                actionExecutedContext.Response = actionExecutedContext
                    .Request
                    .CreateResponse(HttpStatusCode.BadRequest,
                        actionExecutedContext.ActionContext.ModelState.ConvertToResponseModel());
            }

            if (
                actionExecutedContext.ActionContext.Response.Content.ReadAsStringAsync().Result == "null" &&
                (
                    actionExecutedContext.ActionContext.Request.Method == HttpMethod.Get
                ) &&
                !typeof(IEnumerable).IsAssignableFrom(actionExecutedContext.ActionContext.Response.Content.GetType())
                )
            {
                actionExecutedContext.Response = actionExecutedContext
                    .Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}