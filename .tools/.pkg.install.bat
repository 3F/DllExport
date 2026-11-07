::! https://github.com/3F/DllExport
@echo off

:: Arguments: https://github.com/3F/GetNuTool?tab=readme-ov-file#touch--install--run
:: 1 tmode "path to the working directory" "path to the package"
if "%~1" LSS "1" echo Version "%~1" is not supported.>&2 & exit /B1
set "tmode=%~2"
set "wdir=%~3"
set "pdir=%~4"

if not exist "%pdir%\DllExport.bat.sha1" exit /B1

set "urlDoc=https://github.com/3F/DllExport/wiki/Quick-start"
set dxp="%pdir%\DllExport.bat"
xcopy %dxp% "%wdir%." /Y/R/V/I/Q 2>nul>nul || copy /Y/V %dxp% "%wdir%" 2>nul>nul || exit /B1

if "%use%"=="validator" (
    call "%pdir%\validate.DllExport.bat" "%wdir%"

) else if "%use%"=="wiki" (
    start "" %urlDoc%

) else if "%use%"=="documentation" (
    start "" %urlDoc%

) else if "%use%"=="help" (
    call "%pdir%\validate.DllExport.bat" "%wdir%"
    start "" %urlDoc%

) else if "%use%"=="-" (
    exit /B0

) else if defined use (
    echo "%use%" is not supported in current .pkg.install>&2
    exit /B1
)

if "%~2"=="touch" exit /B0

set /p _version=<"%pdir%\.version"
set /p _rel=<"%pdir%\.release.version"
if defined _rel set _version=%_version%-%_rel%

set "dst=%pdir%\..\DllExport.%_version%"
if exist "%dst%" rmdir /S/Q "%dst%"

::`ren` must be processed as a single command along with the called program due to line-by-line processing by the cmd processor after loss of src
ren "%pdir%" DllExport.%_version% && if "%~2"=="run" ("%wdir%\DllExport.bat") else echo.exit/B0>"%dst%\.cmd"&"%dst%\.cmd"