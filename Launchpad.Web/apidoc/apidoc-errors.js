// ------------------------------------------------------------------------------------------
// Common apiDoc elements
// -- Reusable definitions
// -- Current version of an API definition
// -- Historical versions of API definitions (for comparison, if necessary)
// ------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------
// Bad Request Error
// ------------------------------------------------------------------------------------------
/**
 * @apiDefine BadRequestError
 *
 * @apiError BadRequest The input did not pass the model validation.
 *
 * @apiErrorExample Error-Response:
 * HTTP/1.1 400: Bad Request
 * [
 *     {
 *         "code": "user.lastName.required",
 *         "description": "User lastName is a required field"
 *     },
 *     {
 *         "code": "user.email.invalidFormat",
 *         "description": "User email format is invalid. Required format is text@example.com"
 *     }
 * ]
 */

// ------------------------------------------------------------------------------------------
// Not Found Error
// ------------------------------------------------------------------------------------------
/**
 * @apiDefine NotFoundError
 *
 * @apiError NotFound The requested resource was not found
 *
 * @apiErrorExample Error-Response
 * HTTP/1.1 404: Not Found
 * {
 *     "code": "error.404_not_found",
 *     "description": "Could not locate entity with ID 11bef1e7-3ba3-4669-861e-54e91fd8db79"
 * }
 */

// ------------------------------------------------------------------------------------------
// Unauthorized Error
// ------------------------------------------------------------------------------------------
/**
 *  @apiDefine UnauthorizedError
 *
 *  @apiError Unauthorized The user is not authenticated.
 *
 *  @apiErrorExample Error-Response
 *  HTTP/1.1 400: Unauthorized
 *  {
 *      "code": "error.400_unauthorized",
 *      "description": "Authorization has been denied for this request."
 *  }
 */
