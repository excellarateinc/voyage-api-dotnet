## Prerequisties
The following are the prequisties for building the application both locally and in a CI environment.


* Visual Studio 2015
* SQL Server
* Node
* apiDocJs
  1. npm install -g apidoc

## CI

### MSBUILD Command
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



### APIDoc
API doc should be run as part of the build process. This process ouputs a set of static files. 

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
  specila javascript filter that contains reusable comment blocks.
  
  
