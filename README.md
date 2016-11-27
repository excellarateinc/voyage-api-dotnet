## Overview
A foundational set of web services that implement industry standard guidelines, common best practices, and the experienced insights afforded to Lighthouse Software thru decades of enterprise business software development. 

If you are extending this API to build a new app, then replace this section with a detailed overview of the new app. Include as much or as little detail as necessary to convey to the developers what this project is about. Often times less is more. 

## Topics
* [Quick Start](readme_docs/DEVELOP.md#quick-start)
* [Features](readme_docs/FEATURES.md)
* [Development](readme_docs/DEVELOPMENT.md)
* [Development Recipes](readme_docs/DEVELOPMENT-RECIPES.md)
* [Testing](readme_docs/TESTING.md)
* [Deploy](readme_docs/DEPLOY.md)

## Features

___Web Services___
* [API Documentation](#consumer-api-docs)
* [User Management Services](#user-management-services)

__Security__
* [Role Based Authorization](#role-based-authorization)
* [OAuth2 Authentication](#oauth2-authentication)
* [Auditing](#access-and-data-auditing)

__Technology__
* [RESTful API](#restful-api)
* [Integrated Test Suite](#integrated-test-suite)

__Team__
* [Development Team Standards](#development-team-standards)
* [Documented Best Practices](#documented-best-practices)

#### RESTful API
[Web Service Patterns](readme_docs/WEB-SERVICE-PATTERNS.md)

#### API Documentation

#### OAuth2 Authentication

#### Role Based Authorization
[Authorization](readme_docs/AUTHORIZATION.md)

#### User Management Services

#### Activity and Data Auditing

##### Activity Audit
Activity auditing captures all requests made to a particular API endpoint and writes a record with metadata to the database. The metadata that is collected would include the authenticated user, requestor's IP Address, current date and time, request URL, request body (if POST, PUT, PATCH), and any other pertinent information the developer has accessible and determines should be in the activity audit log. 

** Talk about response status code and result. How is this logged?

#### Integrated Test Suite

#### Documented Best Practices


