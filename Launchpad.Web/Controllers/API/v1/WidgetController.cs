using Launchpad.Core;
using Launchpad.Models;
using Launchpad.Services.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Launchpad.Web.Controllers.API.V1
{



    [RoutePrefix(Constants.RoutePrefixes.V1)]
    public class WidgetController : ApiController
    {
        private IWidgetService _widgetService;

        public WidgetController(IWidgetService widgetService)
        {
            _widgetService = widgetService.ThrowIfNull(nameof(widgetService));
        }

        /**
         * @api {get} /v1/widget Get all widgets
         * @apiVersion 0.1.0
         * @apiName GetWidgets
         * @apiGroup Widget
         * 
         * @apiSuccess {Object[]} widgets List of widgets.
         * @apiSuccess {String} widgets.name Name of the widget.
         * @apiSuccess {Number} widgets.id ID of the widget.
         *
         * @apiSuccessExample Success-Response:
         *      HTTP/1.1 200 OK
         *      [
         *       {
         *          "id": 3,
         *          "name": "Large Widget"
         *       },
         *       {
         *          "id": 7,
         *          "name": "Medium Widget"
         *       }
         *      ]
         * 
         */
        [Route("widget")]
        public IEnumerable<WidgetModel> Get()
        {
            return _widgetService.GetWidgets();
        }


        /**
         * @api {get} /v1/widget/:id Get a widget
         * @apiVersion 0.1.0
         * @apiName GetWidget
         * @apiGroup Widget
         *
         * @apiParam {Number} id widget's unique ID.
         *
         * @apiSuccess {String} name Name of the widget.
         * @apiSuccess {Number} id ID of the widget.
         *
         * @apiSuccessExample Success-Response:
         *     HTTP/1.1 200 OK
         *     {
         *        "id": 3,
         *        "name": "Large Widget"
         *     }
         *
         * @apiError (Error 404) WidgetNotFound The Widget with the requested id was not found.
         * 
         * @apiErrorExample Error-Response:
         *     HTTP/1.1 404 Not Found
         *     {
         *       "message": "Widget with ID 33 not found"
         *     }
         */
        [Route("widget/{id:int}")]
        public HttpResponseMessage Get(int id)
        {
            var widget = _widgetService.GetWidget(id);
            if (widget == null)
            {
                var httpError = new HttpError($"Widget with ID {id} not found");
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, httpError);
            }
            else
            {
                return Request.CreateResponse(widget);
            }
        }

        /**
         * @api {post} /v1/widget Create a new widget
         * @apiVersion 0.1.0
         * @apiName CreateWidget
         * @apiGroup Widget
         * 
         * @apiParam {String} name Name of the widget
         * 
         * @apiSuccess {Object} widget New widget
         * @apiSuccess {String} widget.name Name of the widget
         * @apiSuccess {Number} widget.id ID of the widget
         * 
         * @apiSuccessExample Success-Response:
         *      HTTP/1.1 201 CREATED      
         *      {
         *           "id": 70,
         *           "name": "Small Widget"
         *      }
         *      
         *@apiUse BadRequestError
         */
        [Route("widget")]
        [HttpPost]
        public HttpResponseMessage AddWidget([FromBody] WidgetModel widget)
        {
            WidgetModel model = _widgetService.AddWidget(widget);
            return Request.CreateResponse(HttpStatusCode.Created, model);
        }

        /**
         * @api {delete} /v1/widget/:id Delete a widget
         * @apiVersion 0.1.0 
         * @apiName DeleteWidget
         * @apiGroup Widget
         * 
         * @apiParam {Number} id ID of the widget which will be deleted
         * 
         * @apiSuccessExample Success-Response:
         *      HTTP/1.1 204
         * 
         */ 
        [Route("widget/{id:int}")]
        [HttpDelete]
        public HttpResponseMessage DeleteWidget(int id)
        {
            _widgetService.DeleteWidget(id);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        /**
         * @api {put} /v1/widget Update an existing widget
         * @apiVersion 0.1.0
         * @apiName UpdateWidget
         * @apiGroup Widget
         * 
         * @apiParam {String} name Name of the widget
         * @apiParam {Number} id ID of the widget
         * 
         * @apiSuccess {Object} widget Updated widget
         * @apiSuccess {String} widget.name Name of the widget
         * @apiSuccess {Number} widget.id ID of the widget
         * 
         * @apiError (Error 404) WidgetNotFound The Widget with the requested id was not found.
         * 
         * @apiErrorExample Error-Response:
         *     HTTP/1.1 404 Not Found
         *     {
         *       "message": "Widget with ID 33 not found"
         *     }
         *
         * @apiUse BadRequestError   
         */
        [Route("widget")]
        [HttpPut]
        public HttpResponseMessage UpdateWidget([FromBody] WidgetModel widget)
        {
            WidgetModel model = _widgetService.UpdateWidget(widget);

            if (model == null)
            {
                var httpError = new HttpError($"Widget with ID {widget.Id} not found");
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, httpError);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
        }

     
    }
}
