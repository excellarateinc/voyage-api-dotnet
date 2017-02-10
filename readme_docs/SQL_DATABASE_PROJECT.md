## Schema Changes
SQL Server Database projects work by defining the desired state of the database. This means the output of the project (.dacpac) contains the definition of how the database should look after it is applied. It differs from a migration strategy which describes how the database should change. For futher details around this topic [see](http://workingwithdevs.com/delivering-databases-migrations-vs-state/).

Schema is defined via DDL. From a developer perspective, all changes will be implemented by modifying or creating a CREATE TABLE statement. When the .dacpac is deployed, the appropriate changes to transition the target to the .dacpac state are generated. For example, when a new column [Deleted] was added to existing the table [UserPhones] the .sql file looked like:

```
CREATE TABLE [dbo].[UserPhone] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [UserId]      NVARCHAR (128) NOT NULL,
    [PhoneNumber] NVARCHAR (15)  NOT NULL,
    [PhoneType]   INT            NOT NULL,
    [Deleted] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_dbo.UserPhones] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.UserPhones_dbo.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[UserPhone]([UserId] ASC);


```

The actual change applied to the existing database was:

```
ALTER TABLE [dbo].[UserPhone]
    ADD [Deleted] BIT DEFAULT 0 NOT NULL;
```

If the column was renamed to Deleted2 the .sql file in the project would be updated to:

```
CREATE TABLE [dbo].[UserPhone] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [UserId]      NVARCHAR (128) NOT NULL,
    [PhoneNumber] NVARCHAR (15)  NOT NULL,
    [PhoneType]   INT            NOT NULL,
    [Deleted2] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_dbo.UserPhones] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.UserPhones_dbo.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[UserPhone]([UserId] ASC);
```

And the change would be applied to the database as:
```
EXECUTE sp_rename @objname = N'[dbo].[UserPhone].[Deleted]', @newname = N'Deleted2', @objtype = N'COLUMN';
```

### Project Structure
All .ddl is grouped by namespace and type. The the .sql file is named after the object. For instance, the .sql file that defines [dbo].[User] would be found in dbo\Tables\User.sql.

### Adding or Updating Structures
1. Create a new .sql file in the corresponding location
  1. Follow the project structure guidance above - each file should be grouped by namespace and type and named after the object it impacts or creates
2. Define the object using T-SQL or the built in designer
3. Build the project
4. Review generated .ddl by using the generate script functionality of SqlPackage.exe
  1. This helps ensure the intended change will be applied. Note: This is only of use when there is a database that reflects the current database state
5. Once the changes are correct, check in the changes and the continuous deployment process will apply the changes to the necessary databases


## Reference Data
Reference data can be loaded via post-deployment scripts. Note: This option should be used for small datasets. 
Large datasets should use other tools for import.

### Adding reference data
The following steps outline creating a new script for reference data.

1. Add a post-build script to Scripts\Seed. This script should have the same name as the table that is going to be populated. For instance,
a script that loads the Role table should be named Role.sql.
    1. The script should support re-running or in other words idempotent. 
    2. Use the merge syntax to allow for the handling of inserts, updates, and deletes
2. Once the script is complete, modify the Post-Deployment.sql script to call the new reference data script
    1. It is important to put the script in the correct order. The scripts will execute in the order in which they are listed.
    2. Use the :r syntax to include the new script
    
#### Reference Data Script Example
````
WITH Role_CTE 
AS (
	SELECT *
	FROM (
		VALUES 
			 (N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'Administrator')
			,(N'927fe4a9-4e27-4635-a208-eb5afc953294', N'Basic')			 
		) AS RoleSeed([Id], [RoleName])
	)
-- Reference Data for Role 
MERGE INTO dbo.[ROLE] AS [Target]
USING Role_CTE AS [Source]
	ON [Target].[NAME] = [Source].[RoleName]
WHEN NOT MATCHED BY TARGET
	THEN
		INSERT (
			  [Id]
			, [NAME]
			)
		VALUES (
			 [Source].[Id]
			,[Source].[RoleName]
			)
		OUTPUT $action, inserted.*, deleted.*;

````

#### Post-Deployment.sql Example
```
/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
:r .\Seed\Role.sql
:r .\Seed\User.sql
:r .\Seed\RoleClaim.sql
:r .\Seed\UserPhone.sql
:r .\Seed\UserRole.sql
:r .\Seed\Widget.sql

```


## Deployment
When a database project builds, the output is a .dacpac file that can be used to apply the changes against a new or existing database. There are a number of options for deploying the package. 

###SqlPackage.exe 
This is a utility that can deploy a .dacpac file. Additionally, it can also be used to generate script that will update a database to the state in the .dacpac.

#### Sample Command
```
"C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\Extensions\Microsoft\SQLDB\DAC\130\SqlPackage.exe" /TargetConnectionString:"Integrated Security=SS
PI;Persist Security Info=False;Data Source=localhost;Initial Catalog=Voyage" /SourceFile:".\Voyage.Database.dacpac" /Action:Publish
Publishing to database 'Voyage' on server 'localhost'.
Initializing deployment (Start)
Initializing deployment (Complete)
Analyzing deployment plan (Start)
Analyzing deployment plan (Complete)
Updating database (Start)
Update complete.
Updating database (Complete)
Successfully published database.
```

###Powershell
PowerShell is another option for deploying a .dapac. Below is a sample script that will deploy a package. Note the links in the comment - this script was developed using those resources.

```
#https://blogs.msmvps.com/deborahk/deploying-a-dacpac-with-powershell
#http://www.systemcentercentral.com/deploying-sql-dacpac-t-sql-script-via-powershell/
#PowerShell.exe -ExecutionPolicy AllSigned
#https://msdn.microsoft.com/powershell/reference/5.1/Microsoft.PowerShell.Core/about/about_Execution_Policies

add-type -path "C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\Extensions\Microsoft\SQLDB\DAC\130\Microsoft.SqlServer.Dac.dll"

$dacService = new-object Microsoft.SqlServer.Dac.DacServices "server=localhost;Integrated Security = True;"

$dp = [Microsoft.SqlServer.Dac.DacPackage]::Load(".\Voyage.Database.dacpac")



try{
    # register event. For info on this cmdlet, see http://technet.microsoft.com/en-us/library/hh849929.aspx 
    register-objectevent -in $dacService -eventname Message -source "msg" -action { out-host -in $Event.SourceArgs[1].Message.Message } | Out-Null
 
    echo "Starting Deployment"
    $result =  $dacService.deploy($dp, "Voyage", "True")     
}
catch [Exception]{
    echo $_.Exception|format-list -force
}

# clean up event 
unregister-event -source "msg" 
echo "Done."
```

###T-SQL Script
A T-SQL script can generated using the .dacpac and a target database. This script will upgrade the target to match the state of a .dacpac. In this scenario, a dba can take the package and run this command to review the changes and optionally apply the script to upgrade the database.

####Sample Command
```
"C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\Extensions\Microsoft\SQLDB\DAC\130\SqlPackage.exe" /TargetConnectionString:"Integrated Se
curity=SSPI;Persist Security Info=False;Data Source=localhost;Initial Catalog=lp" /SourceFile:".\Voyage.Database\bin\Debug\Voyage.Database.dacpac" /Action:S
cript /OutputPath:Voyage.Database-2016-07-12-09480717.sql
Generating publish script for database 'lp' on server 'localhost'.
Successfully generated script to file C:\code\Voyage.Database-2016-07-12-09480717.sql.
```
