## Development Recipes
Instructional recipies for how to do something within the codebase. 

> __Keep Organized__ Keep the Table of Contents alphabetized and do your best to extend this document in a way that will be easy to read/scroll for all developers.

## Table of Contents
* [APIDoc - Document A Web Service](#apidoc---document-a-web-service)
* [API Versioning](#api-versioning)
* [Audit - Enable DB Entity Change Tracking](#audit---enable-db-entity-change-tracking)
* [Creating a Controller](#creating-a-controller)
* [Creating a Service](#creating-a-service)
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

1. In ***Launchpad.Web*** execute npm run doc
   - This is an npm script that is defined in package.json
   - Script: apidoc -o docs -i .\\ -f \".cs$\" -f \"_apidoc.js\"
   - This will scan the Controllers folder for endpoints and place the output in \docs

To view the documentation either run the application and navigate to /docs/ or open the static index.html file.

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

Each entity must be configured for auditing. In the application, this is done by creating an IAuditConfiguration. There should be a 
configuration per entity that should be audited. There is a base class called BaseAuditConfiguration which by default will track
all properties on the entity. Overriding Configure allows a user to ignore certain properties on the entity. The configurations will be invoked from the DataModule at application start.

```
public class ApplicationUserAuditConfiguration : BaseAuditConfiguration<ApplicationUser>
{
        public override void Configure()
        {
            EntityTracker
                .TrackAllProperties<ApplicationUser>()
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

### Consuming Services
A pattern has been defined to standardize the response of services. Services which operate (Get, Add, Update, Delete) on an entity will return an EntityResult. The result will contain a model (if applicable) as well as properties indicating if the operating was successful. These properties can be used to determine which HttpStatusCode should be used as the result value. 

To cutdown on repeated logic, a base class has been created to handle the selection of the HttpStatusCode for common scenarios. The implementor is free to bypass these base methods as needed.

### BaseApiController
The BaseApiController is an abstract class that provides several helper methods for determining which IHttpActionResult should be returned to the client based on an EntityResult. By inheriting the class, the derived class will have access to the following methods:

| Method | Description |
|:----|:----|
|protected IHttpActionResult CreatedEntityAt`<TModel`>(string routeName, Func`<object`> routeValue, EntityResult`<TModel`> entityResult)|Creates a 201 Response with the entityResult.Model as the body and the location header set to the supplied route. |
|protected IHttpActionResult CheckErrorResult(EntityResult entityResult)|Check if the EntityResult contains an error. If there is an error, it will create a response with a 404 if the IsEntityNotFound flag is set. Otherwise, it will return a 400. In both cases, any entityResult.Errors will be added to the ModelState|
| protected IHttpActionResult CreateModelResult<TModel>(EntityResult<TModel> entityResult)| Creates a 200 response with the entityResult.Model as the body|
|protected IHttpActionResult NoContent(EntityResult entityResult)| Creates a 204 response|

Note: If the EntityResult contains an error, the appropriate error code will be returned instead.

Generally speaking, the following HttpMethods map to the following base methods.

| HttpMethod | Base Method | Description |
|:----|:----|:----|
|HttpGet|CreateModelResult|GETs should return a payload. When the GET specifies an ID and it is not found, the result will have the IsEntityNotFound flag set. The base method will then generate the 404.|
|HttpPut|CreateModelResult|Puts will operate on a specific instance of an entity and return a model|
|HttpPost|CreatedEntityAt|Posts include a Location header indicating the URL of the newly created resource|
|HttpDelete| NoContent| A delete does not have a response body|

Providing a common base impelementation allows the application to standardize the interpretation of EntityResults as well as standardize the responses that are returned to the client.

### Implementation
The following steps provide guidance around adding a new service

1. Add a new class file to Launchpad.Web in the correct version folder
   1. The class should end in the suffix Controller
2. Inherit from BaseApiController
3. Add class and method attributes
4. Add dependencies on services
5. Invoke the service and pass the result to the appropriate base method to generate the correct IHttpActionResult

Sample Method
```
        public IHttpActionResult GetRoleById(string roleId)
        {
            var entityResult = _roleService.GetRoleById(roleId);
            return CreateModelResult(entityResult);
        }
```
:arrow_up: [Back to Top](#table-of-contents)

## Creating a Service
Services (Not Api Services) evaluate and execute the business logic in the application. They can be used as depenendencies in ApiControllers as well as other services. A pattern has been established for the return value of services. The goal is to standardize the expected shape of the result of a method call on a service. This will help form a consistent model of how the service layer as a whole operates regardless of underlying business logic. 

### EntityResult and EntityResult`<TModel>`
When a service acts upon an entity within the database, it should use an EntityResult as the method return type. This class encapsulates both the resulting model as well as whether or not the method was successful. This allows common logic to be written to consume any EntityResult - or in otherwords this allows the controller to make a service call and then call common logic to map the result into a response.

The class draws upon the IdentityResult that is found in the ASP.Net Identity framework. The properties of the class are explained below.

| Property  | Description |
|:----|:----|
|TModel|The result of the operation. Note: This is only in the generic version.|
|Errors|List of errors that occured during the operation. These errors should be added using the WithErrorCodeMessage to ensure correct response formatting|
|Succeeded|Boolean indicating if the operation was a success|
|IsEntityNotFound|Boolean indicating if the operation failed due an entity not being found|

The class has the following methods:

| Method | Description |
|:----|:----|
|public EntityResult WithErrorCodeMessage(string code, string message)|Helper method for adding error messages. These errors will be structured to ensure correct response formatting.|
| public EntityResult WithEntityNotFound(object id)|Helper method for adding an error message for not found entity. This method help ensures a standard format for any not found entities|

Note: The WithErrorCodeMessage has a similar signature to the extension used for adding error messages for validation rules. 

### EntityResultService
The EntityResultService is an abstract class that services can inherit to provide access to several methods that help ensure consistent creation of EntityResults. 

| Method | Description |
|:----|:----|
|protected EntityResult NotFound(object id)| Generates a result that represents a not found entity |
|protected EntityResult`<TModel`> NotFound`<TModel`>(object id)| Generates a result that represents a not found entity|
|protected EntityResult`<TModel`> Success`<TModel`>(TModel model)| Genereates a result that represents a succssful operation with a model output|
|protected EntityResult<TModel> FromIdentityResult<TModel>(IdentityResult result, TModel model)| Converts an IdentityResult to a EntityResult|
|protected EntityResult FromIdentityResult(IdentityResult result)| Converts an IdentityResult to an EntityResult|

### Implementation
The following steps provide guidance around adding a new service

1. Add a new class to Launchpad.Services
  1. This file should end in the suffix Service to indicate it is a service
2. If the service handles operations on an entity (User, Role, ect.) inherit from EntityResultService
3. Add a new interface to Launchpad.Services
  1. This interface will be the contract that the aformentioned class implements. 
  2. Define the methods for the service. If the method operates on an entity, it should have a result type of EntityResult
4. Implement in interface in the class
5. Utilize the base methods to create the EntityResult 

Sample Method
```
       public EntityResult<RoleModel> GetRoleById(string id)
       {
            //Attempt to find the role by id
            var role = _roleManager.FindById(id);

            return role == null ?
                NotFound<RoleModel>(id) :
                Success(_mapper.Map<RoleModel>(role));

        }
```
In the above example, the method will return an EntityResult containing the model if the role is found. Otherwise, it will generate a failure result indicating that the requested role was not found.

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

