@echo off

:: VS IDE plugin: https://github.com/3F/vsSolutionBuildEvent/releases/latest

call tools\gnt /p:wpath="%cd%" /p:ngconfig="packages.config" /nologo /v:m /m:7 || goto err
call packages\vsSolutionBuildEvent\GUI.bat || goto err

exit /B 0

:err
echo. GUI Failed. Error %ERRORLEVEL%. 1>&2
exit /B %ERRORLEVEL%