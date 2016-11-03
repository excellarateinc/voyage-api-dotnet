## Table of Contents
* [About](#about)
* [Web Services Patterns](WEB-SERVICES-PATTERNS.md)

## About
Web Services API for the Launchpad app

## Development Environment

### Tools

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

### Source code
After pulling the source code install the required packages: 

1. Pull the source code from github repo
2. Perform a Nuget package restore
3. Perform a npm install in folder ***Launchpad.Web***

## Build

### Database
The application uses a SQL Database and Code First Migrations. This migration strategy will be replaced with a TBD tool.

1. In Visual Studio, open the package manager console
2. Set the Default project to ***Launchpad.Data***
3. Run Update-Database from the console to create the database
   - The connection string in ***Launchpad.Web*** web.config determines where the database will be created
   - The default is localhost/sqlexpress with initial catalog Launchpad
   - When the web.config connection string is changed, update the connection string in ***Launchpad.Data.IntegrationTests***

### Users
A default user will be seeded into the database when Update-Database is run. The user is *admin@admin.com* / *Hello123!* - new users can be created using the Register link. 

Each new user will be created with a 'Basic' role whcih allows them to log in. You may notice on the dashboard a 403 (Forbidden) is generated. The basic role does not have the 'list.widgets' claim. To resolve this and see the widgets, login as the admin and select Add Claim. Choose 'Basic' as the role and enter 'lss.permission' and 'list.widgets' for the claim type and claim value respectively. 


### Server Side Development Notes
The server exposes 3 endpoints:
- api/account: Used for user management
- api/{v}/widget: Simple example of CRUD operations on an entity
- api/status: Used to query application statuses 

#### Dependency Injection
The application is using Autofac as the DI container. Each project contains a module file that is responsible for registering the contained
components with the container. The overall container is setup in the ContainerConfig in ***Launchpad.Web***.

The lifetime for the majority of components should be per request to keep request scopes isolated. Autofac will handle diposing the resolved
values the end of the request. For more: [http://docs.autofac.org/en/latest/lifetime/disposal.html](http://docs.autofac.org/en/latest/lifetime/disposal.html)

Autofac supports assembly scanning. This allows the registration of all components that implement a particular interface. For example the repositories
are registered using scanning. This can simplify the container configuration through the automatic registration via convention. (All repositories implement IRepository, ect.)

```
  builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<IStatusMonitor>()
                .AsImplementedInterfaces()
                .InstancePerRequest();
```

#### Logging
Logging has been configured to use [Serilog](https://serilog.net/) with a SQL Server sink. It is registered as a singleton. 

This is a structure logging library which allows additional
functionality when searching for events. For more information see the website. 

##### Usage
To utilize logging, add it as a constructor dependency and the container will resolve it.
````
public StatusV2Controller(IStatusCollector statusCollector, ILogger log)
{
  _statusCollector = statusCollector.ThrowIfNull(nameof(statusCollector));
  _log = log.ThrowIfNull(nameof(log));
}
````

````
[Route("status/{id:int}")]
public IHttpActionResult Get(MonitorType id)
{
  _log.Information("Request for MonitorType -> {id}", id);
  return Ok(_statusCollector.Collect(id));
}
````

#### API Versioning
API versioning is handled through URL versioning. Each version of the an api will have a new controller source file and a unique 
url that contains the version. The routing for these versions is handled via attributes. The steps for creating a new version of an API
are roughly as follows:

1. If a subfolder does not exist for a version, create it 
   - \v1, \v2, \v3...
2. Add a route prefix to the static RoutePrefixes class
3. Create the a new controller
4. Add the RoutePrefix attribute at the class level
5. Add the Route attribute to each operation, specifying the route template

#### Server Stack
- Autofac
- EntityFramework
- Asp.Net Identity
- Asp.Net MVC
- Asp.Net Web Api 2
- Owin Middleware for Bearer Security
- Serilog  


### Client Development Notes
There is a simple client UI within ***Launchpad.Web***. It supports the following scenarios:

- As a user, I want to be able to register so I can access secure data.
- As a user, I want to login so I can see secure data.
- As a user, I want to see a list of widgets.
- As a user, I want to logout so that other parties cannot access secure data.

#### Client Build Commands
Currently, the client is built using Grunt tasks. These tasks have been wrapped in NPM scripts to support removing the Grunt dependency.

- npm test: This will run JSLint and the Karma test
- npm run build: This will run JSLint and build the output files to /dist/ (at this time it does not minify)

#### Client Stack 
The following components are used in the client:

1. [Bootstrap](http://getbootstrap.com/)
2. [Angular (1.5+)](https://angularjs.org/)
   - New functionality should be build using components
3. [Angular UI Router (1.0.0-beta.3)](https://ui-router.github.io/ng1/)
   - Note: The beta library is needed to support component routing
4. [Karma](https://karma-runner.github.io/1.0/index.html)
5. [Sinon](http://sinonjs.org/) 
   - Mocking library for JavaScript

### Generating Documentation
The web api controllers are using inline documentation called apiDoc. The documentation is embedded as comments above each controller method. For 
more details see the [documentation](http://apidocjs.com/).

#### Sample Documentation Comments
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

#### Reusable apiDoc blocks
apiDoc supports creating reusuable documentation blocks using [@apiDefine](http://apidocjs.com/#param-api-define). This 
cuts down on repeated comment blocks for shared elements such as errors. 
All reusable blocks should be placed in  ***Launchpad.Web\Controllers\_apidoc.js***


##### Current @apiDefine blocks

1. BadRequestError
   - Used when an endpoint can return a 400
2.  NotFoundError
    - Used when an endpoint can return a 404

#### Generating documentation
To generate the api docs after a change:
1. In ***Launchpad.Web*** execute npm run doc
   - This is an npm script that is defined in package.json
   - Script: apidoc -o docs -i Controllers
   - This will scan the Controllers folder for endpoints and place the output in \docs

To view the documentation either run the application and navigate to /docs/ or open the static index.html file.

## Testing
The project has both client and server tests. 

### Client Testing
1. The tests use behavior and state verification
   - Use Sinon to setup expectations and verify the behavior
   - Use a Sinon sandbox for easy teardown of mocked components
2. Place the test file alongside the components under test
   - The build process will ignore any file with **.spec.js in the name
3. Use $componentController service to get instance for testing component controllers in isolation
4. Use nested describes to group tests together 
5. For continuous testing use: karma start --singleRun=false


#### Sinon Sandbox Setup
```
  beforeEach(function(){
        sandbox = sinon.sandbox.create();
    });
 
  afterEach(function(){
        sandbox.restore();
    });
  
```

### Server Testing 
1. Integration tests exist for the Launchpad.Data
   - The primary goal of these tests is to excercise the EntityFramework configuration and repositories
2. There should be a mirrored structure between project files and test files
   - This allows the developer to locate the test file quickly based on the location of the project file
3. The tests use behavior and state verification
   - Use MOQ to setup expectations and verify the behavior
   - There is a BaseUnitTest class which provides a MockRepository and AutFixture.Fixture member to the 
     derived class
4. Use interfaces for dependencies in order to loosely couple components and make code mock-able
5. When possible avoid statics to promote testable components



    


### Testing Stack
The current testing stack for client and server are found below. 

#### Client Tests
1. [Karma](https://karma-runner.github.io/1.0/index.html)
2. [Jasmine](http://jasmine.github.io/) 
3. [Sinon](http://sinonjs.org/) 

#### Server Tests
1. [Xunit](https://xunit.github.io/)
2. [AutoFixture](https://github.com/AutoFixture/AutoFixture)
3. [FluentAssertions](http://www.fluentassertions.com/)
4. [MOQ](https://github.com/moq/moq4)
