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
