using Launchpad.Core;
using Launchpad.Models;
using Launchpad.Services.Interfaces;
using Launchpad.Web.Filters;
using System.Web.Http;
using static Launchpad.Web.Constants;

namespace Launchpad.Web.Controllers.API.V1
{


    [Authorize]
    [RoutePrefix(Constants.RoutePrefixes.V1)]
    public class WidgetController : BaseApiController
    {
        private IWidgetService _widgetService;

        public WidgetController(IWidgetService widgetService)
        {
            _widgetService = widgetService.ThrowIfNull(nameof(widgetService));
        }

        /**
         * @api {get} /v1/widgets Get all widgets
         * @apiVersion 0.1.0
         * @apiName GetWidgets
         * @apiGroup Widget
         * 
         * @apiPermission lss.permission->list.widgets
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
         * @apiUse AuthHeader
         * @apiUse UnauthorizedError
         */
        [ClaimAuthorize(ClaimValue = LssClaims.ListWidgets)]
        [Route("widgets")]
        public IHttpActionResult Get()
        {
            var entityResult = _widgetService.GetWidgets();
            return CreateModelResult(entityResult);
        }


        /**
         * @api {get} /v1/widgets/:id Get a widget
         * @apiVersion 0.1.0
         * @apiName GetWidget
         * @apiGroup Widget
         *
         * @apiPermission lss.permission->view.widget
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
         * @apiUse NotFoundError
         * @apiUse AuthHeader
         * @apiUse UnauthorizedError
         */
        [ClaimAuthorize(ClaimValue = LssClaims.ViewWidget)]
        [Route("widgets/{id:int}", Name = "GetWidgetById")]
        public IHttpActionResult Get(int id)
        {
            var entityResult = _widgetService.GetWidget(id);
            return CreateModelResult(entityResult);
        }

        /**
         * @api {post} /v1/widgets Create a new widget
         * @apiVersion 0.1.0
         * @apiName CreateWidget
         * @apiGroup Widget
         * 
         * @apiPermission lss.permission->create.widget
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
         * @apiUse BadRequestError
         * @apiUse AuthHeader
         * @apiUse UnauthorizedError
         */
        [ClaimAuthorize(ClaimValue = LssClaims.CreateWidget)]
        [Route("widgets")]
        [HttpPost]
        public IHttpActionResult AddWidget([FromBody] WidgetModel widget)
        {
            var entityResult = _widgetService.AddWidget(widget);
            return CreatedEntityAt("GetWidgetById", () => new { Id = entityResult.Model.Id }, entityResult);
        }

        /**
         * @api {delete} /v1/widgets/:id Delete a widget
         * @apiVersion 0.1.0 
         * @apiName DeleteWidget
         * @apiGroup Widget
         * 
         * @apiPermission lss.permission->delete.widget
         * 
         * @apiParam {Number} id ID of the widget which will be deleted
         * 
         * @apiSuccessExample Success-Response:
         *      HTTP/1.1 204
         *      
         * @apiUse AuthHeader
         * @apiUse UnauthorizedError
         * 
         */
        [ClaimAuthorize(ClaimValue = LssClaims.DeleteWidget)]
        [Route("widgets/{id:int}")]
        [HttpDelete]
        public IHttpActionResult DeleteWidget(int id)
        {
            var entityResult = _widgetService.DeleteWidget(id);
            return NoContent(entityResult);
        }

        /**
         * @api {put} /v1/widgets/:id Update an existing widget
         * @apiVersion 0.1.0
         * @apiName UpdateWidget
         * @apiGroup Widget
         * 
         * @apiPermission lss.permission->update.widget
         * 
         * @apiParam {String} name Name of the widget
         * @apiParam {Number} id ID of the widget
         * @apiParam {Number} :id ID of the widget
         * 
         * @apiSuccess {Object} widget Updated widget
         * @apiSuccess {String} widget.name Name of the widget
         * @apiSuccess {Number} widget.id ID of the widget
         * 
         * @apiUse NotFoundError
         * @apiUse BadRequestError   
         * @apiUse AuthHeader
         * @apiUse UnauthorizedError
         */
        [ClaimAuthorize(ClaimValue = LssClaims.UpdateWidget)]
        [Route("widgets/{id:int}")]
        [HttpPut]
        public IHttpActionResult UpdateWidget([FromUri] int id, [FromBody] WidgetModel widget)
        {
            var entityResult = _widgetService.UpdateWidget(id, widget);
            return CreateModelResult(entityResult);
        }


    }
}
