@echo off

:: Copyright (c) 2016-2019  Denis Kuzmin [ entry.reg@gmail.com ] GitHub/3F
:: Distributed under the DllExport project:
:: https://github.com/3F/DllExport
:: ---
:: Offline version - wrapper

:: - :: - - - - - - - - - - - - - - -

:: the command when no arguments at all
set "defaultCommand=-action Configure"

:: common directory with offline packages
set "pkgsDir=packages.offline"

:: DllExport package
set "unpackedPkg=DllExport"

:: DllExport Manager
set "mgrFile=DllExport.bat"


:: - - - - - - - - - - - - - - -
setlocal enableDelayedExpansion

set args=%*

call :isEmptyOrWhitespace args _is
if [!_is!]==[1] (
    set args=%defaultCommand%
)

call .\%pkgsDir%\%unpackedPkg%\%mgrFile% -packages %pkgsDir% -dxp-version actual !args!

exit /B %ERRORLEVEL%



:isEmptyOrWhitespace
:: Usage: call :isEmptyOrWhitespace input output(1/0)
setlocal enableDelayedExpansion
set "_v=!%1!"

if not defined _v endlocal & set /a %2=1 & exit /B 0
 
set _v=%_v: =%
set "_v= %_v%"
if [^%_v:~1,1%]==[] endlocal & set /a %2=1 & exit /B 0
 
endlocal & set /a %2=0
exit /B 0