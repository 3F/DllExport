@echo off

:: Wrapper. Offline .NET DllExport
:: https://github.com/3F/DllExport
:: - - - - - - - - - - - - - - - -

:: Activated version
set "pkgVersion=actual"

:: When there are no arguments to this wrapper
set "defaultCommand=-action Configure"

:: Path to the received package
set "pkgsDir=packages"










:: - - - - - - - - - - - - - - -
setlocal enableDelayedExpansion

set args=%*
set "__=DllExport"
if not defined args set args=%defaultCommand%
set __dxp_pv=/Offline

if "%pkgVersion%"=="actual" (
    ".\\%pkgsDir%\\%__%\\%__%" -packages %pkgsDir% -dxp-version actual !args!
) else (
    ".\\%pkgsDir%\\%__%.%pkgVersion%\\%__%" -packages %pkgsDir% !args!
)
