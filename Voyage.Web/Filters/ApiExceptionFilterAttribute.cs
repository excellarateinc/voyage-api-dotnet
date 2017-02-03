using System.Net.Http;
using System.Web.Http.Filters;
using Voyage.Core.Exceptions;
using Voyage.Web.Extensions;

namespace Voyage.Web.Filters
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