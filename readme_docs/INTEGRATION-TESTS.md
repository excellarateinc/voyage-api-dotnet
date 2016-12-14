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

#### Initializing LocalDb
There are two options for initializing the LocalDb instance for the developer environment:

*Post Build Event*

Use the post build event on Launchpad.Web.IntegrationTests to automatically call the configure-test-db.bat Note: This will cause the build of the project be slower since it will need to deploy the .dacpac
```
CALL "$(SolutionDir)\configure-test-db.bat" "$(SolutionDir)\Launchpad.Database\bin\$(ConfigurationName)\Launchpad.Database.dacpac"
```
*Manually Invoke .bat*

Manually call the configure-test.db.bat Note: This will not recreate the database on every run - be wary of depending on test data that was created in previous runs. Best practice: Always recreate the database and run the tests before committing.

```
configure-test-db.bat
LocalDB instance "Integration-Test-Instance" stopped.
LocalDB instance "Integration-Test-Instance" deleted.
<Remainder omitted for brevity>
```

#### HostFixture

This test fixture is shared across all tests. Before any test runs, it will bootstrap the API services. This allows tests to interact with the services through the HTTP stack. Dispose will automatically be called at the end of the test run.

In order for the test fixture to be injected into the test, it must be decorated with the following attribute:

```
[Trait("Category", "Self-Hosted")]
```

Additionally, there must be a constructor argument of type HostFixture.
 
#### ApiConsumer
 
 This abstract class represents a class which will interact with the services. It has a number of convenience methods for creating requests:

|Member|Description|
|:----|:----|
|GetUrl| Formats the given path to an absolute URL.  | 
|CreateSecureRequests | Initializes a HttpRequestMessage to the given URL and sets the Authorization header to the default token |
|CreateUnauthorizedRequests|Initializes a HttpRequestMessage to the given URL and sets the Authorization header to an invalid token|
|Client| A default HttpClient to use for communication|

#### ApiTest
This abstract class repesents an integration test. The core concept is that any given test file will test a single API endpoint. These system under test is reprsented by the abstract properties Method and PathUnderTest. Implementors will define these values for the endpoint that is under test. For example, a testing the retrieval of all useres would set the values to HttpMethod.Get and "/api/v1/users/" respectively.

#### DataHelper
This is an abstract class that encapsulates getting test data for an entity. This is useful for tests such as getting user by id. The concrete implmentation for users, UserHelper, allows tests to share the code that determines an existing UserId.

|Method|Description|
|:----|:----|
|Refresh|Issues the HttpRequest which loads the entities that will be used by subsequent methods to get test data|
|GetSingleEntity|Retrieves an arbitrary existing entity|
|GetAllEntities|Retrieves all entities that were loaded by Refresh|

Additionally, concrete implementations of this class can add their own methods to handle common test data functions such as creating a new record which is useful in the setup phase of a delete test.

**Sample Implementation**
```
    public class UserHelper : DataHelper<UserModel>
    {
        private List<UserModel> _entities = Enumerable.Empty<UserModel>().ToList();

        public override IEnumerable<UserModel> GetAllEntities()
        {
            return _entities;
        }

        public override UserModel GetSingleEntity()
        {
            return _entities.First();
        }

        public async override Task Refresh()
        {
            var httpRequestMessage = CreateSecureRequest(HttpMethod.Get, "/api/v1/users");
            var response = await Client.SendAsync(httpRequestMessage);
            _entities = (await response.ReadBody<UserModel[]>()).ToList();
        }
    }
```

**Sample Usage**
```
        [Fact]
        public async void GetUserById_Should_Return_Status_200()
        {
            // Arrange               
            await _userHelper.Refresh();
            var user = _userHelper.GetSingleEntity();
            var httpRequestMessage = CreateSecureRequest(Method, PathUnderTest, user.Id);

            // ACT
            var response = await Client.SendAsync(httpRequestMessage);

            // ASSERT
            response.Should()
                .HaveStatusCode(HttpStatusCode.OK);

            UserModel model = await response.ReadBody<UserModel>();
            model.Should().NotBeNull();
            model.ShouldBeEquivalentTo(user);
        }
```

#### Test Files
Test files are grouped in folders by entity. Entity is defined as the primary object that the endpoint exposes. For instance, there is a folder for Users and a folder for UserRoles because there are a distinct set of endpoints that deal with the entities. 

Each HttpMethod / URL combination has its own test file. This is done to help make it easy to find the tests for a single endpoint. Additionally, this structure lends itself to easy scanning and comparison between the existing tests and the documentation. There should be a test for each type of response for a given endpoint.

