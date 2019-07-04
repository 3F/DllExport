@echo off

set reltype=%1
set cimdll=packages\vsSBE.CI.MSBuild\bin\CI.MSBuild.dll
set _msbuild=tools\hMSBuild

if "%reltype%"=="" (
    set reltype=Release
)

:found

call packages_restore.cmd || goto err

call %_msbuild% -notamd64 "DllExport.sln" /v:m /l:"%cimdll%" /m:4 /t:Build /p:Configuration="%reltype%" || goto err

exit /B 0

:err

echo. Build failed. 1>&2
exit /B 1
