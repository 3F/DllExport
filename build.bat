@echo off

set cimdll=packages\vsSBE.CI.MSBuild\bin\CI.MSBuild.dll
set _msbuild=tools\hMSBuild

:found

call packages_restore.cmd || goto err

%_msbuild% "DllExport.sln" /v:normal /l:"%cimdll%" /m:4 /t:Build /p:Configuration=Release || goto err
exit /B 0

:err

echo. Build failed. 1>&2
exit /B 1
