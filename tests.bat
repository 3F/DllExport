@echo off

set cfg=%~1
if not defined cfg set cfg=Release

setlocal
    if exist "DllExport.bat" (

        cd tests

    ) else if exist "bin\%cfg%\raw\" (

        cd bin\%cfg%\raw\tests

    ) else goto buildError

    set "rdir=..\"
    set "tgntPath=GetNuTool\"

    call _run %rdir% DllExport -hMSBuild %tgntPath%
endlocal
exit /B 0

:buildError
    echo. Tests cannot be started for '%cfg%' configuration. Use `%~nx0 ^<config^>` or check your build first. >&2
exit /B 1