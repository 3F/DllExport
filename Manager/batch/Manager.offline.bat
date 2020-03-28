@echo off

:: https://github.com/3F/DllExport
:: ---
:: Offline version - wrapper

:: - :: - - - - - - - - - - - - - - -

:: Activated version to use
set "pkgVersion=actual"

:: Use command when no arguments to this wrapper
set "defaultCommand=-action Configure"

:: Where offline packages are stored
set "pkgsDir=packages"










:: - - - - - - - - - - - - - - -
setlocal enableDelayedExpansion

set args=%*
set "__=DllExport"
if not defined args set args=%defaultCommand%
set __dxp_pv=/Offline

if "%pkgVersion%"=="actual" (
    call ".\\%pkgsDir%\\%__%\\%__%" -packages %pkgsDir% -dxp-version actual !args!
) else (
    call ".\\%pkgsDir%\\%__%.%pkgVersion%\\%__%" -packages %pkgsDir% !args!
)

exit /B %ERRORLEVEL%
