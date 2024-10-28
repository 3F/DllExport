@echo off
:: Copyright (c) 2016  Denis Kuzmin <x-3F@outlook.com> github/3F
:: Part of https://github.com/3F/DllExport

if "%~1"=="" echo Empty function name & exit /B 1
if exist hMSBuild\a.bat ( set "base1=hMSBuild\a" ) else if exist ..\hMSBuild\a.bat ( set "base1=..\hMSBuild\a" ) else if exist ..\hMSBuild\tests\a.bat ( set "base1=..\hMSBuild\tests\a" ) else (
    echo hMSBuild's functions are not found & exit /B 1
)
if not defined G_LevelChild set /a G_LevelChild=2
    if "%~1"=="tryThisOrBase" ( call %base1% shiftArgs 4,99 shArgs %* ) else set shArgs=%*

    :: TODO: (performance) reduce the number of interruptions
    call :!shArgs! 2>%~nx0.err
    call %base1% tryThisOrBase !ERRORLEVEL! %~nx0.err !shArgs!
exit /B !ERRORLEVEL!

:: = = = = = = = = = = = = = = =

:rsrvDXP1
exit /B 0

:rsrvDXP2
exit /B 1
