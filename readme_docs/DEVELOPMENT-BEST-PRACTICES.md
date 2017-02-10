## Development Best Practices
Best practices that all developers on the team agree to adhere to when writing new code or refactoring existing code. Peer code reviews should enforce these best practices along with the documented [coding standards](DEVELOPMENT.md#coding-standards). 

## Table of Contents
* [Dependency Injection](#dependency-injection)
* [Logging](#logging)

## Dependency Injection
The application is using Autofac as the DI container. Each project contains a module file that is responsible for registering the contained
components with the container. The overall container is setup in the ContainerConfig in ***Voyage.Web***.

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

:arrow_up: [Back to Top](#table-of-contents)

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

### Creating Good Log Messages
Logging is only as useful as the content of the messages. With structured logging, searching and reading log messages can be made easy. Use the following guidelines to create good error messages:

#### Event Codes
Prepend a human readable event code to classify messages. For example, anytime an action fails because of a missing claim log a message with the event code of 'Authorization.' The event code will be visible in not only the message but as well as the JSON properties. This will allow for easy filtering by feature or logical group.

```
Logger.Information("({eventCode:l}) {user} does not have claim {claimType}.{claimValue}", EventCodes.Authorization, identity.Name, ClaimType, ClaimValue);
```

#### Logging Context
Utilize the ForContext method to provide additional information about the class that generated the message. This helps identify the component that raised the error and especially useful if different components can log similar messages. 

```
Log.Logger
   .ForContext<ClaimAuthorizeAttribute>()
   .Information("({eventCode:l}) {user} does not have claim {claimType}.{claimValue}", EventCodes.Authorization, identity.Name, ClaimType, ClaimValue);
```

#### Structuring Messages
Take advantage of Serilog by using the message template to provide both a human readable message and a structured JSON object containing key message properties. The message templates can be formed using {} as placeholders. The placeholder name will become the name of the property on the JSON object. Placeholder values are specified as arguments after the template - these arguments are applied to the placeholders in the order which they are specified. 

For example, given the following message template:

```
 _logger.ForContext<VoyageDataContext>()
        .Error(validationException, "({eventCode:l}) {validationErrors}", EventCodes.EntityValidation, string.Join(";", errorMessages));
```

The human readable message will be:

```
(EntityValidation) "'User' has error 'User name delete@admin.com is already taken.'"
```

And the JSON object will be:

```
{
  "Timestamp":"2016-12-08T13:59:25.7642779-06:00",
  "Level":"Error",
  "MessageTemplate":"({eventCode:l}) {validationErrors}",
  "Exception":"<Omitted for Clarify>",
  "Properties":{
      "eventCode":"EntityValidation",
      "validationErrors":"'User' has error 'User name delete@admin.com is already taken.'",
      "SourceContext":"Voyage.Data.VoyageDataContext"},
      "Renderings":{"eventCode":[{"Format":"l","Rendering":"EntityValidation"}]
    }
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

:arrow_up: [Back to Top](#table-of-contents)
