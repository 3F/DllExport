:: build [[configuration] | [# [option [keys]]]
:: https://github.com/3F/DllExport
@echo off & if "%~1"=="#" (
    if "%~2"=="coreclr-ilasm" ( if "%~3"=="" echo -x64& echo -x86& echo -all -x86 -x64& exit /B0
        if not exist src\coreclr\.gitignore call git submodule update --init --recursive coreclr
        setlocal & cd src\coreclr & build-s %3 %4 %5 %6 -release

    ) else if /I "%~2"=="CI" (
        shift & shift & setlocal
            cd .tools & call netfx4sdk -mode sys || call netfx4sdk -mode pkg
        endlocal

    ) else if "%~2"=="" ( call .tools\hMSBuild ~x -GetNuTool & exit /B0 ) else goto err
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
