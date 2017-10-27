@echo off & echo Incomplete script. Compile it first via 'build.bat' - github.com/3F/DllExport 1>&2 & exit /B 1

:: Copyright (c) 2016-2017  Denis Kuzmin [ entry.reg@gmail.com ] :: github.com/3F
:: Distributed under the DllExport project:
:: https://github.com/3F/DllExport
:: ---
:: Based on hMSBuild logic and includes GetNuTool core.
:: https://github.com/3F/hMSBuild
:: https://github.com/3F/GetNuTool

setlocal enableDelayedExpansion

:: - - -
:: Settings by default

set "dxpVersion=$-pkg-version-$"
set "wAction="

:: -

set "dxpName=DllExport"
set "tWizard=tools/net.r_eg.DllExport.Wizard.targets"
set "dxpPackages=packages"
set "pkgSrv=https://www.nuget.org/api/v2/package/"
set "buildInfoFile=build_info.txt"
set "wRootPath=%cd%"

:: -

set /a dxpDebug=0
set /a buildInfo=0
set "gMsbPath="
set "pkgLink="
set "peExpList="

set ERROR_SUCCESS=0
set ERROR_FILE_NOT_FOUND=2
set ERROR_PATH_NOT_FOUND=3

set "args=%* "


:: - - -
:: Help command

set _hl=%args:"=%
set _hr=%_hl%

set _hr=%_hr:-help =%
set _hr=%_hr:-h =%
set _hr=%_hr:-? =%

if not "%_hl%"=="%_hr%" goto usage
goto commands

:usage

echo.
@echo DllExport - $-version-$
@echo Copyright (c) 2009-2015  Robert Giesecke
@echo Copyright (c) 2016-2017  Denis Kuzmin [ entry.reg@gmail.com :: github.com/3F ]
echo.
echo Distributed under the MIT license
@echo https://github.com/3F/DllExport
echo Wizard - based on hMSBuild logic and includes GetNuTool core - https://github.com/3F
echo.
@echo.
@echo Usage: DllExport [args to DllExport] [args to GetNuTool core]
echo ------
echo.
echo Arguments:
echo ----------
echo  -action {type}        - Specified action for Wizard. Where {type}:
echo                           * Configure - To configure DllExport for specific projects.
echo                           * Update    - To update pkg reference for already configured projects.
echo                           * Restore   - To restore configured DllExport.
echo.
echo  -sln-dir {path}       - Path to directory with .sln files to be processed.
echo  -sln-file {path}      - Optional predefined .sln file to process via the restore operations etc.
echo  -metalib {path}       - Relative path from PkgPath to DllExport meta library.
echo  -dxp-target {path}    - Relative path to .target file of the DllExport.
echo  -dxp-version {num}    - Specific version of DllExport. Where {num}:
echo                           * Versions: 1.6.0 ...
echo                           * Keywords: 
echo                             `actual` to use unspecified local version or to get latest available;
echo.
echo  -msb {path}           - Full path to specific msbuild.
echo  -packages {path}      - A common directory for packages.
echo  -server {url}         - Url for searching remote packages.
echo  -pkg-link {uri}       - Direct link to package from the source via specified URI.
echo  -wz-target {path}     - Relative path to .target file of the Wizard.
echo  -pe-exp-list {module} - To list all available exports from PE32/PE32+ module.
echo  -eng                  - Try to use english language for all build messages.
echo  -GetNuTool {args}     - Access to GetNuTool core. https://github.com/3F/GetNuTool
echo  -debug                - To show additional information.
echo  -version              - Displays version for which (together with) it was compiled.
echo  -build-info           - Displays actual build information from selected DllExport.
echo  -help                 - Displays this help. Aliases: -help -h -?
echo.
echo. 
echo -------- 
echo Samples:
echo -------- 
echo  DllExport -action Configure
echo  DllExport -action Restore -sln-file "Conari.sln"
echo.
echo  DllExport -build-info
echo  DllExport -restore -sln-dir -sln-dir ..\ -debug
echo.
echo  DllExport -GetNuTool -unpack
echo  DllExport -GetNuTool /p:ngpackages="Conari;regXwild"
echo  DllExport -pe-exp-list bin\Debug\regXwild.dll

exit /B 0

:: - - -
:: Handler of user commands

:commands

