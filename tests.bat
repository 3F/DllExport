@echo off

if "%~1"=="PublicRelease" ( set "config=Release" ) else set config=%~1
setlocal
    if exist "tests\a.bat" (

        cd tests

    ) else if not defined config (

        echo.&echo %~n0 Debug&echo %~n0 Release
        exit /B 0

    ) else if exist "bin\%config%\raw\" (

        cd bin\%config%\raw\tests

    ) else goto buildError

    set "rdir=..\"
    set "tgntPath=GetNuTool\"

    call _run %rdir% DllExport -hMSBuild %tgntPath%
endlocal
exit /B 0

:buildError
    echo. Tests cannot be started for '%config%' configuration. Use `%~nx0 ^<config^>` or check your build first. >&2
exit /B 1