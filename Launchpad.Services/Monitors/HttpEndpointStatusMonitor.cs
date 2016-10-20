using Launchpad.Services.Interfaces;
using System;
using System.Collections.Generic;
using Launchpad.Models;
using System.Net;
using System.Net.Http;
using Launchpad.Core;
using Launchpad.Models.Enum;

namespace Launchpad.Services.Monitors
{
    /// <summary>
    /// This is here for sample purposes. 
    /// </summary>
    public class HttpEndpointStatusMonitor : IStatusMonitor
    {
        public string Name => "Http Endpoint Status";

        public MonitorType Type => MonitorType.HttpEndpoint;

        private readonly HttpClient _client;

        public HttpEndpointStatusMonitor(HttpClient client)
        {
            _client = client.ThrowIfNull(nameof(client));
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StatusModel> GetStatus()
        {

            //These could come from DI or a custom config
            string[] testUrls = new[] { "http://thisisnotavalidurl.io", "http://www.yahoo.com", "http://thisisnotavalidurl.io" };
            List<StatusModel> statuses = new List<StatusModel>();

            foreach (var url in testUrls)
            {
                StatusModel model;
                try
                {
                    var task = _client.GetAsync(url);
                    task.Wait(5000);
                
                    //This is a weak implementation - other 2XX response would fail
                    if (task.IsCompleted && task.Result.StatusCode == HttpStatusCode.OK)
                    {
                        model = new StatusModel { Code = StatusCode.OK, Message = $"Connected to {url}", Time = DateTime.Now };
                    }
                    else
                    {
                        model = new StatusModel { Code = StatusCode.Critical, Message = $"Unable to connect to {url}", Time = DateTime.Now };
                    }
                }
                catch (AggregateException ex)
                {
                    string message = ex.FlattenMessages();
                    model = new StatusModel { Code = StatusCode.Critical, Message = $"Error processing {url} -> {message}", Time = DateTime.Now };
                }
                statuses.Add(model);

            }
            return statuses;
        }
    }
}