call :isEmptyOrWhitespace args _is
if [!_is!]==[1] (
    if defined wAction goto action
    goto usage
)

set /a idx=1 & set cmdMax=17
:loopargs

    if "!args:~0,8!"=="-action " (
        call :popars %1 & shift
        set "wAction=%2"
        call :popars %2 & shift
    )

    if "!args:~0,9!"=="-sln-dir " (
        call :popars %1 & shift
        set wSlnDir=%2
        call :popars %2 & shift
    )

    if "!args:~0,10!"=="-sln-file " (
        call :popars %1 & shift
        set wSlnFile=%2
        call :popars %2 & shift
    )

    if "!args:~0,9!"=="-metalib " (
        call :popars %1 & shift
        set wMetaLib=%2
        call :popars %2 & shift
    )

    if "!args:~0,12!"=="-dxp-target " (
        call :popars %1 & shift
        set wDxpTarget=%2
        call :popars %2 & shift
    )

    if "!args:~0,13!"=="-dxp-version " (
        call :popars %1 & shift
        set dxpVersion=%2
        echo Selected new DllExport version: !dxpVersion!
        call :popars %2 & shift
    )

    if "!args:~0,5!"=="-msb " (
        call :popars %1 & shift
        set gMsbPath=%2
        call :popars %2 & shift
    )

    if "!args:~0,10!"=="-packages " (
        call :popars %1 & shift
        set dxpPackages=%2
        call :popars %2 & shift
    )

    if "!args:~0,8!"=="-server " (
        call :popars %1 & shift
        set pkgSrv=%2
        call :popars %2 & shift
    )

    if "!args:~0,10!"=="-pkg-link " (
        call :popars %1 & shift
        set pkgLink=%2
        call :popars %2 & shift
    )

    if "!args:~0,11!"=="-wz-target " (
        call :popars %1 & shift
        set tWizard=%2
        call :popars %2 & shift
    )

    if "!args:~0,13!"=="-pe-exp-list " (
        call :popars %1 & shift
        set peExpList=%2
        call :popars %2 & shift
    )

    if "!args:~0,5!"=="-eng " (
        call :popars %1 & shift
        chcp 437 >nul
    )

    if "!args:~0,11!"=="-GetNuTool " (
        call :popars %1 & shift
        goto gntcall
    )

    if "!args:~0,7!"=="-debug " (
        call :popars %1 & shift
        set /a dxpDebug=1
    )
    
    if "!args:~0,9!"=="-version " (
        @echo $-version-$
        exit /B 0
    )

    if "!args:~0,12!"=="-build-info " (
        call :popars %1 & shift
        set /a buildInfo=1
    )
    
set /a "idx+=1"
if !idx! LSS %cmdMax% goto loopargs

goto action

:popars
set args=!!args:%1 ^=!!
call :trim args
set "args=!args! "
exit /B 0

:: - - -
:: Main logic
:action

call :dbgprint "dxpName = '%dxpName%'"
call :dbgprint "dxpVersion = '!dxpVersion!'"

if defined dxpVersion (
    if "!dxpVersion!"=="actual" (
        set "dxpVersion="
    )
)

call :trim dxpPackages
set "dxpPackages=!dxpPackages!\\"

set "_remoteUrl=!dxpName!"
set "wPkgPath=!dxpPackages!!dxpName!"

if defined dxpVersion (
    set "_remoteUrl=!_remoteUrl!/!dxpVersion!"
    set "wPkgPath=!wPkgPath!.!dxpVersion!"
)

if defined peExpList (
    !wPkgPath!\\tools\\PeViewer.exe -list -pemodule "!peExpList!"
    exit /B %ERRORLEVEL%
)

set dxpTarget="!wPkgPath!\\!tWizard!"
call :dbgprint "dxpTarget = '!dxpTarget!'"

if not exist !dxpTarget! (
    if exist "!wPkgPath!" (
        call :dbgprint "Wizard was not found. Trying to replace obsolete version '!wPkgPath!' ..."
        rmdir /S/Q "!wPkgPath!"
    )

    call :dbgprint "-pkg-link = '!pkgLink!'"
    call :dbgprint "-server = '!pkgSrv!'"

    :: TODO: hack for GNT v1.6.1
    if defined pkgLink (
        set pkgSrv=!pkgLink!
        set "_remoteUrl=:../!wPkgPath!"
    )

    call :dbgprint "_remoteUrl = '!_remoteUrl!'"
    call :dbgprint "ngpath = '!dxpPackages!'"

    set _gntC=/p:ngserver="!pkgSrv!" /p:ngpackages="!_remoteUrl!" /p:ngpath="!dxpPackages!"

    if "!dxpDebug!"=="1" (
        call :gntpoint !_gntC!
    ) else (
        call :gntpoint !_gntC! >nul
    )
)

