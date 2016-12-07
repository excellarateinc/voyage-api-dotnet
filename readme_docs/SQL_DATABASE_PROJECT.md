## Schema Changes

Coming soon!

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
PI;Persist Security Info=False;Data Source=localhost;Initial Catalog=Launchpad" /SourceFile:".\Launchpad.Database.dacpac" /Action:Publish
Publishing to database 'Launchpad' on server 'localhost'.
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

$dp = [Microsoft.SqlServer.Dac.DacPackage]::Load(".\Launchpad.Database.dacpac")



try{
    # register event. For info on this cmdlet, see http://technet.microsoft.com/en-us/library/hh849929.aspx 
    register-objectevent -in $dacService -eventname Message -source "msg" -action { out-host -in $Event.SourceArgs[1].Message.Message } | Out-Null
 
    echo "Starting Deployment"
    $result =  $dacService.deploy($dp, "Launchpad", "True")     
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
curity=SSPI;Persist Security Info=False;Data Source=localhost;Initial Catalog=lp" /SourceFile:".\Launchpad.Database\bin\Debug\Launchpad.Database.dacpac" /Action:S
cript /OutputPath:Launchpad.Database-2016-07-12-09480717.sql
Generating publish script for database 'lp' on server 'localhost'.
Successfully generated script to file C:\code\Launchpad.Database-2016-07-12-09480717.sql.
```
