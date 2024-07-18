@echo off
:: Copyright (c) 2016  Denis Kuzmin <x-3F@outlook.com> github/3F
:: Tests. Part of https://github.com/3F/DllExport
:: Based on https://github.com/3F/hMSBuild

setlocal enableDelayedExpansion

:: path to the directory where the release is located
set "rdir=%~1"

:: path to core
set "core=%~2"

:: path to hMSBuild
set "hmsb=%~3"

:: path to GetNuTool tests
set "tgnt=%~4"

call a isNotEmptyOrWhitespaceOrFail core || exit /B1
call a isNotEmptyOrWhitespaceOrFail rdir || exit /B1
call a isNotEmptyOrWhitespaceOrFail hmsb || exit /B1
call a isNotEmptyOrWhitespaceOrFail tgnt || exit /B1

call a initAppVersion Dxp

echo.
call a cprint 0E  ----------------------
call a cprint F0  "DllExport .bat testing"
call a cprint 0E  ----------------------
echo.

if "!gcount!" LSS "1" set /a gcount=0
if "!failedTotal!" LSS "1" set /a failedTotal=0

:::::::::::::::::: :::::::::::::: :::::::::::::::::::::::::
:: Tests


    echo. & call a print "Tests - 'ManagerKeyTests'"
    call .\ManagerKeyTests gcount failedTotal "%core%" "%rdir%"

    ::

    set "core=%core% %hmsb%"
    call a disableAppVersion Hms

    echo. & call a print "Tests - '-hMSBuild VswasTests'"
    call .\VswasTests gcount failedTotal "%core%" "%rdir%"

    echo. & call a print "Tests - '-hMSBuild VswStreamTests'"
    call .\VswStreamTests gcount failedTotal "%core%" "%rdir%"

    echo. & call a print "Tests - '-hMSBuild DiffVTests'"
    call .\DiffVTests gcount failedTotal "%rdir%%core%" "%rdir%" dbghcore

    echo. & call a print "Tests - '-hMSBuild DiffVswStreamTests'"
    call .\DiffVswStreamTests gcount failedTotal "dbgdcore %hmsb%" "%rdir%" dbghcore

    echo. & call a print "Tests - '-hMSBuild keysAndLogicTests'"
    call .\keysAndLogicTests gcount failedTotal "%core%" "%rdir%"

    ::

    call a disableAppVersion Gnt
    echo. & call a print "Tests - '-hMSBuild -GetNuTool keysAndLogicTests'"
    call %tgnt%keysAndLogicTests gcount failedTotal "%rdir%%core% -GetNuTool " ""


::::::::::::::::::
::
echo.
call a cprint 0E ----------------
echo  [Failed] = !failedTotal!
set /a "gcount-=failedTotal"
echo  [Passed] = !gcount!
call a cprint 0E ----------------
echo.

if !failedTotal! GTR 0 goto failed
echo.
call a cprint 0A "All Passed."
exit /B 0

:failed
    echo.
    call a cprint 0C "Tests failed." >&2
exit /B 1
