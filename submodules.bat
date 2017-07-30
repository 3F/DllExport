@echo off

echo Checking submodules ...

set _dep=%1

if "%_dep%"=="" (
    echo Incorrect command. Please use `build.bat` instead of this.
    exit /B 0
)


if not exist "%_dep%" goto restore
exit /B 0

:restore

echo.
echo. Whoops, you need to update git submodules.
echo. But we'll update this automatically.
echo.
echo. Please wait...
echo.

:: GetNuTool
git submodule update --init --recursive GetNuTool || goto gitNotFound

:: TODO option for expensive coreclr

exit /B 0

:gitNotFound

if not exist ".git" (
    echo.  1>&2
    echo To restore submodules via Git scm you should have a `.git` folder, but we can't find this. 1>&2
    echo Unfortunately you should get this manually, or try to clone initially with recursive option: `git clone --recursive ...` 1>&2
    exit /B 3
)

echo.  1>&2
echo. `git` was not found or something went wrong. Check your connection and env. variable `PATH`. Or get submodules manually: 1>&2
echo.     1. Use command `git submodule update --init --recursive` 1>&2
echo.     2. Or clone initially with recursive option: `git clone --recursive ...` 1>&2
echo.  1>&2

exit /B 2