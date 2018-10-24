## Overview
A foundational set of web services that implement industry standard guidelines, common best practices, and the experienced insights afforded to Lighthouse Software thru decades of enterprise business software development. 

Created and supported by Lighthouse Software @ https://LighthouseSoftware.com

## Topics
* [5 Minute Test](#5-minute-test)
* [Features](#features)
* [Development](readme_docs/DEVELOPMENT.md)
* [Development Recipes](readme_docs/DEVELOPMENT-RECIPES.md)
* [Testing](readme_docs/STANDARDS-TESTING.md)
* [Deploy](readme_docs/DEPLOY.md)

## 5 Minute Test
Run the Voyage API and execute a JSON API request within 5 minutes

1. Prerequisites 
   - Ensure you have the latest versions of [Visual Studio](https://www.visualstudio.com/vs/), [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-editions-express) (default instance), and [IIS Express](https://www.microsoft.com/en-us/download/details.aspx?id=48264) installed.
2. Open Visual Studio with administrator privileges.
   - Right-click on the Visual Studio icon and select "Run as administrator".
3. Download source via Visual Studio GitHub extension
   - Open Visual Studio's "Team Explorer" tab and click the "Manage Connections" button. 
   - Under the GitHub section, click "Clone" and enter your GitHub credentials.
   - Choose "voyage-dotnet-api" from the list of repositories.
     * The official repository is located here https://github.com/lssinc/voyage-api-dotnet
   - Click "Clone". When done cloning, open the "Voyage.API" solution.
4. Create the database
   - Open the Database project within the Voyage solution
   - Double-click the localhost.publish.xml file within the root project folder
   - Once the publish script builts, a Publish dialog box will appear
   - Click the Publish button within the Publish dialog box to create the database and execute the scripts.
   - Upon seeing "Publish completed successfully" within the console output, the database will be created and populated. 
5. Start the Authentication service
   - Select the Voyage.Web project
   - Choose the Visual Studio menu option Debug > Start Without Debugging (Ctrl + F5) to execute within IIS Express.
5. Start the Web API service
   - Select the Voyage.Api project
   - Choose the Visual Studio menu option Debug > Start Without Debugging (Ctrl + F5) to execute within IIS Express.
6. Get an access token
   - Using [Postman](https://www.getpostman.com/apps), create a new "POST" request.
   - Set the url to http://localhost:52431/oauth/token
   - In the "Body", use x-www-form-urlencoded and fill in the following key/value pairs:
     - grant_type: password
     - username: admin@admin.com
     - password: Hello123!
     - client_id: 123456
     - client_secret: abcdef
   - Click "Send". You should receive an access token back.
7. Test the API
   - Using [Postman](https://www.getpostman.com/apps), create a new "GET" request.
   - Set the url to http://localhost:55850/api/v1/users
   - Add a header where the key is "Authorization" and the value is "Bearer `<token>`". Replace `<token>` with the full token string from the previous request.
   - Click "Send".
   
## Features

### Web Services
* __HTTP Compliant RESTful API__
  - Follows HTTP protocols for RESTful web services
  - Lightweight JSON requests and responses
  - See our [Web Service Standards](readme_docs/STANDARDS-WEB-SERVICES.md)
* __Public API Status Service__
  - Web service that provides general status of the API to the public
  - Helpful endpiont for automated monitoring
* __User Administration Services__
  - Full suite of user administration web services (list, get, create, update, delete)
  - Secured access through role based security
* __Account Management Services__
  - Users can update their account information themselves
  - Manage account settings
  - Password reset
* __API Documentation__
  - Complete documentation for web services consumers
  - Includes detailed descriptions and example to quickly interact with the API

### Security
* __[OWASP](https://www.owasp.org/index.php/Category:OWASP_Top_Ten_Project) Hacker Proof__
  - Tested nightly against OWASP common hacks (particularly the top 10)
  - Tested nightly using 3rd party penetration testing services to ensure enterprise grade security.
* __[OAuth2](https://oauth.net/2/) Authentication__
  - Bearer Token authentication configuraiton
  - SHA2 hash encrypted user password (when authenticating using the database)
  - Supports other authentication methods
* __Active Directory / LDAP Authentication__
  - Extends OAuth2 to support authentication with an AD/LDAP system
  - Supports Enterprise SSO environments using AD/LDAP
* __Role Based Authorization__
  - Custom role definitions to suit any situation
  - Supports granular security permissions 
  - Full suite of role administration web services (list, get, create, update, delete)
* __Forgot Username / Password Support__
  - Web services that allow users to reset their username and/or password
  - Validates a user via their email address
* __Auditing__
  - Complete enterprise access and data auditing to meet compliance requirements
  - HTTP Request / Response logging to track user activity (anonymous and authenticated users)
  - Database change logging to track manipulation of data over time (anonymous and authenticated users)

### Tech Stack
* __JSON RESTful Web Services__
  - JSON request/response interaction
  - Strict [REST implementation](readme_docs/STANDARDS-WEB-SERVICES.md)
  - [apiDoc](http://apidocjs.com) documentation generated from source code comments
* __Microsoft .NET__
  - [WEB API](https://www.asp.net/web-api) for base web services behavior
  - [Identity Framework](https://www.asp.net/identity) for Authentication and Authorization
  - [Entity Framework](https://www.asp.net/entity-framework) for database ORM
  - Many other frameworks for auditing (OWIN), logging (SeriLog), and much more. 
* __Database Neutral__
  - Capable of integrating with any major database vendor (SQL Server, Oracle, DB2, MySQL, etc)
  - Database interactions follow [SQL99](https://en.wikipedia.org/wiki/SQL:1999) standards with no vendor specific database features relied upon
  - Liquibase database migrations produce on-demand SQL specific to the integrated database
* __Integrated Test Suite__
  - Automated test coverage using [XUnit](https://xunit.github.io) and [Moq](https://github.com/Moq/moq4/wiki/Quickstart) frameworks
  - Tests executed during every build to ensure high quality code coverage
* __Continuous Integration (CI)__
  - Jenkins CI jobs able to invoke MSBuild and apiDoc commands to build, test, and package
  - Jenkins jobs included with with API source
  - Supports other CI environments like Team Foundation Server (TFS)

### Developers
* __Team Protocols__
  - Fast learning curve through clear documentation
  - Easy values, standards, best practices that most developers will aggreement
* __Core Values__
  - Documented core values that we believe will resonate with most development teams
  - Unifies teams and promotes healthy communication
  - See our [Core Values](readme_docs/DEVELOPMENT.md#core-values) documentation
* __Coding Standards__ 
  - Industry accepted language coding standards
  - Best practices when developing within the code base
  - See our [Development Team Standards](#development-team-standards)

### System Administrators
* __Deploy Instructions__
  - Full instructions on how to properly build, test, and package the API app for deploy
  - Continuous Integration job templates for QA, UAT, and PROD
* __Docker Support__
  - Preconfigured Dockerfile for deployment within Amazon Web Services environment
  - Generate a Docker bundle for distribution using built-in tasks
  - Customize to fit any environment
* __Amazon Web Services (AWS) - Elastic Beanstalk__
  - Supports AWS Elastic Beanstalk using a Docker image
  - Run a build task to generate an AWS EB compatible .zip file
* __API Monitoring__
  - Configure automated web uptime monitoring to use the Status Web Service
* __DevOps Ready__
  - [Ansible](https://www.ansible.com) scripts for deploying the API Docker image to the Amazon Web Service (AWS) environment
  - Customize scripts to support any environment
