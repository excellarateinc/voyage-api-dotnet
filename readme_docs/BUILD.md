## Prerequisties
The following are the prequisties for building the application both locally and in a CI environment.


* Visual Studio 2015
* SQL Server
* Node
* apiDocJs
  1. npm install -g apidoc

## CI

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

  
## Local

### 1. Nuget Restore
Visual Studio should automatically restore the dependencies on the first build. If this does not occur, use the package manager console to restore the dependencies.

### 2. Create the database
The application uses a SQL Database and Code First Migrations. This migration strategy will be replaced with a TBD tool.

1. In Visual Studio, open the package manager console
2. Set the Default project to Launchpad.Data
3. Run Update-Database from the console to create the database
  1. The connection string in Launchpad.Web web.config determines where the database will be created
  2. The default is localhost/sqlexpress with initial catalog Launchpad
  3. When the web.config connection string is changed, update the connection string in Launchpad.Data.IntegrationTests

### 3. Build Command
Use the Visual Studio Build/Debug menus to compile and start the project. These menu options will invoke MSBUILD with the appropriate parameters.

### 4. Unit Tests
Use the Visual Studio Test menu to execute the unit and integration tests. The test explorer will show the results. In this case, both integration and unit tests will be run since the assumption is that there is a local database.

