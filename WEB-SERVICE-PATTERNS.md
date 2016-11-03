## Overview
The following web service patterns are implemented within Launchpad API and should be followed closely.

## Table of Contents
* [HTTP Methods](#http-methods)
* [Example Endpoints](#example-endpoints)
* [HTTP Codes](#http-codes)
* [Errors](#errors)

**NOTE:** 
If a new pattern emerges during development, then this page must be updated to reflect the OLD pattern and the NEW pattern 
along with the date and name of the person making the change.

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


## Errors
What pattern do we follow if there are "faults" or errors that need to be communicated back to the caller. 

```
errors [
   {type: "format", message: "email is not formatted correctly. ex: text@text.ext"}
   {type: "required", message: "firstName is a required field"}
]
```
