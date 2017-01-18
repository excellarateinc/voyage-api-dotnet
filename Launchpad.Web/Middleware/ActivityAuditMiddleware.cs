using Launchpad.Core;
using Microsoft.Owin;
using System.Threading.Tasks;
using Launchpad.Web.Extensions;
using System;
using Launchpad.Web.Middleware.Processors;
using Launchpad.Services.Audit;

namespace Launchpad.Web.Middleware
{
    public class ActivityAuditMiddleware : OwinMiddleware
    {
        private readonly IAuditService _auditService;
        private readonly ErrorResponseProcessor _processor;

        public ActivityAuditMiddleware(OwinMiddleware next, IAuditService auditService, ErrorResponseProcessor processor)
            : base(next)
        {
            _processor = processor.ThrowIfNull(nameof(processor));
            _auditService = auditService.ThrowIfNull(nameof(auditService));
        }

        public override async Task Invoke(IOwinContext context)
        {
            // Create a request id - depending on deployment, the
            // owin variable may or may not be initialized
            var requestId = Guid.NewGuid().ToString();

            // Record the request
            var requestAudit = context.ToAuditModel(requestId);
            await _auditService.RecordAsync(requestAudit);

            // Continue pipeline execution
            await Next.Invoke(context);

            // Record the response
            var responseAudit = context.ToAuditModel(requestId);

            // Check if the response is an error that should be processed
            if (_processor.ShouldProcess(context.Response))
            {
                responseAudit.Error = await _processor.GetResponseStringAsync(context.Response);
            }

            await _auditService.RecordAsync(responseAudit);
        }
    }
}
