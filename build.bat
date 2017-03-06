@echo off

if NOT [%1] == [] (
    set msbuild=%1
    goto found
)

:: set bat=%~nx0
:: echo Note: To change MSBuild Tools, use: `%bat:~0,-4% "full_path_to_msbuild.exe"`

set msbuild=tools\msbuild

:found

call packages_restore.cmd -msbuild %msbuild%

%msbuild% "DllExport.sln" /v:normal /l:"packages\vsSBE.CI.MSBuild\bin\CI.MSBuild.dll" /m:4 /t:Build /p:Configuration=Release

:exit