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
:r .\Seed\Client.sql
:r .\Seed\ClientScopeType.sql
:r .\Seed\ClientRole.sql
:r .\Seed\ClientScope.sql

