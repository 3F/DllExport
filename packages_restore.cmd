@echo off

:: To restore packages via GetNuTool
:: https://github.com/NuGet/Home/issues/1521

set _gnt=tools/gnt
set _gntArgs=%*


:: set bat=%~nx0
:: echo Note: To change MSBuild Tools, use: `%bat:~0,-4% -msbuild "full_path_to_msbuild.exe"`

:found

echo. 
echo. [ Restoring of packages. Please wait ... ]
echo. 


call %_gnt% %_gntArgs% /p:wpath="%cd%" /p:ngconfig="packages.config;Wizard/packages.config;PeViewer/packages.config;NSBin/packages.config;RGiesecke.DllExport/packages.config;RGiesecke.DllExport.MSBuild/packages.config" /nologo /v:m /m:4 || goto err

goto exit

:err

echo. failed. 1>&2
exit /B 1

:exit
exit /B 0