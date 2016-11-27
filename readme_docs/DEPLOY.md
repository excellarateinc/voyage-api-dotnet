## Deploy
Deploying to the upper environments (QA -> UAT -> PROD) will be handled by the Continuous Integration (CI) environment. The app is expected to contain all necessary tasks for building, testing, and packaging within it's build framework for execution on the command line. 

## Table of Contents
* [Continuous Integration (CI)](#continuous-integration-ci)
* [Server Configuration](#server-configuration)
* [App Build & Test](#app-build--test)

## Continuous Integration (CI)
[Jenkins](https://jenkins.io) continuous integration build manager will be used to configure jobs and triggers to faciliate the CI process. Reference your development team's WIKI for links to the Jenkins environment. 

### Quality Assurance (QA)
The QA environment is where the Quality Assurance team will exercise the new code in an effort to "sign off" on the feature. The QA team is typically signaled that a feature is ready for testing by updating a status within a work tracking system. Before the QA team will begin testing, they will look at the CI environment build logs for the latest Git commit comments. Creating well described Topic Branch names that include the work ticket number as well as well written commit message will communicate to the QA team what is included in the most recent QA build. 

__CI Job Name:__ QA Build, Test, & Deploy
* Pulls the latest source code from Git "master" long running branch
* Executes the "build" task to compile the source
* Executes the "test" task to run all unit, integration, and functional tests
* Tags the source code with the current version (1.1) + build (544). For example: 1.1.544
* Executes a deployment script that automates the complete deployment to the QA server(s)

### User Acceptance Testing (UAT)
The UAT environment is where stable code from QA is deployed so that the [Product Owner(s)](https://www.mountaingoatsoftware.com/agile/scrum/product-owner) (PO) exercise the new features. When the PO determines that the requested feature(s) meet the acceptance criteria, then the PO will update the work ticket and the feature will be left alone in the "master" branch for an eventual production release. 

__CI Job Name:__ UAT Build & Deploy
* Requests the source code tag to download and build
* Pulls the source code for the given tag from Git "master" long running branch
* Executes the "build" task to compile the source
* Executes a deployment script that automates the complete deployment to the UAT server(s)

### Produduction (PROD)
The PROD environment is where stable software is made available for live usage by the intended users. PROD is almost always very strictly guarded to ensure uptime of the app, data quality (no testing data), and restrict usage to authorized personnel (in case of sensitive data). 

Deploying apps to PROD are often times NOT automated processes unless proper controls are in place to prevent accidental deploys. In the case of this guide, all PROD deploys will be initiated manually by first creating a PROD distribution bundle and then manually executing the steps to properly deploy the software. 

__CI Job Name:__ PROD Build, & Package
* Requests the source code tag to download and build
* Pulls the source code for the given tag from Git "master" long running branch
* Executes the "build" task to compile the source
* Executes a deployment script that automates the complete deployment to the UAT server(s)

### Source Code Tagging
Source code tagging is used to mark a bundle of code that is ready for promotion into the upper environments. An ideal tagging strategy for all non/technical parties involved on a project is to tag a build once and then push that tag to the upper enviornments. Keeping track of a single tag/version is much more simple than having a tag for each environment (QA, UAT, PROD). For example, whenever a QA build completes successfully, a source code tag is applied in Git, like 1.1.544. When the QA team signs off on a tag/version of the app, then they can cite the tag/version that should be promoted to UAT without any confusion. When a Product Owner is testing in UAT, they can sign off on the final stable tag/version that should be promoted to Production. When creating the work order for pushing an app to production, the exact tag/version can be specified so that a precise point-in-time signed-off snapshot of the code is sent to production. 

__Rules__
* Tagging is _only_ applied after a successful build & test of the "QA Build, Test, & Deploy" job
* Tags have the format of _major-version.minor-version.build-number_ like 1.1.544
* Environments higher than the QA environment will request the tag name of the source to build and deploy
* Complete version number must be displayed in the footer of the app (1.1.544)

### Paired PROD Deploys
Deploying to production should always be done in pairs of two peers! Accidents will happen, but they can be mostly prevented if by having a peer watchfully checking deployment steps as they are performed. 

__Rules__ 
* Production deployments require 2 qualified system and/or software engineers
* Production deployment procedures _must_ be documented vertabim so that any system/software engineer can execute them
* Production deployment procedures _must_ document rollback procedures in the event of an error
* Before each action is performed it must be verbally confirmed by the other person(s)
* All parties involved in the production deploy _must_ manually test that the deploy released the correct version of the software
* All parties involved must verbally agree that the software has been successfully deployed
* In the event of an unresolveable error, the deployment team must contact a manager and an expert to assist with the resolution of the issue. 
* Once the deploy has been determined a success or failure, then an email must be sent to the entire project team with details (if necessary)

If at all possible, automate as much of the deployment process as possible to prevent human errors. 

:arrow_up: [Back to Top](#table-of-contents)

## Server Configuration
> __FINISH DOCUMENTATION__

### Windows Server 2012

### IIS

#### Application Pool
The application pool should be configued as follows:

* Framework => .Net Framework v4.0.30319
* Managed pipeline mode => Integrated

#### Connection Strings
The application assumes that the connectionString will be called LaunchpadDataContext and this setting will be inherited. This can be placed in the machine or a parent site configuration. 

Please note that the default IIS editor does not appear to add the required providerName attribute to the connectionString. As a result, it may be necessary to edit the web.config directly in order to add the attribute. Failure to do so will result in a 500 error.

:arrow_up: [Back to Top](#table-of-contents)

## App Build & Test
> __FINISH DOCUMENTATION__

### Prerequisties
The following are the prequisties for building the application from the CI server.

* Visual Studio 2015 <-- REALLY? 
* SQL Server <-- REALLY? 
* .NET Framework vXXX.XXX
* Node
* apiDocJs
  1. npm install -g apidoc

### 1. Nuget Restore
The API project uses nuget to manage external dependencies. The first step of the build process is to run **nuget.exe restore Launchpad.API.sln**

### 2. MSBUILD Command
The following msbuild command can be used to build the project. 

```
msbuild /t:Rebuild /p:OutDir=..\publish\;Configuration=Release;UseWPP_CopyWebApplication=True;PipelineDependsOnBuild=False Launchpad.API.sln
```

Notes:
* **/t:Rebuild** specifies that all projects should be rebuilt
* **/p:** represents ; delimited parameters
* **OutDir** specifies where the build output should be placed
* **Configuration** represents the target configuration
* **UseWPP_CopyWebApplication=True;PipelineDependsOnBuild=False** will force the web project output to be placed in the publish/\_publishedWebsite/{ProjectName} folder
  1. These artifacts can be used deploy the website

### 3. Unit Tests
Unit tests should be run after MSBUILD by excuting the batch script **execute-unit-tests.bat** - the result code will be non-zero when a test fails. 

The script scans the publish folder for any .dlls that contain "UnitTests" in the filename. This script is purposely excluding integration tests since an integration test strategy has not be finalized. 

### 4. APIDoc
API doc should be run after unit tests. This process ouputs a set of static files. 

The following command will run the apidoc process

```
apidoc -o publish\_PublishedWebsites\Launchpad.Web\docs -i .\\Launchpad.Web -f ".cs$" -f "_apidoc.js"
```

Notes:
* apidoc is assumed to have been installed as a global node module
* -o is the output directory. 
  1. In the above example, the deployable artifacts are assumed to be placed in publish\_PublishedWebistes\Launchpad.Web\docs
* -i is the input directory
  1. In the above example, it assumes the command is executed from the root repo directory. 
* -f are filters. These are files that will be evaluated for documentation comments.
  1. In the above example, there are 2 filters. The first filter includes only .cs files. The second filter, includes a 
  special javascript file that contains reusable comment blocks.
  
### 5. Artifacts for deployment
The artifacts that should be deployed will be contained in the publish\_PublishedWebsites\Launchpad.Web folder. The artifacts will include both the apidoc files as well as the API bin files.

:arrow_up: [Back to Top](#table-of-contents)
