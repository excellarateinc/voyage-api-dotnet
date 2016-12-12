using Launchpad.Web.Extensions;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

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
                    .CreateResponse(
                        HttpStatusCode.BadRequest,
                        actionContext.ModelState.ConvertToResponseModel());
            }
        }
    }
}