using Voyage.Core;
using Microsoft.Owin;
using System.Threading.Tasks;
using Voyage.Web.Extensions;
using System;
using Voyage.Web.Middleware.Processors;
using Voyage.Services.Audit;
using Voyage.Services.Ant;
using AntPathMatching;
using Voyage.Models;
using System.Linq;
using System.Configuration;

namespace Voyage.Web.Middleware
{
    public class ActivityAuditMiddleware : OwinMiddleware
    {
        private readonly IAuditService _auditService;
        private readonly ErrorResponseProcessor _processor;
        private readonly IAnt[] _excludePaths;

        public ActivityAuditMiddleware(OwinMiddleware next, IAuditService auditService, ErrorResponseProcessor processor, IAntService antService)
            : base(next)
        {
            _processor = processor.ThrowIfNull(nameof(processor));
            _auditService = auditService.ThrowIfNull(nameof(auditService));
            _excludePaths = antService.GetAntPaths(ConfigurationManager.AppSettings["ActionLoggingExcludePaths"]?.Split(','));
        }

        public override async Task Invoke(IOwinContext context)
        {
            // Create a request id - depending on deployment, the
            // owin variable may or may not be initialized
            var requestId = Guid.NewGuid().ToString();

            // Record the request
            var requestAudit = context.ToAuditModel(requestId);

            // Check if path should be excluded
            var exclude = IsExcludedPath(requestAudit);

            // Record the request
            if (!exclude)
            {
                await _auditService.RecordAsync(requestAudit);
            }

            // Continue pipeline execution
            await Next.Invoke(context);

            // Record the response
            var responseAudit = context.ToAuditModel(requestId);

            // Check if the response is an error that should be processed
            if (_processor.ShouldProcess(context.Response))
            {
                responseAudit.Error = await _processor.GetResponseStringAsync(context.Response);
            }

            if (!exclude)
            {
                await _auditService.RecordAsync(responseAudit);
            }
        }

        private bool IsExcludedPath(ActivityAuditModel model)
        {
            return _excludePaths?.Any(x => x.IsMatch(model.Path)) ?? false;
        }
    }
}
