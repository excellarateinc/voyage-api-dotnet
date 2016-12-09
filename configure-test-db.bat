@echo off

:: Need to install - https://msdn.microsoft.com/en-us/library/mt204009.aspx
:: DacFx


SETLOCAL ENABLEEXTENSIONS

:: root is the folder containing this script (without trailing backslash)
set root=%~dp0
set root=%root:~0,-1%

SET sqlPackage="C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\Extensions\Microsoft\SQLDB\DAC\130\SqlPackage.exe"
SET sqlLocalDb="C:\Program Files\Microsoft SQL Server\130\Tools\Binn\SqlLocalDB.exe"


::Sql instance
SET sqlInstanceName="Integration-Test-Instance"

::Path to the dacpac
SET sourcePath=".\Launchpad.Database\bin\Debug\Launchpad.Database.dacpac"

::Connection string for the target
SET connectionString="Integrated Security=SSPI;Persist Security Info=False;Data Source=(localdb)\Integration-Test-Instance;Initial Catalog=Launchpad"

::Stop and recreate the database
sqlLocalDb stop %sqlInstanceName%

sqlLocalDb delete %sqlInstanceName%

sqlLocalDb create %sqlInstanceName% -s

::Publish the database 
%sqlPackage% /TargetConnectionString:%connectionString% /SourceFile:%sourcePath% /Action:Publish