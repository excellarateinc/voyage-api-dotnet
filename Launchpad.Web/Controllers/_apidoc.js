// ------------------------------------------------------------------------------------------
// General apiDoc documentation blocks and old history blocks.
// Reference: http://apidocjs.com/source/example_full/_apidoc.js
// ------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------
// Current Success.
// ------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------
// Current Errors.
// ------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------
// Current Permissions.
// ------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------
// History.
// ------------------------------------------------------------------------------------------

/**
        * @api {get} /widget/:id Request Widget information
        * @apiVersion 0.1.0
        * @apiName GetWidget
        * @apiGroup Widget
        *
        * @apiParam {Number} id Widget's unique ID!.
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
        * @apiError WidgetNotFound The Widget with the requested id was not found.
        * 
        * @apiErrorExample Error-Response:
        *     HTTP/1.1 404 Not Found
        *     {
        *       "message": "Widget with ID 33 not found"
        *     }
        */

/**
        * @api {get} /widget Get all widgets
        * @apiVersion 0.1.0
        * @apiName GetWidgets
        * @apiGroup Widget
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