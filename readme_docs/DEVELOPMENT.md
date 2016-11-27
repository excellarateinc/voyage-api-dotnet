## Development
Information on how the development team agrees to work together along with information on setting up a local dev environment. Keep this document fresh with any and all information that should be shared with your fellow devs.

## Table of Contents
* [Core Values](#core-values)
* [Code Standards](#code-standards)
* [Workstation Setup](#workstation-setup)
* [Run App Locally](#run-app-locally)
* [Code Branching](#code-branching)
* [Code Review & Commit](#code-review-merge)
* [Deploy (QA, UAT, PROD)](#deploy-qa-uat-prod)

Also check out the [Development HOW TO Guide](DEVELOPMENT-HOWTO.md).

## Core Values
These are the core standards that our development team agrees to follow. All of our decisions should be tested against our core values. Only a team decision can overrule our core values on a case-by-case basis. 

1. __Always act in the best interest of our customer's time, money, and desired quality__
  * We succeed when our customer is amazed at our ability to deliver on-time, within budget, and with the highest possible quality given the time and money constraints. 
  * Test every decision with the question "Does this align with the customer's budget, timeline, and quality expectations?"
2. __Be respectful always (aka Golden Rule)__
  * Teams that respect each other with their words and actions achieve high throughput consistently
  * When we talk down to each other we are deciding that WE are more important than the customer!
3. __No Lone Ranger Developers__
  * Acting on your own excludes your team from decision making, which is a form of disrespect
  * Perfect code is a product of team collaboration and communication
4. __Peer code reviews are required__
  * For every 10 liines of code written, ~ 1 bug is introduced into the code base. 
  * Peer reviewed code has been found to significantly reduce bugs and missed requirements
  * Be thorough and respectful!
5. __Build for today's needs, not what might be needed tomorrow__
  * Make the best choices based on the customer's concrete needs today!
  * Building for the future is subjective guesswork that rarely meets unknowable customer needs

:arrow_up: [Back to Top](#table-of-contents)

## Code Standards
The following coding standards are agreed upon by the development team and are expected to be followed. Where possible, these rules will be enforced by the IDE.
* C#
* XUnit

:arrow_up: [Back to Top](#table-of-contents)

## Workstation Setup

### Required Software
Download and install the following required software for development:
* Visual Studio 2015
* SQL Server Express 20xx
* IIS ?
* Java 1.8 SDK (for Liquibase Database Migration)
* [Node](https://nodejs.org/en/)
* [apiDocJs](http://apidocjs.com/)
  - npm install -g apidoc 

### App
1. Download source via Visual Studio GitHub extension
  * Install GitHub extension [[Download](https://visualstudio.github.com)]
  * GitHub Repo: https://github.com/lssinc/launchpad-dotnet-api
2. SQL Server 
   - Default instance: localhost\sqlexpress
2. Finish this guide

### Database
The application uses a SQL Server database and code first migrations. 

1. In Visual Studio, open the package manager console
2. Set the Default project to ***Launchpad.Data***
3. Run Update-Database from the console to create the database
   - The connection string in ***Launchpad.Web*** web.config determines where the database will be created
   - The default is localhost/sqlexpress with initial catalog Launchpad
   - When the web.config connection string is changed, update the connection string in ***Launchpad.Data.IntegrationTests***

__Seed Data__

A default "admin" user with full access to everything will be seeded into the database when _Update-Database_ is run. 

```
Username: admin@admin.com
Password: Hello123!
```

:arrow_up: [Back to Top](#table-of-contents)

## Run App Locally
1. In VS2015...

:arrow_up: [Back to Top](#table-of-contents)

## Code Branching
The following code branching strategy is meant to ensure the following objectives: 
* Encourage the benefits of frequent commits/pushes to the Git repo
* Isolate work-in-progress from other developers
* Tag branches with meaningful names to identify features
* Only promote peer reviewed code into the "master" branch
  
> __All Branches Work The Same Way__ There are many strategies that may be employed when branching. Regardless of the branching strategies that are used, it's helpful to know that technically branches all function the same way. Terms like "long running branches" or "topic branches" simply refer to a strategy for how to use the branching feature of Git.
  
### Long Running Branch: Master
A project can support multiple "long running" branches that are always open and ready for new code. It's typical to have a long running branch called "master" that holds stable code that is currently in production and code soon to be deployed to production. It's not uncommon for development teams to have a long running branch called "develop" that contains less stable code that needs to be tested and verified before being pushed to the more stable "master" branch. 

The current development practice for this team is to only support a "master" long running branch that will hold all code currently in production and new code that has been peer code reviewed. 

### Topic Branches
Topic branches are shorter lived branches are often times referred to as "feature branches". Topic branches can be thought of as workspaces for developers to make their code changes for a single (hopefully small) feature. 
```
 --------- MASTER ---------------------------------------> Production
    \-- Topic #123 --/     \---- Topic #124 ----/
```

Topic Branch Workflow

1. Create a new branch from a long running branch like "master"
  * Name the Topic branch with the ticket number from your task management app + short english description (ie LP-1234 Navbar Quick Search)
  * Creating a good name allows for quick comparison of the task management app and Git for what's in the release and what's not
3. Make code changes
  * Commit to the local Git repo as often as necessary using descriptive commit messages
  * Write meaningful commit messages as these will often be read by peer developers
4. Push code changes
  * Push to the Topic branch in the Git repo as often as necessary so that changes wont be lost if your computer blows up
5. [Pull Request](https://help.github.com/articles/about-pull-requests/)
  * Create a pull request (if available in your Git repo) and ask 1-2 developers to review the code online
  * GitHub and BitBucket both have great diff tools for doing code reviews on-demand and providing comments. 
6. Peer Reviewer Merge
  * Once the peer reviewer has signed-off on the changes, then ONLY THE PEER REVIEWER can merge the changes into the long running branch "master"
  * The developer's name on the merge is the official stamp of approval and statement that the code reviewer takes responsibility for the quality of the code. 
7. Delete Topic Branch
  * Topic branches that have been merged into "master" should be deleted immediately as they are no longer necessary
  * Work with the team to determine if a different time interval is needed for retaining old branches and update this document
  
### Other Strategies
This guide is focusing on the simplest branching strategies so that even the least experienced person on the team (at least with Git) can quickly get familiar with the workflow. Starting with a complext branching strategy can be a frustrating experience that can cause confusion in a team. If your team is an advanced group ready for the next level of Git, then consider updating this section with you preferred Git workflow strategies and training your team so as to avoid confusion and mistakes. 

Read [Git - Branching Workflows](https://git-scm.com/book/en/v2/Git-Branching-Branching-Workflows) for more information. 

:arrow_up: [Back to Top](#table-of-contents)

## Code Review & Merge
Peer code reviews are perhaps the _most_ important team development step in a project. Code reviews are an opportunity for team members to catch oversights in code, bugs, standards, and best practices. Code reviews are also opportunities for developers to teach each other about our documented standards and guidelines as well as to share other general development knowledge and best practices from experience. Further more, code reviews encourage developers to share work-in-progress frequently and to develop respectful and trusting relationships (ie team building). 

Once a thorough code review has been completed, the peer reviewer will perform the merge process to the long running branch (i.e. master), which will stamp the reviewers name on the merge to show the code was reviewed and approved. 

__Rules__
* All tests _must_ pass on the developer's machine before requesting a code review
* It is _your_ responsibility to have _your_ code reviewed in a timely manner
* It is _never_ acceptable to have your code merged into a long running branch without a code review
* Merging code into a long running branch can only be performed by the peer code reviewer
* You are required to fix any violations of the __documented__ team dev standards/guidelines
* Peer reviewer recommendations that are not supported by documented team dev standards/guidelines are optional
* Seek to have your code reviewed by someone more experienced so that you can learn something new!
* Change up who reviews your code to promote cross training and to learn from new people
* Make time to provide a thoughtful code review. 
* Be kind and follow the [Golden Rule](https://en.wikipedia.org/wiki/Golden_Rule) 

:arrow_up: [Back to Top](#table-of-contents)

## Continuous Integration
Continuous Integration (CI) is the process of integrating and testing new code as soon as the code has been merged into a long running branch. Typically CI is managed by a system like [Jenkins](https://jenkins.io/index.html) that have custom jobs created for each project and environment combination. CI jobs can be executed manually, through commits in a source code repo, or through plugins that support nearly any type of trigger. 

Developers must be aware that as soon as their code is merged into a long running branch (ie master), that the CI job "Build & Test" will be triggered on the new code. The primary function of the "Build & Test" job is to ensure that the latest changes can be built without error and that all unit, integration, and functional tests pass successfully. If the build or a single test fails, then the job fails and notifies the development team. 

Whenever a job fails within the CI environment the whole development team is affected. It is the responsibility of the developer who broke the build to fix the issue as quickly as possible. A developer with repeated offenses should be made aware (however the team decides) that they need to improve their pre-merge quality checks. 

## Deploy (QA, UAT, PROD)
Deploying to the upper environments (DEV -> QA -> UAT -> PROD) will be handled by the Continuous Integration (CI) environment. The app is expected to contain all necessary tasks for building, testing, and packaging within it's build framework for execution on the command line. 

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