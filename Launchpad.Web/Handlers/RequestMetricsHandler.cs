using Launchpad.Core;
using Launchpad.Models;
using Launchpad.Services;
using Launchpad.Services.Interfaces;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Launchpad.Web.Handlers
{
    /// <summary>
    /// Simple usage tracking
    /// </summary>
    /// <remarks>This should be replaced with a robust solution or a 3rd party component</remarks>
    public class RequestMetricsHandler : DelegatingHandler
    {
        private readonly IRequestMetricsService _metricsService;

        public RequestMetricsHandler(IRequestMetricsService metricsService)
        { 
            _metricsService = metricsService.ThrowIfNull(nameof(metricsService));
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var usage = new RequestDataPointModel
            {
                Method = request.Method.Method,
                Path = request.RequestUri.AbsolutePath,
                RequestDateTime = DateTime.Now
            };
            _metricsService.Log(usage);
            return base.SendAsync(request, cancellationToken);
        }
    }
}