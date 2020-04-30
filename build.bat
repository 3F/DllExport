@echo off

set cim=packages\vsSolutionBuildEvent\cim.cmd -vsw-priority Microsoft.NetCore.Component.SDK
set _gnt=tools/gnt

set reltype=%~1
if not defined reltype (
    set reltype=Release
)

call %_gnt% /p:wpath="%cd%" /p:ngconfig="packages.config" /nologo /v:m /m:4 || goto err
call %cim% "DllExport.sln" /v:m /m:4 /p:Configuration="%reltype%" || goto err

goto exit

:err

echo. Build failed. 1>&2
exit /B 1

:exit
exit /B 0