@echo off

set cfg=%~1
if not defined cfg set cfg=Release

set tdir=.\\bin\\%cfg%\\tests\\mgr\\
if not exist %tdir% goto buildError

setlocal
    cd %tdir%
    call .\tests
endlocal

exit /B 0

:buildError

echo. Tests cannot be started for '%cfg%' configuration: Use `%~nx0 ^<config^>` or check your build first. 1>&2
exit /B 1