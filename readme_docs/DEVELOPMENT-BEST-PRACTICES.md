## Development Best Practices
Best practices that all developers on the team agree to adhere to when writing new code or refactoring existing code. Peer code reviews should enforce these best practices along with the documented [coding standards](DEVELOPMENT.md#coding-standards). 

## Table of Contents
* [Dependency Injection](#dependency-injection)
* [Logging](#logging)

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

## HTTP Request Validation
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
