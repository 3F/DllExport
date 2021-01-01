@echo off

set cim=packages\vsSolutionBuildEvent\cim.cmd

set reltype=%~1
if not defined reltype set reltype=Release

call tools/gnt /p:wpath="%cd%" /p:ngconfig="packages.config" /nologo /v:m /m:6 || goto err
call %cim% /v:m /m:4 /p:Configuration="%reltype%" || goto err

exit /B 0

:err
echo. Build failed. 1>&2
exit /B 1