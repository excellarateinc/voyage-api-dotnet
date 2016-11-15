::Borrowed and butchered from:
::https://gist.github.com/crankery/b6105d2283cd0eadf85d
::http://thecache.trov.com/integrating-xunit-v2-with-jenkins/

@echo off

::Delayed Expansion will cause variables to be expanded at execution time rather than at parse time, 
::this option is turned on with the SETLOCAL command. When delayed expansion is in effect variables 
::can be referenced using !variable_name! (in addition to the normal %variable_name% )
::http://ss64.com/nt/delayedexpansion.html
setlocal EnableDelayedExpansion 

:: root is the folder containing this script (without trailing backslash)
set root=%~dp0
set root=%root:~0,-1%

:: put xunit binaries into a folder without versioning in the name
set bin=%root%\xunit


:: set defaults
set resultCode=0
set outputPath=^"%root%\xunit-results.xml^"
set failOnError=1

:: process command line
if not [%1]==[] if not [%1]==[-] set outputPath=%1
if not [%2]==[] if not [%2]==[-] set configuration=%2
if not [%3]==[] if not [%3]==[-] set failOnError=%3

:: report configuration
echo output-path:   %outputPath%
echo configuration: %configuration%
echo fail-on-error: %failOnError%


:: clear out old bin path
echo "%bin%"
if exist "%bin%" rmdir "%bin%" /s /q
mkdir "%bin%"

:: Copy the current xunit console runner to the bin folder
for /f "tokens=*" %%a in ('dir /b /s /a:d "%root%\packages\xunit.runner.console.*"') do (
  copy "%%a\tools\*" "%bin%" >NUL
)

:: Copy the current xunit exeuction library for .net 4.5 to the bin folder
for /f "tokens=*" %%a in ('dir /b /s /a:d "%root%\packages\xunit.extensibility.execution.*"') do (
  copy "%%a\lib\net45\*" "%bin%" >NUL
)

:: Discover test projects
echo Discovering assemblies
set testAssemblies=
for /f "tokens=*" %%a in ('dir /b /s "%root%\publish\*.UnitTests.dll"') do (
  echo "Found: %%a"
  set testAssembly=^"%%a^"
  
   if [!testAssemblies!]==[] (
    set testAssemblies=!testAssembly!
  ) else (
    set testAssemblies=!testAssemblies! !testAssembly!
  )
)

echo on

"%bin%\xunit.console.exe" %testAssemblies% -xml %outputPath% -parallel all

@echo off
if /i %failOnError% neq 0 (
  set resultCode=%ERRORLEVEL%
)

exit /b %resultCode%
