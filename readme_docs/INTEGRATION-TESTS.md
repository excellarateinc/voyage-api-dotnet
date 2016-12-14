## Approach
Integration testing helps ensure all components work together at runtime. The main testing goals of the application 
integration test suite are:

1. To exercise the database project (albeit from a empty state)
2. To exercise the full HTTP request/response lifecycle
3. To isolate test data from other environments
4. To exercise EntityFramework against a SQL database

### Hosting
Since the application is built using Owin, the API can be self-hosted without IIS. This allows us to execute integration tests that 
exercise the full HTTP stack. 

Using Microsoft.Owin.Hosting.WebApp will take in a bootstrapping class and start a webserver. In the case of the test fixture, the services will be hosted on http://localhost:9000.

```
var webAppInstance = WebApp.Start<Startup>(url: BaseAddress);
```

### Database Interaction
 There are several approaches to handling database interaction with the application technology stack:
 
1. Mocking: The datacontext can be mocked. However, this will not exercise EF configuration and Linq to Objects does not work 
the same as Linq to Entities. For more information, see the [documentation](https://msdn.microsoft.com/en-us/library/dn314429(v=vs.113).aspx).
2. Test Doubles: The datacontext can be replaced with a test double. This approach has the same issues as #1. 
For more information, see the [documentation](https://msdn.microsoft.com/en-us/library/dn314431(v=vs.113).aspx).
3. Use a database: While there are some in-memory implementations such as Effort these do not seem maintained and still have similar 
limitations to the aformentioned options. In the future, .Net Core EF will support an in memory database to help with testing. 
Outside of an in-memory database there is using a real database instance. 

In order to meet the goals of the integration testing approach, the tests will use a SQL Server database. A lightweight engine is available with Visual Studio called LocalDb. This will allow the API to not only excercise the EntityFramework configuration but also provide a true SQL Database interaction.

### Creating Tests

#### OwinFixture
This is a shared test fixture which ensures tests do not run concurrently as this could cause issues with spinning up and tearing down the 
hosts. 

#### Requests and Responses
HTTP communication is handled with the standard HttpClient. 

*Configuring the request*

Requests are configured using the HttpResponseRequest class. This class will be used by the HttpClient to send the request to the API. The test can use the OwinFixture extension method GetEndPoint to form the full URL of the hosted services.

```
public static string GetEndpoint(this OwinFixture fixture, string path)
{
    return $"{fixture.BaseAddress}{path}";
}
```

Usage

```
OwinFixture.GetEndpoint("/api/v1/statuses")
```

*Authentication*

When issuing requests to secure endpoints, it is necessary to add the Authorization token to the request. Simply set the header on the request:

```
 message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", fixture.DefaultToken);
```

There is a helper extension method that will create a secure request with the default token.

```
public static HttpRequestMessage CreateSecureRequest(this OwinFixture fixture, HttpMethod method, string path)
{
   var message = new HttpRequestMessage(method, fixture.GetEndpoint(path)); 
   message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", fixture.DefaultToken);
   return message;
}
```

*Sending the Request*

Requests can be sent with the default client on the fixture using SendAsync. This method is awaitable and will return the HttpResponseMessage.

```
var response = await OwinFixture.Client.SendAsync(httpRequestMessage);
```

*Reading the Response*

Custom assertions can be used to help verify the response. Additionally, there response body is available for inspection. There is an extension method that will deserialize a JSON payload to a concrete C# type. 

```
public static async Task<TType> ReadBody<TType>(this HttpResponseMessage message)
{
    var rawContent = await message.Content.ReadAsStringAsync();
    TType models = JsonConvert.DeserializeObject<TType>(rawContent);
    return models;
}
```

#### Custom Assertions
Custom assertions have been added to help create better BDD-style tests. The current custom assertions include:

*HttpResponseMessageAssertions*

|Method|Description|
|:----|:----|
|HaveHeader|Asserts the response has the expected header|
|HaveStatusCode|Asserts the response has the expected status code.|


### LocalDb
For local development, everything that is needed to create the LocalDb instance is installed with Visual Studio. There is a .bat file called configure-test-db.bat which automates the deployment of the integration test database. At a high level, the .bat file performs the following steps using out of box utilities:

1. Using SqlLocalDb.exe stop the instance
2. Using SqlLocalDb.exe delete the instance
3. Delete the log and data files
4. Using SqlLocalDb.exe create and start the instance
5. Using SqlPackage.exe deploy the .dacpac from the Launchpad.Database project

Since the .dacpac contains all the schema and seed data to run the application, this process gives a new instance of the database with all required data.

#### Seed Data
Currently, all seed data is contained as post-deployment scripts in the .dacpac. In the future, if additional seed data is needed for tests there are a few options:

1. Modify the database project to conditional execute post-deployment scripts
2. Create .sql scripts and modify the configure-test-db.bat to execute them
3. Within the Owin fixture programmatically seed the data


### Limitations
The above approach has the following limitations:

1. Owin selfhosting is used for the API - live deployments will use IIS. (One of the main points of Owin is to separate the application from the host - this is a feature)
2. Additional dependencies on localdb and management utilities requires more dependencies for local and CI environments
3. Less flexible than in-memory implementations
