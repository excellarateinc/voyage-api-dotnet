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
    [ConventionAuthorize]
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
         * @apiPermission lss.permission->list.widgets
         * 
         * @apiUse AuthHeader
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
         * @apiUse UnauthorizedError
         */
        //[ClaimAuthorize(ClaimValue = LssClaims.ListWidgets)]
        [ConventionAuthorize]
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
         * 
         * @apiPermission lss.version->view.widget
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
         * @apiUse AuthHeader
         * @apiUse UnauthorizedError
         */
        //[ClaimAuthorize(ClaimValue = LssClaims.ViewWidget)]
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
         * @apiPermission lss.version->create.widget
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
         * @apiUse BadRequestError
         * @apiUse AuthHeader
         * @apiUse UnauthorizedError
         */
        //[ClaimAuthorize(ClaimValue = LssClaims.CreateWidget)]
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
         * @apiPermission lss.version->delete.widget
         * 
         * @apiParam {Number} id ID of the widget which will be deleted
         * 
         * @apiSuccessExample Success-Response:
         *      HTTP/1.1 204
         *
         * @apiUse AuthHeader
         * @apiUse UnauthorizedError
         */
        //[ClaimAuthorize(ClaimValue = LssClaims.DeleteWidget)]
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
         * @apiPermission lss.version->update.widget
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
         * 
         * @apiUse AuthHeader
         * @apiUse UnauthorizedError 
         */
        //[ClaimAuthorize(ClaimValue = LssClaims.UpdateWidget)]
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