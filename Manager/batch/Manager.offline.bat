@echo off

:: Copyright (c) 2016-2020  Denis Kuzmin [ x-3F@outlook.com ] GitHub/3F
:: Licensed under the MIT license
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
if not defined args set args=%defaultCommand%

call ".\\%pkgsDir%\\%unpackedPkg%\\%mgrFile%" -packages %pkgsDir% -dxp-version actual !args!
exit /B %ERRORLEVEL%
