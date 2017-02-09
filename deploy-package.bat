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
SET sourcePath=".\Voyage.Database\bin\Debug\Voyage.Database.dacpac"

::Connection string for the target
SET connectionString="Integrated Security=SSPI;Persist Security Info=False;Data Source=localhost;Initial Catalog=Voyage"

:: process command line
if not [%1]==[] if not [%1]==[-] set toolPath=%1
if not [%2]==[] if not [%2]==[-] set sourcePath=%2
if not [%3]==[] if not [%3]==[-] set connectionString=%3


:: report configuration
echo connectionString:   %connectionString%
echo sourcePath: %sourcePath%
echo toolPath: %toolPath%

%toolPath% /TargetConnectionString:%connectionString% /SourceFile:%sourcePath% /Action:Publish

