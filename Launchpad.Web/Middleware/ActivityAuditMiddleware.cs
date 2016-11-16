using Launchpad.Core;
using Microsoft.Owin;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Launchpad.Web.Middleware
{
    public class ActivityAuditMiddleware : OwinMiddleware
    {
        private readonly ILogger _logger;

        public ActivityAuditMiddleware(ILogger logger, OwinMiddleware next) : base(next)
        {
            _logger = logger.ThrowIfNull(nameof(logger));
        }


        public async override Task Invoke(IOwinContext context)
        {
            //Correlate the logged message for request and response
            var correlationId = Guid.NewGuid();

            _logger
                 .ForContext<ActivityAuditMiddleware>()
                 .Information("({eventCode:l}) ({correlationId}) Request => {method:l} {path:l} User => {user}",
                     EventCodes.ActivityAudit,
                     correlationId,
                     context.Request.Method,
                     context.Request.Path.Value
                         );


            await Next.Invoke(context);

            _logger
                 .ForContext<ActivityAuditMiddleware>()
                 .Information("({eventCode:l}) ({correlationId}) User => {user} Response => {statusCode}",
                    EventCodes.ActivityAudit,
                    correlationId,
                    context.Response.StatusCode,
                    _getIdentityName(context)

                    );
        }

        private string _getIdentityName(IOwinContext context)
        {
            string identityName = "No Identity";

            if (context.Authentication != null && 
                context.Authentication.User != null && 
                context.Authentication.User.Identity != null && 
                !string.IsNullOrEmpty(context.Authentication.User.Identity.Name))
                identityName = context.Authentication.User.Identity.Name;

            return identityName;
        }
    }



}