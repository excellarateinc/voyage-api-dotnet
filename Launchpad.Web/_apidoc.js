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
 * [
 *   {
 *     "code": "missing.required.field",
 *     "field": "userModel.LastName",
 *     "description": "Last name is a required field"
 *   },
 *   {
 *     "code": "invalid.email",
 *     "field": "userModel.Email",
 *     "description": "Last name is a required field"
 *   }
 * ]
 */

/**
*   @apiDefine NotFoundError
*
*   @apiError NotFound The requested resource was not found
*
*   @apiErrorExample Error-Response
*   HTTP/1.1 404: Not Found
*/

/**
*   @apiDefine UnauthorizedError
*   
*   @apiError Unauthorized The user is not authenticated.
*   
*   @apiErrorExample Error-Response
*       HTTP/1.1 400: Unauthorized
*       {
*           "message": "Authorization has been denied for this request."
*       }
*/

// ------------------------------------------------------------------------------------------
// Current Headers.
// ------------------------------------------------------------------------------------------

/**
* @apiDefine AuthHeader
*
* @apiHeader {String} Authorization Authentication token returned from the /api/Token service 
* 
* @apiHeaderExample Header-Example:
*   {
*    
*       "Authorization": "bearer 5_leQJfkDVIFxRgt-YZcnRGvzlrsFdNXClRaZEaTf3jzToaoFAUtOyneKNhtYo2cCSC9_fYXXBMS3UHwY3MZuzRbWmLqG_H1T5Cm6YHUzzQHPpKCCUqcYMx6m3NMLQoGVX9_f2svSSY4HieOTzwT-LTfxYbxHyuiRY0sJrmoGnQOtWGGC9GhdUXPCpM9_1L8p0vdAC1wnnTKtkDkxulAUkIxj3jJIUc9lUhqLoh5BRYJ8VsABsKEZYouLWvYfL5eqkL7EjwtxRNBhy9rJSU7nc_6lNCk5PEpsFg-laLYWeVO10gVFBkI3Ml7I8KgJA2Bs3vKKccIxuOwqoWjU_XIhV3f9Lhjm34OqSrwcThfx940k5TEWjHG9CHqxnItRvgHtqahc2rP8q0QuZ0qw6Ph5kxEE18YVGa8IVX7zAPW8V6WtXnVnR7SbZwnwiNTuh3i0t0Ib_gOKZAwUZTrVhsTqnFe20zMvJIsOzTtijl80bJMF7ZBAbKKL6zKfJcQ_DwYJaM0CISapXGjbJYMNWPaOzg6kTKtVuHb4akxfxlsAUk0QFqxZ793TT-KCdh-c0ppUNb3brPMh8wGpJDhwLbnhUfpBVnuxdC-sDrGUtzpSnMrPavKivovVT-sXBbqzeg6udOhjN3JXIv2ZzctisLYTw"
*    
*   } 
*/


// ------------------------------------------------------------------------------------------
// Current Permissions.
// ------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------
// History.
// ------------------------------------------------------------------------------------------
