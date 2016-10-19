using Launchpad.Core;
using Launchpad.Models;
using Launchpad.Services.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Launchpad.Web.Controllers.API
{
    public class WidgetController : ApiController
    {
        private IWidgetService _widgetService;

        public WidgetController(IWidgetService widgetService)
        {
            _widgetService = widgetService.ThrowIfNull(nameof(widgetService));
        }

        /**
         * @api {get} /widget Get all widgets
         * @apiVersion 0.1.1
         * @apiName GetWidgets
         * @apiGroup Widget
         * 
         * @apiSuccess {Object[]} widgets List of widgets.
         * @apiSuccess {String} widgets.name Name of the Widget.
         * @apiSuccess {Number} widgets.id ID of the Widget.
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
        public IEnumerable<WidgetModel> Get()
        {
            return _widgetService.GetWidgets();
        }


        /**
         * @api {get} /widget/:id Get a widget
         * @apiVersion 0.1.1
         * @apiName GetWidget
         * @apiGroup Widget
         *
         * @apiParam {Number} id Widget's unique ID.
         *
         * @apiSuccess {String} name Name of the Widget.
         * @apiSuccess {Number} id ID of the Widget.
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
    }
}
