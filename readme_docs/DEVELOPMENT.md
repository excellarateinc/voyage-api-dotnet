## Development
Information on how the development team agrees to work together along with information on setting up a local dev environment. Keep this document fresh with any and all information that should be shared with your fellow devs.

## Table of Contents
* [Core Values](#core-values)
* [Code Standards](#code-standards)
* [Workstation Setup](#workstation-setup)
* [Run App Locally](#run-app-locally)
* [Code Branching](#code-branching)
* [Code Review & Merge](#code-review--merge)
* [Continuous Integration](#continuous-integration)

Also check out [Development Recipes](DEVELOPMENT-RECIPES.md).

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
> __FINISH THIS SECTION__

The following coding standards are agreed upon by the development team and are expected to be followed. Where possible, these rules will be enforced by the IDE.

__Langauges__
* [C#](#STANDARDS-CSHARP.md)
* [SQL](#STANDARDS-SQL.md)
* XUnit

__Best Practices__
* [Authorization](STANDARDS-AUTHORIZATION.md)
* [Database](STANDARDS-DATABASE.md)
* [Web Services](STANDARDS-WEB-SERVICES.md)
* ?? [Logging](DEVELOPMENT-BEST-PRACTICES.md#logging)
* ?? [Dependency Injection](DEVELOPMENT-BEST-PRACTICES.md#dependency-injection)

:arrow_up: [Back to Top](#table-of-contents)

## Workstation Setup
> __FINISH THIS SECTION__

### Required Software
Download and install the following required software for development:
* [Visual Studio 2015 or greater](https://www.visualstudio.com/vs/)
* [SQL Server 2012 or greater](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
* IIS (Turn Windows feature on)
* [Java 1.8 SDK (for Liquibase Database Migration)](http://www.oracle.com/technetwork/java/index.html)
* [Node](https://nodejs.org/en/)
* [apiDocJs](http://apidocjs.com/)

### Instructions
> __EXPLAIN the process start-to-finish with no assumptions__

1. Download source via Visual Studio GitHub extension
   - Install GitHub extension [[Download](https://visualstudio.github.com)]
   - GitHub Repo: https://github.com/lssinc/launchpad-dotnet-api
   - Open Visual Studio
   - Go to the "Team Explorer" tab
   - Click the "Manage Connections" button.
   - Under the GitHub section, click "Clone".
   - Enter your GitHub credentials.
   - Choose "launchpad-dotnet-api" from the list of repositories.
   - Once cloning is complete, open the "Launchpad.API" solution.
2. Nuget Restore
   - Visual Studio should automatically restore the dependencies on the first build.
   - If packages aren't restored on build, you have two options. 
     * You can right click the solution in Visual Studio and select "Restore NuGet Packages".
     * Go to the "Tools" tab, select "NuGet Package Manager" then "Package Manager Console". From the console that shows up, click the "Restore" button in the upper right corner.
3. Create the database
   - NOTE: This app uses a SQL Database and Code First Migrations. This migration strategy will be replaced with a TBD tool.
   - In Visual Studio, open the package manager console by going to the "Tools" tab, select "NuGet Package Manager" then "Package Manager Console". 
   - Set the Default project to Launchpad.Data in the "Default project" dropdown.
   - Type "Update-Database" (no quotes) in the console and hit enter to create the database. This will run the Entity Framework migration scripts. If this fails with a message telling you the command is unrecognized, restart Visual Studio and try again.
     * The connection string in Launchpad.Web web.config determines where the database will be created
     * The default is localhost/sqlexpress with initial catalog Launchpad
     * When the web.config connection string is changed, update the connection string in Launchpad.Data.IntegrationTests
4. Build
   - Use the Visual Studio Build/Debug menus to compile and start the project. These menu options will invoke MSBUILD with the appropriate parameters.
5. Test
   - Use the Visual Studio Test Explorer to view and execute the unit and integration tests. 
     * To open the test explorer, click the "Test" tab in Visual Studio, select "Windows" then "Test Explorer".
   - NOTE: A local database is required in order for integration tests to complete successfully. 

> __SEED DATA__
>
> A default "admin" user with full access to everything will be seeded into the database when _Update-Database_ is run. Use this account when initially interacting with the web services or creating new users or applying new user roles. 
```
Username: admin@admin.com
Password: Hello123!
```

:arrow_up: [Back to Top](#table-of-contents)

## Run App Locally
> __FINISH THIS SECTION__

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

:arrow_up: [Back to Top](#table-of-contents)
