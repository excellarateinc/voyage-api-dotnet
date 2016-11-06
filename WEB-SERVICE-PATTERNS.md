## Overview
The following the patterns defined below describes the expectations that should be available in every web service (WS) as a rule. All developers utilizing this WS API should be able to rely on the consistency of implementation. All developers implementing a web service in this API should know these patterns an adhere to them strictly. Should a situation appear that is outside of these patterns and rules, then it should be reviewed by the team and updated within this document with the clearly communicated understanding that everyone is to follow the revised patterns and rules. 

## Table of Contents
* [References](#references)
* [Definitions](#definitions)
* [HTTP Methods](#http-methods)
* [Example Endpoints](#example-endpoints)
* [HTTP Codes](#http-codes)
* [Response Status](#response-status)
* [Response Errors](#response-errors)

## References
The patterns and rules defined within this document are heavily influenced by the following people and frameworks.

* [Best Practices For a Programatic RESTful API](http://www.vinaysahni.com/best-practices-for-a-pragmatic-restful-api)
* [OData](http://www.odata.org) specification as a guideline (not an absolute rule)

## Definitions

#### Web Service Provider (aka Provider)
The provider of web services to an audience of web service consumers. A provider provides web services for consumption.

#### Web Service Consumer (aka Consumer)
The consumer of web services made available by a web service provider. A consumers consumes web services to push and pull data. 

## HTTP Methods

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

## Example Endpoints

#### /users - GET
Returns a list of users that the consumer is eligible to receive.

#### /users?select=Roles - GET
Follows the same process as /users - GET, but additionally returns the associated Roles of the User (this follows OData spec). Consult the web services API Doc documentation for options on the additional information that the web service supports in the response. 

#### /users/1 - GET
Returns a specific User with "id" of 1. Returns a 404 response code if no user is found. 

#### /users - POST
Creates a new user record. Returns a 200 OK if the user is successfully created or if there are validation or other expected errors. See the Status object with an optional Errors object in the response for more information. See [Response Status](#response-status) and [Response Errors](#response-errors) for more information.  

#### /users/1 - PUT
Updates an existing user record by overwriting the object in the database with the given request payload. If data is omitted from the request payload, then it will be deleted in the database. PUT represents a complete overwrite, whereas PATCH represents a partial update.

#### /users/1 - DELETE
Deletes a specific User with "id" of 1

#### /users/1/roles - GET
Returns a list of roles that the consumer is eligible to receive. This is an example of nesting associated objects of the User in a RESTful pattern. 

#### /users/1/roles/2 - GET
Returns a specific User Role from User id=1 and Role id = 2.

#### /users/1/roles - POST
Creates a new role for User with "id" = 1. Returns a 200 OK if the role is successfully created or if there are validation or other expected errors. See the Status object with an optional Errors object in the response for more information. See [Response Status](#response-status) and [Response Errors](#response-errors) for more information.

#### /users/1/roles/2 - PUT
Updates an existing role record (id=2) for existing user record (id=1) by overwriting the object in the database with the given request payload. If data is omitted from the request payload, then it will be deleted in the database. PUT represents a complete overwrite, whereas PATCH represents a partial update.

#### /users/1/roles/2 - DELETE
Deletes a specific user role with id="2" for user with id = "1".

## HTTP Codes
The following codes are returned as HTTP Status Codes in the response header depending on the circumstances described below.

#### 200 OK
Returned for an expected and acceptable web service request from a consumer. 

#### 401 Unauthorized
Returned when a consumer makes a request to a web service that is secured and the consumer is not authorized to make the request.

#### 403 Forbidden
Returned when a consumer is authorized (i.e. logged in) and makes a request to a secured web service but the consumer doesn't have the proper authorization to make the request. 

#### 404 Not Found
Returned when a request is made for specific data, usually using an ID, and the data cannot be found. This response code is also returned automatically by the web server if a endpoint or URL is not defined by the application. 

#### 500 Internal Server Error
Returned when an unexpected error occurs within the app for any reason. This is usually accompanied by an application stack trace.

## Response Status
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

## Response Errors
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
* message (text, required): any text describing the error
* stackTrace (text, optional): stack trace from an exception thrown by the application.
