@echo off

:: Need to install - https://msdn.microsoft.com/en-us/library/mt204009.aspx
:: DacFx


SETLOCAL ENABLEEXTENSIONS

:: root is the folder containing this script (without trailing backslash)
set root=%~dp0
set root=%root:~0,-1%

::Path to the SqlPackage.exe
SET toolPath="C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\Extensions\Microsoft\SQLDB\DAC\130\SqlPackage.exe"

::Path to the dacpac
SET sourcePath=".\Launchpad.Database\bin\Debug\Launchpad.Database.dacpac"

::Connection string for the target
SET connectionString="Integrated Security=SSPI;Persist Security Info=False;Data Source=localhost;Initial Catalog=lp"

::Generate the default file name
::https://blogs.msdn.microsoft.com/myocom/2005/06/03/creating-unique-filenames-in-a-batch-file/
for /f "delims=/ tokens=1-3" %%a in ("%DATE:~4%") do (
    for /f "delims=:. tokens=1-4" %%m in ("%TIME: =0%") do (
        set outputPath=Launchpad.Database-%%c-%%b-%%a-%%m%%n%%o%%p.sql
    )
)

:: process command line
if not [%1]==[] if not [%1]==[-] set toolPath=%1
if not [%2]==[] if not [%2]==[-] set sourcePath=%2
if not [%3]==[] if not [%3]==[-] set connectionString=%3
if not [%4]==[] if not [%4]==[-] set outputPath=%4


:: report configuration
echo connectionString: %connectionString%
echo sourcePath: %sourcePath%
echo toolPath: %toolPath%
echo outputPath: %outputPath%

%toolPath% /TargetConnectionString:%connectionString% /SourceFile:%sourcePath% /Action:Script /OutputPath:%outputPath%
