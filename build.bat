@echo off

if NOT [%1] == [] (
    set msbuild=%1
    goto found
)

for %%v in (14.0, 12.0, 15.0, 4.0, 3.5, 2.0) do (
    for /F "usebackq tokens=2* skip=2" %%a in (
        `reg query "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSBuild\ToolsVersions\%%v" /v MSBuildToolsPath 2^> nul`
    ) do if exist %%b (
        set msbuild="%%bmsbuild.exe"
        goto found
    )
)

set bat=%~nx0
echo MSBuild Tools was not found. Please define it manually like: `%bat:~0,-4% "full_path_to_msbuild.exe"` 1>&2

goto exit

:found

echo MSBuild Tools: %msbuild%

%msbuild% gnt.core /p:ngconfig="packages.config|RGiesecke.DllExport/packages.config|RGiesecke.DllExport.MSBuild/packages.config" /nologo /verbosity:m
%msbuild% "DllExport.sln" /verbosity:normal /l:"packages\vsSBE.CI.MSBuild\bin\CI.MSBuild.dll" /m:8 /t:Build /p:Configuration=Release

:exit