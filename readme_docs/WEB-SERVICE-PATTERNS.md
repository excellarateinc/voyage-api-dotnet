## Web Service Patterns
The following the patterns defined below describes the expectations that should be available in every web service (WS) as a rule. All developers utilizing this WS API should be able to rely on the consistency of implementation. All developers implementing a web service in this API should know these patterns an adhere to them strictly. Should a situation appear that is outside of these patterns and rules, then it should be reviewed by the team and updated within this document with the clearly communicated understanding that everyone is to follow the revised patterns and rules. 


## Table of Contents
* [References](#references)
* [Definitions](#definitions)
* [RESTful URLs](#restful-urls)
* [Request Methods](#request-methods)
* [Versioning](#versioning)
* [Filtering, Sorting, Searching, Selecting Fields](#filtering-sorting-searching-selecting-fields) 
* [Response Status Codes](#response-status-codes)
* [Response Body](#response-body)
* [Examples](#examples)


## References
The patterns and rules defined within this document are heavily influenced by the following people and frameworks.

* [Best Practices For a Programatic RESTful API](http://www.vinaysahni.com/best-practices-for-a-pragmatic-restful-api)
* [OData](http://www.odata.org) specification as a guideline (not an absolute rule)


## Definitions

#### Web Service Provider (aka Provider)
The provider of web services to an audience of web service consumers. A provider provides web services for consumption.

#### Web Service Consumer (aka Consumer)
The consumer of web services made available by a web service provider. A consumers consumes web services to push and pull data. 


## RESTful URLs
There are [many articles](https://www.google.com/#q=restful+url+design) and [REST specifications](http://www.ics.uci.edu/~fielding/pubs/dissertation/rest_arch_style.htm) online that can educate on the basics. This API follows a few key points:

1. Structure the URL from the **consumer's** perspective and be consistent!
2. Endpoint names are plural nouns 
  * /users - returns a list of users
  * /tickets - returns a list of tickets
3. Get single objects by adding the object "id" after the plural noun endpoint name
  * /users/1 - retrieves user with id=1
  * /orders/2 - retrieves order with id=2
4. Use [HTTP Request Methods](#http-request-methods) to do different actions with a single plural noun endpoint
  * /users/1 POST - create a new user
  * /users/1 GET - retrieve user id=1
  * /users/1 PUT - update user id=1
  * /users/1 DELETE - delete user id=1
5. Relational requests extend the parent endpoint
  * /users/1/roles - returns the roles associated with user id=1
  * /users/1/roles/2 - returns role with id=2 for user with id=1
6. Use "verb" endpoint names only when it makes better sense from the consumer's perspective
  * /search - invokes a global search perhaps
  * /users/1/star - adds a star to the user's profile
  * NOTE: Optionally consider to embed the "action" within a plural noun resource like "/users/1" PATCH with body parameter "isStarred: true". In some cases this might lead to a more intuitive API with fewer "action" endpoints to be concerned about. 

Reference APIs: [Github API](https://developer.github.com/v3/), [Enchant API](http://dev.enchant.com/api/v1)


## Request Methods

#### GET
Requests one or more objects from the database. Optionally includes a request parameter on the URL string for the "id" of the 
specific object to retrieve. For example, "/users" to retrieve a list of users and "/users/1" to retrieve user with id=1.

#### POST
Inserts a new object into the database. For example, "/users" to post a new user object to the database.

#### PUT
Updates the entire object into the database. A PUT assumes that the request will contain all necessary fields to completely 
replace the object within the database. For example, "/users/1" to update an existing user within the database.

#### PATCH
Partially updates an object within the database. The purpose of PATCH is to send only the data that should be changed about an object while keeping everything else unchanged. As of this writing, PATCH will not be used very often, if at all. 

#### DELETE
Requests the deletion of a specific object. For example, "/users/1" to delete user with id=1 from the database.


## Versioning
Versioning the API ensures backward compatability with consumers and allows the developers of the API to release new versions of a web services API without impacting existing consumers (unless they choose to be impacted). Where to place the version number value within the request is an (ongoing debate)[http://stackoverflow.com/questions/389169/best-practices-for-api-versioning]. The approach taken by this API follows [Vinay Sahni](http://www.vinaysahni.com/best-practices-for-a-pragmatic-restful-api#versioning), which is to include the MAJOR version number of the entire API in the URI and include a sub-version value of the API within a custom HTTP header attribute titled "API-Version".

```
curl https://api.[your-hostname].com/v1/users -H "API-Version: 2016-11-06"
```

### Incrementing Versions
When one or more changes are made to any of the web services within the API, the major or minor version of the API should be incremented. 

> NOTE: Versioning is never done at the individual endpoint level as that would create an unmanagable tangle of version numbers that would be difficult for a consumer to follow. For example, /v1/users/1/v3/roles/1/v9/claims. This tells a story of API version 1 with /users version 1 and /roles version 3 that is calling into /claims v9. 

### Major Versions
A major version is located within the URL with no sub-version number. Incrementing a major version number is a subjective process to be defined by the development and/or marketing team. In general, increment the major version number by 1 when the API schema has significant changes that will require consumers to invest significant time to refactor their applications to conform to the new web services schema. 

API Version 1 - Initial version with the most current minor version.
```
curl https://api.[your-hostname].com/v1/users
```

API Version 2 - Latest API with the most current minor version.
```
curl https://api.[your-hostname].com/v2/users
```

### Minor Versions
A minor version is located within the HTTP request header in an attribute titled "API-Version". The minor version is an optional value that will default to the latest minor version of the API.

The minor version value strategy of this API is to use a date in the format of "YYYY-MM-DD". The date chosen should be any date after the last minor version date. 

The Major Version number combined with the Minor Version number defines a specific API schema at a point in time. 

API Version 1 - Initial version with specific minor version "2016-11-6"
```
curl https://api.[your-hostname].com/v1/users -H "API-Version: 2016-11-06"
```

API Version 2 - Latest API with specific minor version "2016-11-13"
```
curl https://api.[your-hostname].com/v2/users -H "API-Version: 2016-11-13"
```

## Filtering, Sorting, Searching, Selecting Fields
The following common actions use query parameters to add provide the web service with options for processing the request and how to return the data in the response.  
```
/v1/users?sort=-name
```
> NOTE: Query parameters should not be embedded into the RESTful URL. DON'T do "/v1/users/sort/-name"!

> INFO: Be sure to document all query parameters so that the consumer has a clear definition of how to use the web service API

#### Filters
Add customer query parameters for each filter the consumer is able to manipulate. For example, to filter a list of users to only those that are active and have roles:
```
/v1/users?status=active&hasRoles=true
```

#### Sorting
Sorting a list of results from a resource call should follow a standard protocol:

```
/v1/users?sort=lastName,firstName,-active
```

* "sort" query parameter holds the sorting details
* Include one or more object property names separated by commas like "?sort=lastName,firstName,-active"
  * The sort order will be performed in the order that the names are specified. 
* Specify "descending sort" for a object property by prefixing the object property with a minus sign (ie "-lastName")
* Specify "ascending sort" for an object property by having no prefix to the object property name.

#### Searching
Searching within a resource should follow the standard protocol:

```
/v1/users?q=hello
```

* "q" query parameter holds the search details
* Include an appropriate value supported by the web service for searching like "hello" to full-text search for the word "hello"

#### Selecting Fields
Web services working with large data sets, large objects, or object associations may wish to allow the consumer to control the data returned in the response. The following protocol should be followed in these situations.

Returns the default set of data for a list of users:
```
/v1/users
```

Returns the default set of data for a list of users as well as the "roles" associated object for each user record:
```
/v1/users?select=default,roles
```

Returns only the "firstName", "lastName", "email", and "phones" data within the list of users:
```
/v1/users?select=firstName,lastName,email,phones
```

> NOTE: These "select" options are unique to each web service and must be documented clearly to the consumer. 


## Response Status Codes
The following codes are returned as HTTP Status Codes in the response header depending on the circumstances described below. All responses regardless of the status code, should contain a well formed JSON response object that contains meaningful information for the consumer.

#### 200 OK
Returned for an expected and acceptable web service request from a consumer. 

#### 201 Created
Returned after a POST request successfully creates a new resource. The location (URL) of the newly created resource should be defined within the "Location" HTTP Header attribute within the response. See [Location Header](https://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.30) in the HTTP spec. 

#### 400 Bad Request
Returned when the request parameters are incorrect, invalid, or incomplete. This response status code should be use to enforce the schema of the request. 

#### 401 Unauthorized
Returned when a consumer makes a request to a web service that is secured and the consumer is not authorized to make the request. 

#### 402 Request Failed
Returned when an expected failure situation occurrs, such as a parameter has an incorrect format or value. Other reasons for returning this status code might be a failure to properly complete a transaction. In every case, the response should include a detailed explaination to the consumer as to the error code, description, and any other information they should have to retry. 

#### 403 Forbidden
Returned when a consumer is authorized (i.e. logged in) and makes a request to a secured web service but the consumer doesn't have the proper authorization to make the request. 

#### 404 Not Found
Returned when a request is made for specific data, usually using an ID, and the data cannot be found. This response code is also returned automatically by the web server if a endpoint or URL is not defined by the application. 

#### 500 Internal Server Error
Returned when an unexpected error occurs within the app for any reason. This is usually accompanied by an application stack trace.


## Response Body
Every HTTP Response should contain a well defined HEADER and BODY so that the consumer has a well formed and _meaningful_ response message. Consumers of web services can be frustrated during development or while debugging a production issue if the web service response is not informative enough as to the reasons for success, partial success, or failure of a request. 

### For POST
1. 201 "Created" HTTP Response Status Code should be returned IF the request is successfully processed. See [HTTP Response Status Codes](#http-response-status-code) for other behaviors.
2. HTTP header attribute "Location" should contain the URL to the newly created resource (http://api.[your-hostname].com/v1/users/35
3. Response body should return the full record in JSON format (so that the consumer doesn't have to immediately refetch)
4. Response body should contain a "status" and/or "errors" object(s) with additional metadata about the processing of the request. 

### For PUT, PATCH
1. 200 "Ok" HTTP Response Status Code should be returned IF the request is successfully processed. See [HTTP Response Status Codes](#http-response-status-code) for other behaviors.
2. Response body should return the full record in JSON format (so that the consumer doesn't have to immediately refetch)
3. Response body should contain a [Response Body Status](#response-body-status) and/or [Response Body Errors](#response-body-errors) object(s) with additional metadata about the processing of the request. 

### Response Body Status
Web service responses might need to contain a "status" that notifies the consumer that the action completed, partially completed, or some other status that instructs the consumer on next steps. 

The following example is a situation with a POST request to /users which resulted in a successful saving to the database
```
{
    id: 1,
    firstName: "John",
    lastName: "Doe",
    email: "blah",
    status: {
        code: "success", 
        message: "User was saved successfully!"
    }
}
```

The 'status' object has the following structure:
* code (enum/text, required): 'success', 'partial', 'error'
* message (text, required): any text describing the status

### Response Body Errors
When an anticipated error occurs within the app, like a required field or format error, it is necessary to communicate back to the consumer the errors that occured in a standard format. All web services should follow the same pattern so that the consumers can reliably expect to find the error information in a consistent location and structure.  

The following example is a situation with a PUT request to /users/1 resulted in rejected request due to invalid data

```
{
    id: 1,
    firstName: "",
    lastName: "Doe",
    email: "blah",
    status: {
        code: "error", 
        message: "Errors occurred while updating the user record"
    },
    errors: [
       {type: "format", message: "email is not formatted correctly. ex: text@text.ext"}
       {type: "required", message: "firstName is a required field"}
    ]
}
```

The 'errors' list contains 'error' objects with the following structure:
* type (enum/text, required): 'required', 'format', 'unknown'
* message (text, required): any human readable text describing the error
* stackTrace (text, optional): stack trace from an exception thrown by the application.

## Examples

#### /v1/users - GET
Returns a list of users that the consumer is eligible to receive.

#### /v1/users?select=Roles - GET
Follows the same process as /users - GET, but additionally returns the associated Roles of the User (this follows OData spec). Consult the web services API Doc documentation for options on the additional information that the web service supports in the response. 

#### /v1/users/1 - GET
Returns a specific User with "id" of 1. Returns a 404 response code if no user is found. 

#### /v1/users - POST
Creates a new user record. Returns a 200 OK if the user is successfully created or if there are validation or other expected errors. See the Status object with an optional Errors object in the response for more information. See [Response Status](#response-status) and [Response Errors](#response-errors) for more information.  

#### /v1/users/1 - PUT
Updates an existing user record by overwriting the object in the database with the given request payload. If data is omitted from the request payload, then it will be deleted in the database. PUT represents a complete overwrite, whereas PATCH represents a partial update.

#### /v1/users/1 - DELETE
Deletes a specific User with "id" of 1

#### /v1/users/1/roles - GET
Returns a list of roles that the consumer is eligible to receive. This is an example of nesting associated objects of the User in a RESTful pattern. 

#### /v1/users/1/roles/2 - GET
Returns a specific User Role from User id=1 and Role id = 2.

#### /v1/users/1/roles - POST
Creates a new role for User with "id" = 1. Returns a 200 OK if the role is successfully created or if there are validation or other expected errors. See the Status object with an optional Errors object in the response for more information. See [Response Status](#response-status) and [Response Errors](#response-errors) for more information.

#### /v1/users/1/roles/2 - PUT
Updates an existing role record (id=2) for existing user record (id=1) by overwriting the object in the database with the given request payload. If data is omitted from the request payload, then it will be deleted in the database. PUT represents a complete overwrite, whereas PATCH represents a partial update.

#### /v1/users/1/roles/2 - DELETE
Deletes a specific user role with id="2" for user with id = "1".
