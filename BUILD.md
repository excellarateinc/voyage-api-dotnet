## Prerequisties
The following are the prequisties for building the application both locally and in a CI environment.


* Visual Studio 2015
* SQL Server
* Node
* apiDocJs
  1. npm install -g apidoc

## CI

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
* 
