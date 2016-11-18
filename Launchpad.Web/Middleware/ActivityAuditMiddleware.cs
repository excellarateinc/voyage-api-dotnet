using Launchpad.Core;
using Microsoft.Owin;
using Serilog;
using System.Threading.Tasks;
using Launchpad.Web.Extensions;
using Launchpad.Services.Interfaces;
using System;

namespace Launchpad.Web.Middleware
{
    public class ActivityAuditMiddleware : OwinMiddleware
    {
        private static string EmptyId = Guid.Empty.ToString();

        private readonly ILogger _logger;
        private readonly IAuditService _auditService;

        public ActivityAuditMiddleware(OwinMiddleware next, ILogger logger, IAuditService auditService) : base(next)
        {
            _logger = logger.ThrowIfNull(nameof(logger));
            _auditService = auditService.ThrowIfNull(nameof(auditService));
        }


        public async override Task Invoke(IOwinContext context)
        {
            
            var requestId = Guid.NewGuid().ToString();
            var requestAudit = context.ToAuditModel();
            if (EmptyId.Equals(requestAudit.RequestId, StringComparison.InvariantCultureIgnoreCase))
                requestAudit.RequestId = requestId;

            await _auditService.RecordAsync(requestAudit);

            _logger
                 .ForContext<ActivityAuditMiddleware>()
                 .Information("({eventCode:l}) ({requestId}) Request => {method:l} {path:l}",
                     EventCodes.ActivityAudit,
                     requestAudit.RequestId,
                     requestAudit.Method,
                     requestAudit.Path);


            await Next.Invoke(context);

            var responseAudit = context.ToAuditModel();
            if (EmptyId.Equals(responseAudit.RequestId, StringComparison.InvariantCultureIgnoreCase))
                responseAudit.RequestId = requestId;

            await _auditService.RecordAsync(responseAudit);

            _logger
                 .ForContext<ActivityAuditMiddleware>()
                 .Information("({eventCode:l}) ({requestId}) User => {user} Response => {statusCode}",
                    EventCodes.ActivityAudit,
                    responseAudit.RequestId,
                    responseAudit.UserName,
                    responseAudit.StatusCode);
        }

    }



}