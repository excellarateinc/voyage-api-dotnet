// ------------------------------------------------------------------------------------------
// General apiDoc documentation blocks and old history blocks.
// Reference: http://apidocjs.com/source/example_full/_apidoc.js
// ------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------
// Current Request.
// ------------------------------------------------------------------------------------------

/**
* @apiDefine UserRequestModel
*
* @apiParam {Object} user User
* @apiParam {String} user.userName Username of the user
* @apiParam {String} user.email Email
* @apiParam {String} user.firstName First name
* @apiParam {String} user.lastName Last name
* @apiParam {Object[]} user.phones User phone numbers
* @apiParam {String} user.phones.phoneNumber Phone number
* @apiParam {String} user.phones.phoneType Phone type
*
* @apiExample {json} Example body:
* {
*   "firstName": "FirstName",
*   "lastName": "LastName",
*   "username": "FirstName3@app.com",
*   "email": "FirstName3@app.com",
*   "phones": [{
*       "phoneType": "mobile",
*       "phoneNumber" : "5555551212"
*   }],
*   "isActive": true
* }
*
*/

// ------------------------------------------------------------------------------------------
// Current Success.
// ------------------------------------------------------------------------------------------


/**
* @apiDefine UserSuccessModel 
*
* @apiSuccess {Object} user User 
* @apiSuccess {String} user.id User ID
* @apiSuccess {String} user.userName Username of the user
* @apiSuccess {String} user.email Email
* @apiSuccess {String} user.firstName First name
* @apiSuccess {String} user.lastName Last name
* @apiSuccess {Object[]} user.phones User phone numbers
* @apiSuccess {String} user.phones.phoneNumber Phone number
* @apiSuccess {String} user.phones.phoneType Phone type  

* @apiSuccessExample Success-Response:
* {
*   "id": "f9d69894-7908-4606-918e-410dca8c3238",
*   "firstName": "FirstName",
*   "lastName": "LastName",
*   "username": "FirstName3@app.com",
*   "email": "FirstName3@app.com",
*   "phones": [
*       {
*           "id": 3,
*           "userId": "f9d69894-7908-4606-918e-410dca8c3238",
*           "phoneNumber": "5555551212",
*           "phoneType": "Mobile"
*       }
*   ],
*   "isActive": true
* }
*/

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
