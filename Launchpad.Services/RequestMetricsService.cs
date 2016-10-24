using Launchpad.Models;
using Launchpad.Services.Interfaces;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Launchpad.Services
{

    /// <summary>
    /// This is a simple web api request tracker - it will track the last 10 requests.
    /// </summary>
    public class RequestMetricsService : IRequestMetricsService
    {

        //Number of items in the queue before dequeue is called
        const int maxModels = 10;

        //Queue to hold the data points
        private readonly ConcurrentQueue<RequestDataPointModel> _requestQueue;

        public RequestMetricsService()
        {
            _requestQueue = new ConcurrentQueue<RequestDataPointModel>();
        }

        /// <summary>
        /// Log a request 
        /// </summary>
        /// <param name="model">Request data point</param>
        public void Log(RequestDataPointModel model)
        {
            if(_requestQueue.Count >= 10)
            {
                RequestDataPointModel expired;
                _requestQueue.TryDequeue(out expired);
            }
            _requestQueue.Enqueue(model);
        }

        /// <summary>
        /// Retrieve a snapshot of the requests in the queue
        /// </summary>
        /// <returns>Request data points</returns>
        public IEnumerable<RequestDataPointModel> GetActivity()
        {
            return _requestQueue.ToList();
        }
    }
}
