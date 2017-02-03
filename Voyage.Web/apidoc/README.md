## apiDoc

#### Overview
[apiDoc](http://apidocjs.com) is a NodeJS app that scans files for documentation relating to a web services API. apiDoc is used within this app to generate human readable JSON web services API documentation in HTML/CSS. To generate the documentation, apiDoc will scan select source files (defined in package.json > scripts > 'apidoc' task) for apiDoc specific documentation syntax. Embedding the API documentation within the source files keeps the code and documentation together for easier maintenance. 

There are a number of other tools available for generating API documentation, but apiDoc has a much lower learning curve and is very intuitive. 

For complete information on how to use and deploy API Doc for this application, see the [Deploy documentation](../../readme_docs/DEPLOY.md). 

#### Configuration
The apidoc configuration is defined within the apidoc.json file. Documenation on each configuration setting can be found on the [apiDoc website](http://apidocjs.com). 

#### Common Definitions
A set of common definitions are documented within this folder. The naming convention for common definition files is to prefix the filename with "apidoc-" and end the filename with ".js". 

```
apidoc-errors.js
apidoc-header.js
apidoc-users.js
```

The command to generate the documentation will scan all .cs and .js files throughout the codebase, which will include the common definition files in this folder. For more information on the command that executes apidoc, see the package.json file in this folder. 

## Quick Start
### 1. Install NodeJS 
* Go to the NodeJS website at [https://nodejs.org](https://nodejs.org) 
* Download the distribution for your OS
* Double-click the download to initiate the installation process

### 2. Install apiDoc
* Open a terminal / command prompt 
* Execute the following npm install command. 
```
c:\ > npm install apidoc -g
```

> NOTE: Make sure to include "-g" global install command as this will allow you to invoke apiDoc regardless of which directory you are in. 

### 3. Generate the Docs
* Open a terminal / command prompt 
* Change directory to c:\your-workspace\launchpad-dotnet-api\apidoc
```
c:\ > cd c:\your-workspace-here\launchpad-dotnet-api\apidoc
```
* Execute the apidoc npm command

```
npm run apidoc
```

* Validate that the HTML documentation was generated in the ./docs folder

> NOTE: If any of the apidoc syntax is incorrect, then you will see the errors printed out in the command prompt window with detailed explainations of what is broken. Once you have resolved these sytanx issues, then re-run this command to generate the docs. 


### 4. Open the docs in a browser
This assumes that you are wanting to view the apidoc generated HTML locally on your own machine. If you are deploying the documentation to a server, then read the [DEPLOY documentation](../../readme_docs/DEPLOY.md) for detailed instructions on how to deploy to a server. 

* Open your favorite web browser
* File > Open...
* Navigate to the apidocs "docs" folder
```
c:\your-workspace\launchpad-dotnet-api\apidoc\docs
```
* Select the "index.html" and click "Open" or "Ok"
