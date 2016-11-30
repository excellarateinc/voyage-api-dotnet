using Launchpad.Models;
using Launchpad.Web.Extensions;
using System;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace Launchpad.Web.Controllers.API
{
    public abstract class BaseApiController : ApiController
    {
        protected IHttpActionResult CreatedEntityAt<TModel>(string routeName, Func<object> routeValue, EntityResult<TModel> entityResult)
            where TModel : class
        {
            var actionResult = CheckErrorResult(entityResult);
            return actionResult != null ? actionResult : CreatedAtRoute(routeName, routeValue(), entityResult.Model);
        }

        protected IHttpActionResult CheckErrorResult(EntityResult entityResult)
        {
            if (!entityResult.Succeeded)
            {
                //If there are any errors, add them 
                if (entityResult.Errors.Any())
                {
                    foreach (var error in entityResult.Errors)
                    {
                        ModelState.AddModelError("model", error);
                    }
                }

                if (entityResult.IsMissingEntity)
                {
                    return Content(HttpStatusCode.NotFound, ModelState.ConvertToResponseModel());
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, ModelState.ConvertToResponseModel());
                }
            }
            return null;
        }

        protected IHttpActionResult CreateModelResult<TModel>(EntityResult<TModel> entityResult)
            where TModel : class
        {
            var actionResult = CheckErrorResult(entityResult);
            return actionResult != null ? actionResult : Ok(entityResult.Model);
        }

        protected IHttpActionResult NoContent(EntityResult entityResult)
        {
            var actionResult = CheckErrorResult(entityResult);
            return actionResult != null ? actionResult : StatusCode(HttpStatusCode.NoContent);
        }
    }
}