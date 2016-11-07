using Launchpad.Core;
using Launchpad.Models.Enum;
using Launchpad.Services.Interfaces;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Launchpad.Web.Controllers.API.V1
{
   
    [RoutePrefix(Constants.RoutePrefixes.V1)]
    public class StatusController : ApiController
    {
        private readonly IStatusCollector _statusCollector;

        public StatusController(IStatusCollector statusCollector)
        {
            _statusCollector = statusCollector.ThrowIfNull(nameof(statusCollector));
        }

        /**
        * @api {get} /v1/statuses/:id Get status by monitor type
        * @apiVersion 0.1.0
        * @apiName GetStatusByType
        * @apiGroup Status
        *
        * @apiParam {Number} id Monitor type
        *
        * @apiSuccess {Object[]} statusAggregate List of status aggregates.
        * @apiSuccess {String} statusAggregate.name Name of the aggregate.
        * @apiSuccess {Number} statusAggregate.type Type of aggregate
        * @apiSuccess {Object[]} statusAggregate.status Array of statuses. 
        * @apiSuccess {Number} statusAggregate.status.code Status code
        * @apiSuccess {String} statusAggregate.status.message Status mesage
        * @apiSuccess {Date} statusAggregate.status.time Observation date
        *
        * @apiSuccessExample Success-Response:
        *      HTTP/1.1 200 OK
        *      [
        *          {
        *              "name": "Database Status",
        *              "type": 0,
        *              "status": [
        *                  {
        *                      "code": 0,
        *                      "message": "Launchpad Database -> Connected",
        *                      "time": "2016-10-20T09:30:13.2447018-05:00"
        *                  }
        *              ]
        *          }
        *      ]
        */
        [Route("statuses/{id:int}")]
        public IHttpActionResult Get(MonitorType id)
        {
            return Ok(_statusCollector.Collect(id));
        }

        /**
         * @api {get} /v1/status Get status from all monitors
         * @apiVersion 0.1.0
         * @apiName GetStatus
         * @apiGroup Status
         * 
         * @apiSuccess {Object[]} statusAggregate List of status aggregates.
         * @apiSuccess {String} statusAggregate.name Name of the aggregate.
         * @apiSuccess {Number} statusAggregate.type Type of aggregate
         * @apiSuccess {Object[]} statusAggregate.status Array of statuses.
         * @apiSuccess {Number} statusAggregate.status.code Status code
         * @apiSuccess {String} statusAggregate.status.message Status message
         * @apiSuccess {Date} statusAggregate.status.time Observation date
         * 
         * @apiSuccessExample Success-Response:
         *      HTTP/1.1 200 OK
         *      [
         *          {
         *              "name": "Database Status",
         *              "type": 0
         *              "status": [
         *                  {
         *                      "code": 0,
         *                      "message": "Launchpad Database -> Connected",
         *                      "time": "2016-10-20T09:30:13.2447018-05:00"
         *                  }
         *              ]
         *          },
         *          {
         *              "name": "Http Endpoint Status",
         *              "type": 1
         *              "status": [
         *                  {
         *                      "code": 0,
         *                      "message": "Connected to http://www.google.com",
         *                      "time": "2016-10-20T08:44:45.0739773-05:00"
         *                  },
         *                  {
         *                      "code": 1,
         *                      "message": "Error processing http://thisisnotavalidurl.io -> The remote name could not be resolved: 'thisisnotavalidurl.io'",
         *                      "time": "2016-10-20T08:44:50.5613938-05:00"
         *                  }
         *              ]
         *          }
         *      ]
         */
        [Route("statuses")]
        public IHttpActionResult Get()
        {
            var status = _statusCollector.Collect();
            return Ok(status);
        }

    }
}