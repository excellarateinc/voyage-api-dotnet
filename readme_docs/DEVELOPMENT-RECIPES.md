## Development Recipes
Instructional recipies for how to do something within the codebase. 

> __Keep Organized__ Keep the Table of Contents alphabetized and do your best to extend this document in a way that will be easy to read/scroll for all developers.

## Table of Contents
* [APIDoc - Document A Web Service](#apidoc---document-a-web-service)
* [API Versioning](#api-versioning)
* [Audit - Enable DB Entity Change Tracking](#audit---enable-db-entity-change-tracking)
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
All reusable blocks should be placed in  ***Launchpad.Web\Controllers\_apidoc.js***

### Current @apiDefine blocks
1. BadRequestError
   - Used when an endpoint can return a 400
2.  NotFoundError
    - Used when an endpoint can return a 404

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
