## Development Recipes
Instructional recipies for how to do something within the codebase. 

> __Keep Organized__ Keep the Table of Contents alphabetized and do your best to extend this document in a way that will be easy to read/scroll for all developers.

## Table of Contents
* [APIDoc - Document A Web Service](#apidoc---document-a-web-service)
* [API Versioning](#api-versioning)
* [Audit - Enable DB Entity Change Tracking](#audit---enable-db-entity-change-tracking)
* [Consuming API Services](#consuming-api-services)
* [Creating a Controller](#creating-a-controller)
* [Creating a Service](#creating-a-service)
* [API Exceptions](#api-exceptions)
* [HTTP Request - Validate Request Data](#http-request---validate-request-data)


## APIDoc - Document A Web Service
Web service documentation for consumers is facilitated through the use of a framework called [apiDoc](http://apidocjs.com). apiDoc provides a set of annotation that are placed in a comment block within the web service controller class. To generate the documentation website for consumers, apiDoc provides a Node script that scans the source files for apiDoc annotations to create a pretty HTML website. 

The complete documenation of apiDoc can be found on their website [http://apidocjs.com](http://apidocjs.com/).

### Example
Below is an example of the comments used to document an endpoint.

```
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
        ...
```

### Reusable apiDoc blocks
apiDoc supports creating reusuable documentation blocks using [@apiDefine](http://apidocjs.com/#param-api-define). This 
cuts down on repeated comment blocks for shared elements such as errors. 
All reusable blocks should be placed in  ***Launchpad.Web\\_apidoc.js***

### Current @apiDefine blocks

#### Headers
1. AuthHeader
    - Used when an API requires the authorization header 
    
#### Errors
1. BadRequestError
   - Used when an API can return a 400
2.  NotFoundError
    - Used when an API can return a 404
3. UnauthorizedError
    - Used when an API can generate a 401

#### Request Params
1. UserRequestModel
    - Used when an API takes a user as input

#### Response Params
1. UserSuccessModel
    - Used when an API returns a single user

    
### Generating documentation
To generate the api docs after a change:

1. In ***Launchpad.Web/apidoc*** execute npm run apidoc
   - This is an npm script that is defined in package.json
   - Script: apidoc -o docs -i .\\ -f \".cs$\" -f \"_apidoc.js\"
   - This will scan the Controllers folder for endpoints and place the output in \docs

To view the documentation either run the application and navigate to /apidoc/docs/ or open the static index.html file.

:arrow_up: [Back to Top](#table-of-contents)

## API Versioning
API versioning is handled through URL versioning. See the [Web Service Pattern for API Versioning](WEB-SERVICE-PATTERNS.md#versioning). 

Each version of the an api will have a new controller source file and a unique url that contains the version. The routing for these versions is handled via attributes. The steps for creating a new version of an API are roughly as follows:

1. If a subfolder does not exist for a version, create it 
   - \v1, \v2, \v3...
2. Add a route prefix to the static RoutePrefixes class
3. Create the a new controller
4. Add the RoutePrefix attribute at the class level
5. Add the Route attribute to each operation, specifying the route template

:arrow_up: [Back to Top](#table-of-contents)

## Audit - Enable DB Entity Change Tracking
> __VALIDATE THIS APPROACH__

Data auditing is implemented using the [Tracker Enabled DbContext](https://github.com/bilal-fazlani/tracker-enabled-dbcontext)
nuget package. This package includes a custom DbContext called TrackerIdentityContext. The LaunchpadDataContext inherits from this class. 
When save changes is called on the context, the ChangeTracker is used to create audit records. 

Each entity must be configured for auditing. In the application, this is done by adding a row to the BaseAuditConfiguration.cs file. There should be a 
row per entity that should be audited. The configurations will be invoked from the DataModule at application start.

```
public static class BaseAuditConfiguration
{
        public override void Configure()
        {
            EntityTracker.TrackAllProperties<ApplicationUser>()
                .Except(_ => _.PasswordHash);
        }
}
```

#### Soft Deletes
Auditing will track soft deletes if the model implements ISoftDeleteable. When the Deleted flag is changed to true, it will be tracked as SoftDeleted and when the flag is changed to false it will be tracked as UnDeleted. Additional information can be found [here](https://github.com/bilal-fazlani/tracker-enabled-dbcontext/wiki/8.-Soft-Deletable)

#### Database Tables
The nuget package defines the tables that store the auditing records.

| Column | Description | 
|:----|:----|
| core.AuditLog | Creates the header record for the auditing operation |
| core.AuditLogDetail | Creates the change details |
| core.LogMetadata| Stores custom metedata (unused) |

The event types map to the following enumeration:

| enum  | Value |
|:----|:----|
|Added|0|
|Deleted|1|
|Modified|2|
|SoftDeleted|3|
|UnDeleted|4|

:arrow_up: [Back to Top](#table-of-contents)

## Consuming API Services
There are two type of services available via the API: anonymous and secure services. Anonymous services are those that can be used without any authentication. Meanwhile, secure services are those that require a bearer token in order to process the request.

### Anonymous Services
Anonymous services can be used without including an Authorization header. The following endpoints are anonymous:

| Path | Description | 
|:----|:----|
| /api/vX/login | Attempts to authentication the user and returns an authorization token on success.|
| /api/vX/account/register | Registers a new user|
| /api/vX/statuses | Returns application status information | 

Each of the services can be used by issuing a standard HTTP Request. For more information, see the API documentation for details on consuming these services.

#### Sample Anonymous Request

```
GET http://54.196.167.24/api/v1/statuses HTTP/1.1
Host: 54.196.167.24
Connection: keep-alive
Cache-Control: no-cache
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.99 Safari/537.36
Postman-Token: 554afee4-3092-c84e-abb6-5a9467a38c52
Accept: */*
Accept-Encoding: gzip, deflate, sdch
Accept-Language: en-US,en;q=0.8
```

#### Sample Anonymous Response
```
HTTP/1.1 200 OK
Content-Length: 29
Content-Type: application/json; charset=utf-8
Server: Microsoft-IIS/7.5
X-Powered-By: ASP.NET
Date: Tue, 06 Dec 2016 21:43:57 GMT

{"buildNumber":"some_number"}
```

### Secure Services
Secure services need to have a special header included in the request. The header is:

```
{
    "Authorization": "bearer {Token}"
}
```
where {Token} is the access_token returned from the login service.

The workflow for consuming these sevices is as follows:

1. Call the Login api with the username and password
2. Upon a 200 response, save the access_token
3. Create a new request and set the Authorization header to bearer + access_token
4. Configure remaining properties of the request
5. Execute the request

**Note:** The access_token can be reused for multiple requests. It is unnecessary to call Login prior to every secure request. 

#### Sample Login Request
```
POST http://54.196.167.24/api/v1/login HTTP/1.1
Host: 54.196.167.24
Connection: keep-alive
Content-Length: 65
Postman-Token: 63193519-f0b2-b6eb-b905-939487bc103f
Cache-Control: no-cache
Origin: chrome-extension://fhbjgbiflinjbdggehcddcbncdddomop
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.99 Safari/537.36
Content-Type: application/x-www-form-urlencoded
Accept: */*
Accept-Encoding: gzip, deflate
Accept-Language: en-US,en;q=0.8

grant_type=password&username=admin%40admin.com&password=Hello123!

```

#### Sample Login Response

```
HTTP/1.1 200 OK
Cache-Control: no-cache
Pragma: no-cache
Content-Length: 921
Content-Type: application/json;charset=UTF-8
Expires: -1
Server: Microsoft-IIS/7.5
Access-Control-Allow-Origin: chrome-extension://fhbjgbiflinjbdggehcddcbncdddomop
Access-Control-Allow-Credentials: true
X-Powered-By: ASP.NET
Date: Tue, 06 Dec 2016 21:47:04 GMT

{"access_token":"eSqrWlnTcxdMBEB_MO-i812xCBysFGidFy2KHsYjRDbP21vBT5XpuHd3f_fxAYMvyuDhC84S02oAAzd8y4JO07R-R4svWnMiF38EOrJIpKJeP9S-aEs9TVpq7LQEJ1fZlSVcDZV_xttBTetveordQl0vKF7021fXCu05N-X4Y_SIQmVliiwk3v4xSx8skrobV4HBEDyiPEjYut04l_9j_m9BiEzfuGp0_B_o8phUu29SLRMhbtCrGHtCxrfW0BqcsRE3eerp2w2U-ynalVAWgTH339CmWFRK44WPgfpTUVNwnKnj2mr40iglYqKCi-ifxlA_9F4dNfJ4ixQj4QpIjXkAV9_WB1bsCnl-0cVsbWAvmHtIrXQKy9LKt7KykncxuCQlOMgt0K8BH-T9kSffiU2xpnBa9pcYsAIVN1ObZpkjV9RhyVwEyNphIEpUqHwir_fRMBtuSiH1gG5v3H60Vdr_cROTui9x-fhizM-s3ZnlviQhHc1qbhAIA48zYm8GOO8XO_z2H8zS-t94NVthBd1Z3QtI-024HmmXd9NzRKg0A30xSQsE0URm7-2jUN1HUwVNbNuDjXEf05FR2yfGnxoIWA5ZP25ozy_3UBsFXXWAiEDumi6Kk57M2B-xoINzxfZIJvcd_H_9EObGG8G_bcyAXHKnbu0Gwrbhb3Zwv4fslPI1diMXox4kJZtYxEOTjCSaZQlM9sq7I64Fl6apzj_PKLtRw1JMOgKLLzF9Ceo","token_type":"bearer","expires_in":86399,"userName":"admin@admin.com",".issued":"Tue, 06 Dec 2016 21:46:56 GMT",".expires":"Wed, 07 Dec 2016 21:46:56 GMT"}

```

#### Sample Secure Request

```
GET http://54.196.167.24/api/v1/roles HTTP/1.1
Host: 54.196.167.24
Connection: keep-alive
Authorization: bearer eSqrWlnTcxdMBEB_MO-i812xCBysFGidFy2KHsYjRDbP21vBT5XpuHd3f_fxAYMvyuDhC84S02oAAzd8y4JO07R-R4svWnMiF38EOrJIpKJeP9S-aEs9TVpq7LQEJ1fZlSVcDZV_xttBTetveordQl0vKF7021fXCu05N-X4Y_SIQmVliiwk3v4xSx8skrobV4HBEDyiPEjYut04l_9j_m9BiEzfuGp0_B_o8phUu29SLRMhbtCrGHtCxrfW0BqcsRE3eerp2w2U-ynalVAWgTH339CmWFRK44WPgfpTUVNwnKnj2mr40iglYqKCi-ifxlA_9F4dNfJ4ixQj4QpIjXkAV9_WB1bsCnl-0cVsbWAvmHtIrXQKy9LKt7KykncxuCQlOMgt0K8BH-T9kSffiU2xpnBa9pcYsAIVN1ObZpkjV9RhyVwEyNphIEpUqHwir_fRMBtuSiH1gG5v3H60Vdr_cROTui9x-fhizM-s3ZnlviQhHc1qbhAIA48zYm8GOO8XO_z2H8zS-t94NVthBd1Z3QtI-024HmmXd9NzRKg0A30xSQsE0URm7-2jUN1HUwVNbNuDjXEf05FR2yfGnxoIWA5ZP25ozy_3UBsFXXWAiEDumi6Kk57M2B-xoINzxfZIJvcd_H_9EObGG8G_bcyAXHKnbu0Gwrbhb3Zwv4fslPI1diMXox4kJZtYxEOTjCSaZQlM9sq7I64Fl6apzj_PKLtRw1JMOgKLLzF9Ceo
Cache-Control: no-cache
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.99 Safari/537.36
Postman-Token: 28d8eeb2-8d05-c248-0b68-66589d8f7088
Accept: */*
Accept-Encoding: gzip, deflate, sdch
Accept-Language: en-US,en;q=0.8
```

#### Sample Secure Response
```
HTTP/1.1 200 OK
Content-Length: 1751
Content-Type: application/json; charset=utf-8
Server: Microsoft-IIS/7.5
X-Powered-By: ASP.NET
Date: Tue, 06 Dec 2016 21:47:50 GMT

[{"id":"3a408ee9-e99d-4ca3-bc5d-a1dec54d6d85","name":"Administrator","claims":[{"claimType":"lss.permission","claimValue":"assign.role","id":3},{"claimType":"lss.permission","claimValue":"create.role","id":4},{"claimType":"lss.permission","claimValue":"delete.role","id":5},{"claimType":"lss.permission","claimValue":"list.roles","id":6},{"claimType":"lss.permission","claimValue":"revoke.role","id":7},{"claimType":"lss.permission","claimValue":"view.claim","id":8},{"claimType":"lss.permission","claimValue":"list.users","id":9},{"claimType":"lss.permission","claimValue":"list.user-claims","id":10},{"claimType":"lss.permission","claimValue":"view.user","id":11},{"claimType":"lss.permission","claimValue":"update.user","id":12},{"claimType":"lss.permission","claimValue":"delete.user","id":13},{"claimType":"lss.permission","claimValue":"create.user","id":14},{"claimType":"lss.permission","claimValue":"list.widgets","id":15},{"claimType":"lss.permission","claimValue":"list.role-claims","id":16},{"claimType":"lss.permission","claimValue":"delete.role-claim","id":17},{"claimType":"lss.permission","claimValue":"create.claim","id":18},{"claimType":"lss.permission","claimValue":"view.role","id":19},{"claimType":"lss.permission","claimValue":"view.widget","id":20},{"claimType":"lss.permission","claimValue":"update.widget","id":21},{"claimType":"lss.permission","claimValue":"create.widget","id":22},{"claimType":"lss.permission","claimValue":"delete.widget","id":23}]},{"id":"1194fe58-7581-4273-badf-be2a29b939a3","name":"Basic","claims":[{"claimType":"lss.permission","claimValue":"login","id":1},{"claimType":"lss.permission","claimValue":"list.user-claims","id":2}]},{"id":"d090fa99-b987-412d-aad3-6c0090569098","name":"Super","claims":[]}]
```

:arrow_up: [Back to Top](#table-of-contents)


## Creating a Controller
Creating a controller will expose a new API endpoint. Controllers should be concerned with the API endpoint route and returning an appropriate HttpStatusCode. They should depend on services to execute to the business logic and return an object that represents that result that should be passed to the client.

### Attributes
When adding a new controller, there are several attributes that should be used to decorate the class. The table below describes these attributes.

| Attribute  | Scope | Description |
|:----|:----|:----|
|AuthorizeAttribute|Class|Any secure API endpoint should have this attribute. It will ensure that the user has been authenticated|
|RoutePrefixAttribute|Class|The route prefix attribute is used with URL versioning to indicate the controller's version|
|ClaimAuthorizeAttribute|Method|The attribute is used to define the required claim needed to execute the controller method|
|RouteAttribute|Method|The attribute is used to define the route of the API endpoint|
|Http{Verb}Attribute|Method|The attribute is used to define the HTTP method used to invoke the method|

### Implementation
The following steps provide guidance around adding a new controller.

1. Add a new class file to Launchpad.Web in the correct version folder
   1. The class should end in the suffix Controller
2. Add class and method attributes
3. Add dependencies on services
4. Invoke the service and pass the result to the appropriate Api Controller method.

Sample Method
```
        public IHttpActionResult GetRoleById(string roleId)
        {
            var result = _roleService.GetRoleById(roleId);
            return Ok(result);
        }
```
:arrow_up: [Back to Top](#table-of-contents)

## Creating a Service
Services (Not Api Services) evaluate and execute the business logic in the application. They can be used as dependencies in ApiControllers as well as other services.

### Implementation
The following steps provide guidance around adding a new service

1. Add a new class to Launchpad.Services
  1. This file should end in the suffix Service to indicate it is a service
2. Add a new interface to Launchpad.Services
  1. This interface will be the contract that the aformentioned class implements. 
  2. Define the methods for the service.
3. Implement in interface in the class
4. If an exceptional case happens (item isn't found, user passes bad values into the method, etc) throw the appropriate API Exception.

Sample Method
```
       public RoleModel GetRoleById(string id)
       {            
            var role = _roleManager.FindById(id);

            if (role == null)
                throw new NotFoundException($"Could not locate entity with Id {id}");

            return _mapper.Map<RoleModel>(role);
        }
```
In the above example, the method will return the model if the role is found. Otherwise, it will throw a NotFoundException, which will generate a failure result indicating that the requested role was not found.

:arrow_up: [Back to Top](#table-of-contents)

## API Exceptions
In order to maintain consistency around error messaging in the API, there are a set of exception classes that make generating a standard response object to send to the client simple. This approach has multiple benefits including:

1. Consistent, standardized error messaging for the client to consume.
2. Error cases that happen deep in the service layer are easy to handle and bubble up to the top of the stack.
3. The business layer does not know about web concerns (ex: http status codes).

These exceptions extend the ApiException class. There is an exception filter in the .NET pipeline that intercepts any exception thrown with this base type and creates an http response with the correct error code and message.

### How to Use
The following steps provide guidance around using the API Exceptions.

1. Create a service method to perform some action.
2. When the exceptional case occurs, check for it (example: role comes back null)
3. Throw the appropriate exception. In this case, NotFoundException makes sense.

Sample Method
```
       public RoleModel GetRoleById(string id)
       {            
            var role = _roleManager.FindById(id);
            
            // Role wasn't found, throw exception to let the client know.
            if (role == null)
                throw new NotFoundException($"Could not locate entity with Id {id}");            
        }
```

### Creating New API Exceptions
The most common exceptions live in the "Exceptions" folder in Voyage.Core. In the case you need to create a new one to suit your needs, do the following.

1. Create a new class in the "Exceptions" folder with the name ending in "Exception".
2. Extend the ApiException class.
3. Extend the appropriate ApiException constructors.

This exception will now be able to be thrown anywhere in the application.

Sample Exception
```
       public class UnauthorizedException : ApiException
       {
           public UnauthorizedException()
               : base(HttpStatusCode.Unauthorized)
           {
           }

           public UnauthorizedException(string message)
               : base(HttpStatusCode.Unauthorized, Constants.ErrorCodes.Unauthorized, message)
           {
           }
       }
```

:arrow_up: [Back to Top](#table-of-contents)

## HTTP Request - Validate Request Data
[FluentValidation](https://github.com/JeremySkinner/FluentValidation) is used to perform HTTP Request input validations.

From a .NET perspective, the standard ModelState validation is used. The model state dictionary will be populated with the validation 
errors. An ActionFilterAttribute will then transform the dictionary into the expected output.

#### Creating A Validator

* In Launchpad.Models create a new class {Model}Validator under the validators folder
* Inherit AbstractValidator<TModel>
* Configure rules using the fluent API
```
    RuleFor(_ => _.Username)
        .NotEmpty()
        .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "Username is a required field");

    RuleFor(_=>_.Email)
        .NotEmpty()
        .WithErrorCodeMessage(Constants.ErrorCodes.MissingField, "Email is a required field")
        .EmailAddress()
        .WithErrorCodeMessage(Constants.ErrorCodes.InvalidEmail, "Email is invalid");
```
 * Decorate the model with the ValidatorAttribute 
```
    [Validator(typeof(UserModelValidator))]
    public class UserModel
```
 * Create a corresponding test file in the test project. Use the [extension methods](https://github.com/JeremySkinner/FluentValidation/wiki/g.-Testing) to write tests for each validation rule.

```
    [Fact]
    public void Should_Have_Error_When_Name_Is_Null()
    {
        _validator.ShouldHaveValidationErrorFor(role => role.Name, null as string);
    }
```

#### Validation Response
Validation errors will be returned as a 400 Bad Request response. The body of the response will be an array with items  
that have the following structure:

```
  {
    code: 'String that represents the type of error',
    description: 'An english description of the error',
    field: 'The property that generated the error'
  }
```

For example:

```
  {
    "code": "missing.required.field",
    "field": "model.FirstName",
    "description": "First name is a required field"
  }
```

The transformation occurs in the ValidateModelAttribute.

Due to the way model state works, the code must be embedded into the validation error message. This is accomplished 
via the WithErrorCodeMessage which will take an error code and a message and create a string with the format of errorCode::message. 
During the transformation, this message will be split into the code and description.

```
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> WithErrorCodeMessage<T,TProperty>(this IRuleBuilderOptions<T,TProperty> options, string code, string message)
        {
            options.WithMessage("{0}::{1}", code, message);
            return options;
        }
    }
```

```
    public static BadRequestErrorModel ToModel(this ModelError error, string field)
    {
        var model = new BadRequestErrorModel();
        model.Field = field;

        var codedMessage = error.ErrorMessage.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
        if(codedMessage.Length == 2)
        {
            model.Code = codedMessage[0];
            model.Description = codedMessage[1];
        }
        else
        {
            model.Description = error.ErrorMessage;
        }
        return model;
    }
```

While the encoding is is not necessarily desirable, being able to utilize the standard ModelState validation of ASP.Net is easier 
than implementing a custom validation framework. It will also be familiar to .Net developers.

:arrow_up: [Back to Top](#table-of-contents)