if "!buildInfo!"=="1" (
    call :dbgprint "buildInfo = '!wPkgPath!\\!buildInfoFile!'"
    if not exist "!wPkgPath!\\!buildInfoFile!" (
        echo information about build is not available.
        exit /B %ERROR_FILE_NOT_FOUND%
    )
    type "!wPkgPath!\\!buildInfoFile!"
    exit /B 0
)

if not exist !dxpTarget! (
    echo Something went wrong. Try to use another keys.
    exit /B %ERROR_FILE_NOT_FOUND%
)

call :dbgprint "-sln-dir = '!wSlnDir!'"
call :dbgprint "-sln-file = '!wSlnFile!'"
call :dbgprint "-metalib = '!wMetaLib!'"
call :dbgprint "-dxp-target = '!wDxpTarget!'"
call :dbgprint "wRootPath = !wRootPath!"
call :dbgprint "wAction = !wAction!"

if defined gMsbPath (
    call :dbgprint "Use specific MSBuild tools '!gMsbPath!'"
    set msbuildPath=!gMsbPath!
    goto rundxp
)

call :msbnetf
if "!ERRORLEVEL!"=="0" goto rundxp


echo MSBuild tools was not found. Try with `-msb` key. `-help` for details.
exit /B %ERROR_FILE_NOT_FOUND%

:rundxp

call :isEmptyOrWhitespace msbuildPath _is
if [!_is!]==[1] (
    echo Something went wrong. Use `-debug` key for details.
    exit /B %ERROR_FILE_NOT_FOUND%
)

set xMSBuild="!msbuildPath!"

call :dbgprint "Target: !xMSBuild! !dxpTarget!"

!xMSBuild! /nologo /v:m /m:4 !dxpTarget!
exit /B 0

:: - - -
:: Tools from .NET Framework - .net 4.0, ...
:msbnetf

call :dbgprint "trying via MSBuild tools from .NET Framework - .net 4.0, ..."

for %%v in (4.0, 3.5, 2.0) do (
    call :rtools %%v Y & if [!Y!]==[1] exit /B 0
)
call :dbgprint "msbnetf: unfortunately we didn't find anything."
exit /B %ERROR_FILE_NOT_FOUND%

:rtools
call :dbgprint "checking of version: %1"
    
for /F "usebackq tokens=2* skip=2" %%a in (
    `reg query "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSBuild\ToolsVersions\%1" /v MSBuildToolsPath 2^> nul`
) do if exist %%b (
    call :dbgprint "found: %%b"
        
    set msbuildPath=%%b
    call :msbuildfind
    set /a %2=1
    exit /B 0
)
set /a %2=0
exit /B 0

:gntcall
call :dbgprint "direct access to GetNuTool..."
call :gntpoint !args!
exit /B 0

:msbuildfind

set msbuildPath=!msbuildPath!\MSBuild.exe
exit /B 0

:: =

:dbgprint
if "!dxpDebug!"=="1" (
    set msgfmt=%1
    set msgfmt=!msgfmt:~0,-1! 
    set msgfmt=!msgfmt:~1!
    echo.[%TIME% ] !msgfmt!
)
exit /B 0

:trim
:: Usage: call :trim variable
call :_v %%%1%%
set %1=%_trimv%
exit /B 0
:_v
set "_trimv=%*"
exit /B 0

:isEmptyOrWhitespace
:: Usage: call :isEmptyOrWhitespace input output(1/0)
setlocal enableDelayedExpansion
set "_v=!%1!"

if not defined _v endlocal & set /a %2=1 & exit /B 0
 
set _v=%_v: =%
set "_v= %_v%"
if [^%_v:~1,1%]==[] endlocal & set /a %2=1 & exit /B 0
 
endlocal & set /a %2=0
exit /B 0

:gntpoint
setlocal disableDelayedExpansion 

:: ========================= GetNuTool =========================