#### Requests and Responses
HTTP communication is handled with the standard HttpClient which is exposed via inheritance *Test File* -> *ApiTest* -> *ApiConsumer*. 

#### Request Authentication
When issuing requests to secure endpoints, it is necessary to add the Authorization token to the request. Simply set the header on the request:

```
 message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", fixture.DefaultToken);
```

See the ApiConsumer section for a convenience method that will perform this task.

#### Response Verification
Custom assertions have been added to help create better BDD-style tests. The current custom assertions include:

*HttpResponseMessageAssertions*

|Method|Description|
|:----|:----|
|HaveHeader|Asserts the response has the expected header|
|HaveHeaderValue|Asserts the response has the expected header value|
|HaveStatusCode|Asserts the response has the expected status code.|

*BadRequestCollectionAssertions*

|Method|Description|
|ContainErrorFor|Given an enumerable of BadRequstErrorModel verifies that there is at least 1 error for the given field/code. This is useful when creating tests for 400 BAD REQUEST.|

#### Response Body
The response body is available for inspection. There is an extension method that will deserialize a JSON payload to a concrete C# type. 

```
public static async Task<TType> ReadBody<TType>(this HttpResponseMessage message)
{
    var rawContent = await message.Content.ReadAsStringAsync();
    TType models = JsonConvert.DeserializeObject<TType>(rawContent);
    return models;
}
```


#### Test Coverage
The integration tests should try to exercise the most critical portions of the application. Given that the application is a set of services consumed by clients, the publically documented API is critical. Developers can use the documentation as a guide for creating good test coverage. For example, if the documentation lists the success response as 200 and the possible error responses as 401 and 404 good test coverage would have a test for each of the responses. 

#### Step-By-Step 
The following steps are a guide to creating a new test file:

1. Create a class named with the following pattern {HttpVerb}{Entity}Tests.
2. Add the required Collection attribute
3. Inherit from ApiTest
4. Generate the constructor
5. Implement the abstract class
  1. Method should be the method under test
  2. PathUnderTest should be the endpoint location
6. Create a new [Fact]
  1. Use CreateSecureRequest to initialize a message (assuming a secure endpoint)
  2. Use Client.SendAsync to retrieve the response
  3. Verify the response using the custom assertions and by inspecting the body

**Sample**

```
    [Trait("Category", "Self-Hosted")]
    [Collection(HostCollectionFixture.Name)]
    public class GetUsersTests : ApiTest
    {
        public GetUsersTests(HostFixture hostFixture) : base(hostFixture)
        {
        }

        public override HttpMethod Method => HttpMethod.Get;

        public override string PathUnderTest => "/api/v1/users";

        [Fact]
        public async void GetUsers_Should_Return_Status_200()
        {
            // ARRANGE
            var httpRequestMessage = CreateSecureRequest(Method, PathUnderTest);

            // ACT
            var response = await Client.SendAsync(httpRequestMessage);

            // ASSERT
            response.Should()
                .HaveStatusCode(HttpStatusCode.OK);
            UserModel[] models = await response.ReadBody<UserModel[]>();
            models.Should().NotBeNullOrEmpty();
        }
    }
```


#### Unauthorized Tests
The cross-cutting concern of authorization is handled by the Owin OAuth Middleware. This crosscutting concern executes prior the request being passed to the controller. As a result, the tests for 401 can be handled via a single data driven test. The test code is shown below.

```
        [Theory]
        [MemberData("UnauthorizedUrls")]
        public async void Endpoint_Should_Respond_With_401_When_Unauthorized(HttpMethod method, string path)
        {
            var request = CreateUnauthorizedRequest(method, path);

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            response.Should().HaveStatusCode(HttpStatusCode.Unauthorized, "{0} {1} is secure", method, path);
        }
 ```

The test has two arguments - the HTTP method and the path to the API endpoint. These arguments are provided to the test using the MemberData attribute. This attribute takes the name of a static member which will provide the input of each distinct test value via an object array. In this case, the property is an array of object arrays and each item represents a set of test arguments. For each set of test arguments, the test code will be invoked.

To add an additional 401 endpoint test, simply add the arguments to the array:

```
 public static object[] UnauthorizedUrls =>
           new object[]
           {
               // Role endpoints
               new object[] { HttpMethod.Get, "/api/v1/roles" },
               new object[] { HttpMethod.Get, $"/api/v1/roles/{Guid.Empty}" },
               new object[] { HttpMethod.Delete, $"/api/v1/roles/{Guid.Empty}" },
               new object[] { HttpMethod.Post, "/api/v1/roles" },
           }
```

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
4. From a CI perspective, only a single build agent can run at a time due to the hosting of the services on port 9000.
