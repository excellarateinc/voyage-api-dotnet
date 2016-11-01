using Launchpad.Core;
using Launchpad.Models;
using Launchpad.Services.Interfaces;
using Launchpad.Web.Filters;
using System.Net;
using System.Web.Http;
using static Launchpad.Web.Constants;

namespace Launchpad.Web.Controllers.API.V2
{
    [Authorize]
    [RoutePrefix(Constants.RoutePrefixes.V2)]
    public class WidgetV2Controller : ApiController
    {
        private IWidgetService _widgetService;

        public WidgetV2Controller(IWidgetService widgetService)
        {
            _widgetService = widgetService.ThrowIfNull(nameof(widgetService));
        }

        /**
         * @api {get} /v2/widget Get all widgets
         * @apiVersion 0.2.0
         * @apiName GetWidgets
         * @apiGroup Widget
         * 
         * @apiSuccess {Object[]} widgets List of widgets.
         * @apiSuccess {String} widgets.name Name of the widget.
         * @apiSuccess {Number} widgets.id ID of the widget.
         * @apiSuccess {String} widgets.color Color of the widget
         * 
         * @apiSuccessExample Success-Response:
         *      HTTP/1.1 200 OK
         *      [
         *       {
         *          "id": 3,
         *          "name": "Large Widget",
         *          "color": "Green"
         *       },
         *       {
         *          "id": 7,
         *          "name": "Medium Widget",
         *          "color": "Blue"
         *       }
         *      ]
         * 
         */
        [ClaimAuthorize(ClaimValue = LssClaims.ListWidgets)]
        [Route("widget")]
        public IHttpActionResult Get()
        {
            return Ok(_widgetService.GetWidgets());
        }


        /**
         * @api {get} /v2/widget/:id Get a widget
         * @apiVersion 0.2.0
         * @apiName GetWidget
         * @apiGroup Widget
         *
         * @apiParam {Number} id widget's unique ID.
         *
         * @apiSuccess {String} name Name of the widget.
         * @apiSuccess {Number} id ID of the widget.
         * @apiSuccess {String} color Color of the widget
         * 
         * @apiSuccessExample Success-Response:
         *     HTTP/1.1 200 OK
         *     {
         *        "id": 3,
         *        "name": "Large Widget"
         *        "color": "Green"
         *     }
         *
         * @apiUse NotFoundError
         */
        [Route("widget/{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var widget = _widgetService.GetWidget(id);
            if (widget == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(widget);
            }
        }

        /**
         * @api {post} /v2/widget Create a new widget
         * @apiVersion 0.2.0
         * @apiName CreateWidget
         * @apiGroup Widget
         * 
         * @apiParam {String} name Name of the widget
         * @apiParam {String} color Color of the widget
         * 
         * @apiSuccess {Object} widget New widget
         * @apiSuccess {String} widget.name Name of the widget
         * @apiSuccess {Number} widget.id ID of the widget
         * @apiSuccess {String} widgets.color Color of the widget
         * 
         * @apiSuccessExample Success-Response:
         *      HTTP/1.1 201 CREATED      
         *      {
         *           "id": 70,
         *           "name": "Small Widget",
         *           "color": "Magenta"
         *      }
         *      
         *@apiUse BadRequestError
         */
        [Route("widget")]
        [HttpPost]
        public IHttpActionResult AddWidget([FromBody] WidgetModel widget)
        {
            WidgetModel model = _widgetService.AddWidget(widget);
            return Created($"/api/widget/{widget.Id}", model);
        }

        /**
         * @api {delete} /v2/widget/:id Delete a widget
         * @apiVersion 0.2.0 
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
        public IHttpActionResult DeleteWidget(int id)
        {
            _widgetService.DeleteWidget(id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /**
         * @api {put} /v2/widget Update an existing widget
         * @apiVersion 0.2.0
         * @apiName UpdateWidget
         * @apiGroup Widget
         * 
         * @apiParam {String} name Name of the widget
         * @apiParam {Number} id ID of the widget
         * @apiParam {String} color Coor of the widget
         * 
         * @apiSuccess {Object} widget Updated widget
         * @apiSuccess {String} widget.name Name of the widget
         * @apiSuccess {Number} widget.id ID of the widget
         * @apiSuccess {String} widgets.color Color of the widget
         * 
         * @apiSuccessExample Success-Response:
         *     HTTP/1.1 200 OK
         *     {
         *        "id": 3,
         *        "name": "Large Widget"
         *        "color": "Green"
         *     }
         * 
         * @apiUse NotFoundError
         *
         * @apiUse BadRequestError   
         */
        [Route("widget")]
        [HttpPut]
        public IHttpActionResult UpdateWidget([FromBody] WidgetModel widget)
        {
            WidgetModel model = _widgetService.UpdateWidget(widget);

            if (model == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(model);
            }
        }


    }
}