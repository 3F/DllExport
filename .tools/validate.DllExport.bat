::! https://github.com/3F/DllExport
@echo off
:: [1] Optional input directory where DllExport.bat is located.

set "input=%~1"
if not defined input set "input=%~dp0"

set core="%input%DllExport.bat"
if not exist %core% call :notfound %core% & exit /B2

call DllExport -hMSBuild -GetNuTool ~

call :check "%~dp0\DllExport.bat.sha1" %core% LF
echo.
call :check "%~dp0\DllExport.bat.crlf.sha1" %core% CRLF

echo.
if "%~1"=="" pause
exit /B 0

:check
    :: (1) path .sha1
    :: (2) path to core
    :: (2) LF/CRLF type
    if not exist %1 call :notfound %1 & exit /B2
    set /p _sha1=<%1

        echo %~2 ?= %_sha1% (%~3)
        call svc.gnt -sha1-cmp %2 %_sha1% -package-as-path
exit /B 0

:notfound
    echo %1 is not found>&2
exit /B 0