## Overview
The following web service patterns are implemented within Launchpad API and should be followed closely.

## Table of Contents
* [HTTP Methods](#http-methods)
* [Example Endpoints](#example-endpoints)
* [HTTP Codes](#http-codes)
* [Response Status](#response-status)
* [Response Errors](#response-errors)

## HTTP Methods

#### GET
Requests one or more objects from the database. Optionally includes a request parameter on the URL string for the "id" of the 
specific object to retrieve. For example, "/users" to retrieve a list of users and "/users/1" to retrieve user with id=1.

#### POST
Inserts a new object into the database. For example, "/users" to post a new user object to the database.

#### PUT
Updates the entire object into the database. A PUT assumes that the request will contain all necessary fields to completely 
replace the object within the database. For example, "/users/1" to update an existing user within the database.

#### DELETE
Requests the deletion of a specific object. For example, "/users/1" to delete user with id=1 from the database.

## Example Endpoints
/users - GET - List users

/users/1 - GET - Get user 1

/users - POST - Create new user

/users/1 - PUT - Update user 1

/users/1 - DELETE - Delete user 1


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
* code (enum/text, required): 'success', 'partial'
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
