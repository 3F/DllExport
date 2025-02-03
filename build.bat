::! https://github.com/3F/DllExport
@echo off & if "%~1"=="#" (
    if "%~2"=="ilasm" ( if "%~3"=="" echo -x64& echo -x86& exit /B0
        if not exist src\coreclr\.gitignore call git submodule update --init src/coreclr
        if "%~3"=="-x86" set "_dxpILAsmBinX=86"
        setlocal & cd src\coreclr & build.ilasm %3 -release

    ) else if /I "%~2"=="CI" (
        shift & shift & setlocal
            cd .tools & call netfx4sdk -mode sys || call netfx4sdk -mode pkg
        endlocal
    ) else if "%~2"=="" ( call .tools\hMSBuild ~x -GetNuTool & exit /B0 ) else goto err

) else if "%~1"=="" (
    echo.&echo %~n0 {configuration} ^| # [option and keys]
    echo.&echo %~n0 Release &echo %~n0 # &echo %~n0 # CI Release &echo %~n0 # ilasm -x64
    exit /B 0
)

set reltype=%~1
if not defined reltype set reltype=Release

call .tools\gnt & call packages\vsSolutionBuildEvent\cim.cmd ~x ~c %reltype% DllExport.sln || goto err
set "frel=%reltype:Public=%"

setlocal enableDelayedExpansion
    cd bin\%frel%\raw\tests
    call a initAppVersion Dxp
    call a execute "..\DllExport -h" & call a msgOrFailAt 1 "DllExport %appversionDxp%" || goto err
    call a printMsgAt 1 3F "Completed as a "
endlocal
exit /B 0

:err
    echo Failed build>&2
exit /B 1
