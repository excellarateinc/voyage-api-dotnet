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
        /// <summary>
        /// Creates (201) a Created response 
        /// </summary>
        /// <param name="entityResult">Result</param>
        /// <returns>Created response if there is not an error, otherwise BadRequest or NotFound</returns>
        protected IHttpActionResult CreatedEntityAt<TModel>(string routeName, Func<object> routeValue, EntityResult<TModel> entityResult)
            where TModel : class
        {
            var actionResult = CheckErrorResult(entityResult);
            return actionResult != null ? actionResult : CreatedAtRoute(routeName, routeValue(), entityResult.Model);
        }

        /// <summary>
        /// Checks the given entity result for common error states
        /// </summary>
        /// <param name="entityResult">Result</param>
        /// <returns>IHttpActionResult if an error state is present, otherwise null</returns>
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

        /// <summary>
        /// Creates an OK response 
        /// </summary>
        /// <param name="entityResult">Result</param>
        /// <returns>OK (200) response if there is not an error, otherwise BadRequest or NotFound</returns>
        protected IHttpActionResult CreateModelResult<TModel>(EntityResult<TModel> entityResult)
            where TModel : class
        {
            var actionResult = CheckErrorResult(entityResult);
            return actionResult != null ? actionResult : Ok(entityResult.Model);
        }

        /// <summary>
        /// Creates a NoContent response 
        /// </summary>
        /// <param name="entityResult">Result</param>
        /// <returns>No Content (204) response if there is not an error, otherwise BadRequest or NotFound</returns>
        protected IHttpActionResult NoContent(EntityResult entityResult)
        {
            var actionResult = CheckErrorResult(entityResult);
            return actionResult != null ? actionResult : StatusCode(HttpStatusCode.NoContent);
        }
    }
}