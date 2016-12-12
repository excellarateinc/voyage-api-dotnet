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
        /// <returns>Created response if there is not an error, otherwise BadRequest or NotFound</returns>
        protected IHttpActionResult CreatedEntityAt<TModel>(string routeName, Func<object> routeValue, EntityResult<TModel> entityResult)
            where TModel : class
        {
            var errorResult = CheckErrorResult(entityResult);
            return errorResult ?? CreatedAtRoute(routeName, routeValue(), entityResult.Model);
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
                // If there are any errors, add them 
                if (entityResult.Errors.Any())
                {
                    foreach (var error in entityResult.Errors)
                    {
                        ModelState.AddModelError("model", error);
                    }
                }

                if (entityResult.IsEntityNotFound)
                {
                    return Content(HttpStatusCode.NotFound, ModelState.ConvertToResponseModel());
                }

                return Content(HttpStatusCode.BadRequest, ModelState.ConvertToResponseModel());
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
            var errorResult = CheckErrorResult(entityResult);
            return errorResult ?? Ok(entityResult.Model);
        }

        /// <summary>
        /// Creates a NoContent response 
        /// </summary>
        /// <param name="entityResult">Result</param>
        /// <returns>No Content (204) response if there is not an error, otherwise BadRequest or NotFound</returns>
        protected IHttpActionResult NoContent(EntityResult entityResult)
        {
            var errorResult = CheckErrorResult(entityResult);
            return errorResult ?? StatusCode(HttpStatusCode.NoContent);
        }
    }
}
