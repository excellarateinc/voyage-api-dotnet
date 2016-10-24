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
/**
 * @apiDefine BadRequestError
 *
 * @apiError BadRequest The input did not pass the model validation.
 *
 * @apiErrorExample Error-Response:
 *  HTTP/1.1 400: Bad Request
 *  {
 *      "message": "The request is invalid.",
 *      "modelState": {
 *          "widget.Name": [
 *              "A widget must have a name"
 *          ]
 *      }
 *  }
 */

/**
*   @apiDefine NotFoundError
*
*   @apiError NotFound The requested resource was not found
*
*   @apiErrorExample Error-Response
*   HTTP/1.1 404: Not Found
*/




// ------------------------------------------------------------------------------------------
// Current Permissions.
// ------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------
// History.
// ------------------------------------------------------------------------------------------
