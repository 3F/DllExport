@echo off

set reltype=%~1
if not defined reltype set reltype=Release

call tools\gnt & call packages\vsSolutionBuildEvent\cim.cmd ~x ~c %reltype% || goto err
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
