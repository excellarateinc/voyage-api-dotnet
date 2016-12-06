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
:r .\Scripts\Seed\Role.sql
:r .\Scripts\Seed\User.sql
:r .\Scripts\Seed\RoleClaim.sql
:r .\Scripts\Seed\UserPhone.sql
:r .\Scripts\Seed\UserRole.sql
:r .\Scripts\Seed\Widget.sql

