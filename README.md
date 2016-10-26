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

## Database
The application uses a SQL Database and Code First Migrations. This migration strategy will be replaced with a TBD tool.

1. In Visual Studio, open the package manager console
2. Set the Default project to ***Launchpad.Data***
3. Run Update-Database from the console to create the database
   - The connection string in ***Launchpad.Web*** web.config determines where the database will be created
   - The default is localhost/sqlexpress with initial catalog Launchpad
   - When the web.config connection string is changed, update the connection string in ***Launchpad.Data.IntegrationTests***

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





# Setup
Once you have the source code:

1. Package restore
2. Switch to Package Manager Console
3. Set Default project to Launchpad.Data
4. Run Update-Database from the console
1. The connection string in LaunchPad.Web determines where the database will be created
2. The default is localhost/sqlexpress, initial catalog Launchpad

# Generate Documentation
To generate the api docs after a change:

1. (One Time) Run: npm install apidoc -g
2. npm run doc

The default route is setup to redirect to the documentation. 

# API Endpoints

## Widget
### Sample - Get Widgets
#### URL [http://localhost:52431/api/widget](http://localhost:52431/api/widget)
#### Output

```
[
  {
    "id": 3,
    "name": "Large Widget"
  },
  {
    "id": 7,
    "name": "Medium Widget"
  }
]
```

### Sample - Get Widget
#### URL [http://localhost:52431/api/widget/3](http://localhost:52431/api/widget/3)
#### Output
```
{
  "id": 3,
  "name": "Large Widget"
}
```

  
# Unit Testing
The unit testing libraries are:

1. [Xunit] (https://xunit.github.io/)
2. AutoFixture
3. FluentAssertions
4. MOQ
