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
if not [%1]==[] if not [%1]==[-] set sourcePath=%1

::Connection string for the target
SET connectionString="Integrated Security=SSPI;Persist Security Info=False;Data Source=(localdb)\Integration-Test-Instance;Initial Catalog=Launchpad"

::Clean up mdf Files
SET dataPath="%UserProfile%\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\Integration-Test-Instance"

::Stop the localdb instance
sqlLocalDb stop %sqlInstanceName%

::Delete the localdb instance
sqlLocalDb delete %sqlInstanceName%

::Delete the old database files
@echo on
DEL /Q %dataPath%
@echo off

::Create the database
sqlLocalDb create %sqlInstanceName% -s

::Publish the database 
%sqlPackage% /TargetConnectionString:%connectionString% /SourceFile:%sourcePath% /Action:Publish