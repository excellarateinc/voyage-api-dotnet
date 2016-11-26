## Development
Information on how to setup your local dev environment, best practices, HOW TOs, etc and so on. Keep this document fresh with any and all information that should be shared with your fellow devs.

## Table of Contents
* [Core Values](#core-values)
* [Code Standards](#code-standards)
* [Workstation Setup](#workstation-setup)
* [Run App Locally](#run-app-locally)
* [Branch, Code Review, Commit](#branch-code-review-commit)
* [Deploy (QA, UAT, PROD)](#deploy-qa-uat-prod)

## Core Values
These are the core standards that our development team agrees to follow. All of our decisions should be tested against our core values. Only a team decision can overrule our core values on a case-by-case basis. 

1. __Always act in the best interest of our customer's time, money, and desired quality__
  * We succeed when our customer is amazed at our ability to deliver on-time, within budget, and with the highest possible quality given the time and money constraints. 
  * Test every decision with the question "Does this align with the customer's budget, timeline, and quality expectations?"
2. __Be respectful always (aka Golden Rule)__
  * Teams that respect each other with their words and actions achieve high throughput consistently
  * When we talk down to each other we are deciding that WE are more important than the customer!
3. __No Lone Ranger Developers__
  * Acting on your own excludes your team from decision making, which is a form of disrespect
  * Perfect code is a product of team collaboration and communication
4. __Peer code reviews are required__
  * For every 10 liines of code written, ~ 1 bug is introduced into the code base. 
  * Peer reviewed code has been found to significantly reduce bugs and missed requirements
  * Be thorough and respectful!
5. __Build for today's needs, not what might be needed tomorrow__
  * Make the best choices based on the customer's concrete needs today!
  * Building for the future is subjective guesswork that rarely meets unknowable customer needs

## Code Standards
The following coding standards are agreed upon by the development team and are expected to be followed. Where possible, these rules will be enforced by the IDE.
* C#
* XUnit

## Workstation Setup

## Run App Locally

## Branch, Code Review, Commit

## Deploy (QA, UAT, PROD)

## ??
* [Development Tools](#development-tools)
* [Build](#build)
* [Database](#database)
* [Dependency Injection](#dependency-injection)
* [Logging](#logging)
* [API Versioning](#api-versioning)
* [API Doc](#api-doc)

## Current Tech Stack

TODO: Each of these should be their own document (if necessary)

- Autofac
- EntityFramework
- Asp.Net Identity
- Asp.Net MVC
- Asp.Net Web Api 2
- Owin Middleware for Bearer Security
- Serilog  

## Development Tools
The following tools should be installed on the development machine:

1. Visual Studio 2015
2. SQL Server 
   - Checked in connection strings point at SQLExpress 
   - Default instance: localhost\sqlexpress
3. [Node](https://nodejs.org/en/)
4. [apiDocJs](http://apidocjs.com/)
   - npm install -g apidoc 

## Build
After pulling the source code install the required packages: 

1. Pull the source code from github repo
2. Perform a Nuget package restore
3. Perform a npm install in folder ***Launchpad.Web***

## Database
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

## Dependency Injection
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

## Logging
Logging has been configured to use [Serilog](https://serilog.net/) with a SQL Server sink. It is registered as a singleton. 

This is a structure logging library which allows additional
functionality when searching for events. For more information see the website. 

### Usage
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
```
### Debugging
Serilog will fail silently if there is an issue logging a message. While this is desirable in production, when debugging it can be hard identify the issue with the message template. Serilog offers debugging out to help troubleshoot issues. To turn on the output, use:

```
 Serilog.Debugging.SelfLog.Enable(msg => System.Diagnostics.Debug.WriteLine(msg));
```
 
 
This will write Serilog issues to the output window. For more options see the [documentation](https://github.com/serilog/serilog/wiki/Debugging-and-Diagnostics).

```
016-11-16T16:46:12.8645916Z Exception while emitting periodic batch from Serilog.Sinks.MSSqlServer.MSSqlServerSink: System.AggregateException: One or more errors occurred. ---> System.FormatException: Format String can be only "D", "d", "N", "n", "P", "p", "B", "b", "X" or "x".
   at System.Guid.ToString(String format, IFormatProvider provider)
   at Serilog.Events.ScalarValue.Render(TextWriter output, String format, IFormatProvider formatProvider)
   at Serilog.Parsing.PropertyToken.Render(IReadOnlyDictionary`2 properties, TextWriter output, IFormatProvider formatProvider)
   at Serilog.Sinks.MSSqlServer.MSSqlServerSink.FillDataTable(IEnumerable`1 events)
   at Serilog.Sinks.MSSqlServer.MSSqlServerSink.<EmitBatchAsync>d__10.MoveNext()
```

## API Versioning
API versioning is handled through URL versioning. See the [Web Service Pattern for API Versioning](WEB-SERVICE-PATTERNS.md#versioning). 

Each version of the an api will have a new controller source file and a unique url that contains the version. The routing for these versions is handled via attributes. The steps for creating a new version of an API are roughly as follows:

1. If a subfolder does not exist for a version, create it 
   - \v1, \v2, \v3...
2. Add a route prefix to the static RoutePrefixes class
3. Create the a new controller
4. Add the RoutePrefix attribute at the class level
5. Add the Route attribute to each operation, specifying the route template

## API Doc
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
