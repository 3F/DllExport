@echo off

REM :: To restore packages via GetNuTool
REM :: https://github.com/NuGet/Home/issues/1521

if NOT [%1] == [] (
    set msbuild=%1
    goto found
)

set bat=%~nx0
echo Note: To change MSBuild Tools, use: `%bat:~0,-4% "full_path_to_msbuild.exe"`

set msbuild=tools\msbuild

:found

echo. 
echo. [ Restoring packages. Please wait ... ]
echo. 

echo handler: %msbuild%

%msbuild% gnt.core /p:ngconfig="packages.config|NSBin/packages.config|RGiesecke.DllExport/packages.config|RGiesecke.DllExport.MSBuild/packages.config" /nologo /verbosity:m

:exit