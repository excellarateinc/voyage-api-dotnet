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

Coming soon!
