@echo off

for %%v in (14.0, 12.0, 15.0, 4.0, 3.5, 2.0) do (
    for /F "usebackq tokens=2* skip=2" %%a in (
        `reg query "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSBuild\ToolsVersions\%%v" /v MSBuildToolsPath 2^> nul`
    ) do if exist %%b (
        set msbuild="%%bmsbuild.exe"
        goto found
    )
)

echo MSBuild Tools was not found. Please use it manually like: ` "full_path_to_msbuild.exe" arguments ` 1>&2

goto exit

:found

echo MSBuild Tools: %msbuild% 

%msbuild% %*


:exit