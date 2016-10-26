# launchpad-dotnet-api
Launchpad API

# Development Environment

## Tools

The following tools should be installed on the development machine:

1. Visual Studio 2015
2. SQL Server 
   - Checked in connection strings point at SQLExpress 
   - Default instance: localhost\sqlexpress
3. [Node](https://nodejs.org/en/)
4. [Karma Client](http://karma-runner.github.io/1.0/intro/installation.html)
   - npm install -g karma-Client
5. [apiDocJs](http://apidocjs.com/)
   - npm install -g apidoc 

## Source code
After pulling the source code install the required packages: 

1. Pull the source code from github repo
2. Perform a Nuget package restore
3. Perform a npm package restore in folder ***Launchpad.Web***

# Building

## Database
The application uses a SQL Database and Code First Migrations. This migration strategy will be replaced with a TBD tool.

1. In Visual Studio, open the package manager console
2. Set the Default project to ***Launchpad.Data***
3. Run Update-Database from the console to create the database
   - The connection string in ***Launchpad.Web*** web.config determines where the database will be created
   - The default is localhost/sqlexpress with initial catalog Launchpad
   - When the web.config connection string is changed, update the connection string in ***Launchpad.Data.IntegrationTests***

## Users
Currently, there is not an UI for adding users. However, a user can be added using a REST request:

1. Build and start the application
2. Open a rest client (For example: Postman)
3. Create a post request
   - The first time the IdentityContext is used, it will create the ASP.Net Identity schema in the database
   - This should return a 200

```
POST http://localhost:52431/api/account/Register HTTP/1.1
Content-Type: application/json
Accept: */*

{
	"email":"someemail@agreatdomain.com",
	"password":"SuperTopSecret123!",
	"confirmPassword": "SuperTopSecret123!"
}
```

## Generating Documentation
The web api controllers are using inline documentation called apiDoc. The documentation is embedded as comments above each controller method. For 
more details see the [documentation](http://apidocjs.com/).

### Sample Documentation Comments
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


#### Current @apiDefine blocks

1. BadRequestError
   - Used when an endpoint can return a 400
2.  NotFoundError
    - Used when an endpoint can return a 404

### Generating documentation
To generate the api docs after a change:
1. In ***Launchpad.Web*** execute npm run doc
   - This is an npm script that is defined in package.json
   - Script: apidoc -o docs -i Controllers
   - This will scan the Controllers folder for endpoints and place the output in \docs

To view the documentation either run the application and navigate to /docs/ or open the static index.html file.

# Testing
The unit testing libraries are:

1. [Xunit] (https://xunit.github.io/)
2. AutoFixture
3. FluentAssertions
4. MOQ
