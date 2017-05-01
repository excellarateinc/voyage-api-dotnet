using System.Net.Http;
using System.Web.Http.Filters;
using Voyage.Api.Extensions;
using Voyage.Core.Exceptions;

namespace Voyage.Api.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var exception = context.Exception as ApiException;
            if (exception != null)
            {
                context.Response = context.Request.CreateResponse(exception.StatusCode, exception.Message.ToRequestErrorModel());
            }
        }
    }
}