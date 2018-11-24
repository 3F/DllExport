@echo off & echo Incomplete script. Compile it first via 'build.bat' - github.com/3F/DllExport 1>&2 & exit /B 1

:: Copyright (c) 2016-2018  Denis Kuzmin [ entry.reg@gmail.com ] :: github.com/3F
:: Distributed under the DllExport project:
:: https://github.com/3F/DllExport
:: ---
:: Based on hMSBuild logic and includes GetNuTool core.
:: https://github.com/3F/hMSBuild
:: https://github.com/3F/GetNuTool

set "dpnx0=%~dpnx0"
set args=%*
set esc=%args:!=^!%

:: Escaping '^' is not identical for all cases (DllExport ... vs call DllExport ...). 
:: thus, the most stable way just to leave this 'as is' for user.
REM set esc=%args:^=^^%
REM set esc=%esc:!=^!%

:: # call DllExport  ^^ - ^^
:: # call DllExport  ^  - ^
:: # DllExport       ^^ - ^
:: # DllExport       ^  - empty

:: # call DllExport  %%%% - %
:: # call DllExport  %%   - empty
:: # call DllExport  %    - empty
:: # DllExport       %%   - %
:: # DllExport       %    - empty

setlocal enableDelayedExpansion

:: https://github.com/3F/DllExport/issues/88
:: Bugs for '&' symbol when:
:: 1. "_=%*" or _="%*"
:: 2.1. _=%* Works correctly only for delayed evaluation inside "...", i.e. don't use "args=%_% "
:: 2.2. Mostly can be safely called only as !args!, i.e. try do not use %args%
::
:: Bug for '!' symbol when:
:: 1. %~dpnx0 and %cd% in enableDelayedExpansion mode.
:: 1.1. Derivative assignments can be used safely only as !cd!, i.e. a=!cd! ... b=!a! but not as b=%a%
:: !cd! -ok
:: %~dpnx0 via !dpnx0! -ok

:: - - -
:: Settings by default

set "dxpVersion=$-pkg-version-$"
set "wAction="

:: - - - 

set "dxpName=DllExport"
set "tWizard=tools/net.r_eg.DllExport.Wizard.targets"
set "dxpPackages=packages"
set "pkgSrv=https://www.nuget.org/api/v2/package/"
set "buildInfoFile=build_info.txt"
set "fManager=!dpnx0!"
set "wRootPath=!cd!"
set wMgrArgs=!args!

:: -

set "dxpDebug="
set "buildInfo="
set "gMsbPath="
set "pkgLink="
set "peExpList="
set "kForce="
set "mgrUp="
set "proxy="
set "xmgrtest="

set "E_CARET=^"

set /a ERROR_SUCCESS=0
set /a ERROR_FAILED=1
set /a ERROR_FILE_NOT_FOUND=2
set /a ERROR_PATH_NOT_FOUND=3

:: Current exit code for endpoint
set /a EXIT_CODE=0

:: - - -
:: Help command

for %%v in (!esc!) do (
    if [%%v]==[-help] goto usage
    if [%%v]==[-h] goto usage
)
if not defined qmoff if defined args if not z!args!==z!args:-?=! goto usage

goto commands

:usage

echo.
@echo DllExport - $-version-$
@echo Copyright (c) 2009-2015  Robert Giesecke
@echo Copyright (c) 2016-2018  Denis Kuzmin [ entry.reg@gmail.com :: github.com/3F ]
echo.
echo Distributed under the MIT license
@echo https://github.com/3F/DllExport
echo It was based on hMSBuild logic and includes GetNuTool core - https://github.com/3F
echo.
@echo.
@echo Usage: DllExport [args to DllExport] [args to GetNuTool core]
echo ------
echo.
echo Arguments:
echo ----------
echo  -action {type} - Specified action for Wizard. Where {type}:
echo       * Configure - To configure DllExport for specific projects.
echo       * Update    - To update pkg reference for already configured projects.
echo       * Restore   - To restore configured DllExport.
echo       * Export    - To export configured projects data.
echo       * Recover   - To re-configure projects via predefined/exported data.
echo       * Unset     - To unset all data from specified projects.
echo       * Upgrade   - Aggregates an Update action with additions for upgrading.
echo.
echo  -sln-dir {path}    - Path to directory with .sln files to be processed.
echo  -sln-file {path}   - Optional predefined .sln file to be processed.
echo  -metalib {path}    - Relative path from PkgPath to DllExport meta library.
echo  -dxp-target {path} - Relative path to entrypoint wrapper of the main core.
echo  -dxp-version {num} - Specific version of DllExport. Where {num}:
echo       * Versions: 1.6.0 ...
echo       * Keywords: 
echo         `actual` to use unspecified local version or to get latest available;
echo.
echo  -msb {path}           - Full path to specific msbuild.
echo  -packages {path}      - A common directory for packages.
echo  -server {url}         - Url for searching remote packages.
echo  -proxy {cfg}          - To use proxy. The format: [usr[:pwd]@]host[:port]
echo  -pkg-link {uri}       - Direct link to package from the source via specified URI.
echo  -force                - Aggressive behavior, e.g. like removing pkg when updating.
echo  -mgr-up               - Updates this manager to version from '-dxp-version'.
echo  -wz-target {path}     - Relative path to entrypoint wrapper of the main wizard.
echo  -pe-exp-list {module} - To list all available exports from PE32/PE32+ module.
echo  -eng                  - Try to use english language for all build messages.
echo  -GetNuTool {args}     - Access to GetNuTool core. https://github.com/3F/GetNuTool
echo  -debug                - To show additional information.
echo  -version              - Displays version for which (together with) it was compiled.
echo  -build-info           - Displays actual build information from selected DllExport.
echo  -help                 - Displays this help. Aliases: -help -h
echo.
echo. 
echo -------- 
echo Samples:
echo -------- 
echo  DllExport -action Configure
echo  DllExport -action Restore -sln-file "Conari.sln"
echo  DllExport -proxy guest:1234@10.0.2.15:7428 -action Configure
echo  DllExport -action Configure -force -pkg-link http://host/v1.6.1.nupkg
echo.
echo  DllExport -build-info
echo  DllExport -debug -restore -sln-dir ..\ 
echo  DllExport -mgr-up -dxp-version 1.6.1
echo  DllExport -action Upgrade -dxp-version 1.6.1
echo.
echo  DllExport -GetNuTool -unpack
echo  DllExport -GetNuTool /p:ngpackages="Conari;regXwild"
echo  DllExport -pe-exp-list bin\Debug\regXwild.dll

goto endpoint

:: - - -
:: Handler of user commands

:commands

if not defined args (
    if defined wAction goto action
    goto usage
)

set "key=" & set "kset="
for %%v in (!esc!) do (
    
    if not defined kset set key=%%v
    
    if [!key!]==[-action] ( if defined kset (
        
        set wAction=%%v

        rem - -
        set "key=" & set "kset=" ) else set kset=1
    ) else if [!key!]==[-sln-dir] ( if defined kset (
        
        set wSlnDir=%%v

        rem - -
        set "key=" & set "kset=" ) else set kset=1
    ) else if [!key!]==[-sln-file] ( if defined kset (
        
        set wSlnFile=%%v

        rem - -
        set "key=" & set "kset=" ) else set kset=1
    ) else if [!key!]==[-metalib] ( if defined kset (
        
        set wMetaLib=%%v

        rem - -
        set "key=" & set "kset=" ) else set kset=1
    ) else if [!key!]==[-dxp-target] ( if defined kset (
        
        set wDxpTarget=%%v

        rem - -
        set "key=" & set "kset=" ) else set kset=1
    ) else if [!key!]==[-dxp-version] ( if defined kset (
        
        set dxpVersion=%%v

        rem - -
        set "key=" & set "kset=" ) else set kset=1
    ) else if [!key!]==[-msb] ( if defined kset (
        
        set gMsbPath=%%~v

        rem - -
        set "key=" & set "kset=" ) else set kset=1
    ) else if [!key!]==[-packages] ( if defined kset (
        
        rem dequote %%v
        set dxpPackages=%%~v

        rem - -
        set "key=" & set "kset=" ) else set kset=1
    ) else if [!key!]==[-server] ( if defined kset (
        
        set pkgSrv=%%~v

        rem - -
        set "key=" & set "kset=" ) else set kset=1
    ) else if [!key!]==[-proxy] ( if defined kset (
        
        set proxy=%%~v

        rem - -
        set "key=" & set "kset=" ) else set kset=1
    ) else if [!key!]==[-pkg-link] ( if defined kset (
        
        set pkgLink=%%~v

        rem - -
        set "key=" & set "kset=" ) else set kset=1
    ) else if [%%v]==[-force] (

        set kForce=1

    ) else if [%%v]==[-mgr-up] (

        set mgrUp=1

    ) else if [!key!]==[-wz-target] ( if defined kset (
        
        set tWizard=%%~v

        rem - -
        set "key=" & set "kset=" ) else set kset=1
    ) else if [!key!]==[-pe-exp-list] ( if defined kset (
        
        set peExpList=%%~v

        rem - -
        set "key=" & set "kset=" ) else set kset=1
    ) else if [%%v]==[-eng] (

        chcp 437 >nul

    ) else if [%%v]==[-GetNuTool] (

        call :dbgprint "accessing to GetNuTool ..."
        call :gntpoint !args!

        set /a EXIT_CODE=%ERRORLEVEL%
        goto endpoint

    ) else if [%%v]==[-debug] (

        set dxpDebug=1

    ) else if [%%v]==[-version] (

        @echo $-version-$
        goto endpoint

    ) else if [%%v]==[-build-info] (

        set buildInfo=1

    ) else if [!key!]==[-tests] ( if defined kset (
        
        set xmgrtest=%%~v

        rem - -
        set "key=" & set "kset=" ) else set kset=1
    ) else (
        echo Incorrect key: %%v
        set /a EXIT_CODE=%ERROR_FAILED%
        goto endpoint
    )
    
)

:: - - -
:: Main 
:action

call :dbgprint "dxpName = '%dxpName%'"
call :dbgprint "dxpVersion = '%dxpVersion%'"
call :dbgprint "-sln-dir = " wSlnDir
call :dbgprint "-sln-file = " wSlnFile
call :dbgprint "-metalib = " wMetaLib
call :dbgprint "-dxp-target = " wDxpTarget
call :dbgprint "-wz-target = " tWizard

if defined dxpVersion (
    if "!dxpVersion!"=="actual" (
        set "dxpVersion="
    )
)

if z%wAction%==zUpgrade (
    call :dbgprint "Upgrade is on"
    set mgrUp=1
    set kForce=1
)

call :trim dxpPackages
set "dxpPackages=!dxpPackages!\\"

set "_remoteUrl=!dxpName!"
set "wPkgPath=!dxpPackages!!dxpName!"

if defined dxpVersion (
    set "_remoteUrl=!_remoteUrl!/!dxpVersion!"
    set "wPkgPath=!wPkgPath!.!dxpVersion!"
)

if defined kForce (
    if exist "!wPkgPath!" (
        call :dbgprint "Removing old version before continue. '-force' key rule. " wPkgPath
        rmdir /S/Q "!wPkgPath!"
    )
)

set wzTarget="!wPkgPath!\\!tWizard!"
call :dbgprint "wPkgPath = " wPkgPath

if not exist !wzTarget! (

    if exist "!wPkgPath!" (
        call :dbgprint "Trying to replace obsolete version ... " wPkgPath
        rmdir /S/Q "!wPkgPath!"
    )

    call :dbgprint "-pkg-link = " pkgLink
    call :dbgprint "-server = " pkgSrv

    if defined pkgLink (
        set pkgSrv=!pkgLink!
        
        :: support of local relative paths
        if "!pkgSrv::=!"=="!pkgSrv!" (
            set pkgSrv=!cd!/!pkgSrv!
        )
        
        :: https://github.com/3F/GetNuTool/issues/6
        if "!wPkgPath::=!"=="!wPkgPath!" ( 
            set "_rlp=../" 
        )
        set "_remoteUrl=:!_rlp!!wPkgPath!"
    )

    :: https://github.com/3F/DllExport/issues/74
    if defined gMsbPath (
        set gntmsb=-msbuild "!gMsbPath!"
    )

    set _gntC=!gntmsb! /p:ngserver="!pkgSrv!" /p:ngpackages="!_remoteUrl!" /p:ngpath="!dxpPackages!" /p:proxycfg="!proxy!"
    call :dbgprint "GetNuTool call: " _gntC

    :: gnt's requirements
    set "_gntC=!_gntC:&=%%E_CARET%%&!"

    if defined dxpDebug (
        call :gntpoint !_gntC!
    ) else (
        call :gntpoint !_gntC! >nul
    )
)

if defined peExpList (
    "!wPkgPath!\\tools\\PeViewer.exe" -list -pemodule "!peExpList!"

    set /a EXIT_CODE=%ERRORLEVEL%
    goto endpoint
)

if defined buildInfo (
    call :dbgprint "buildInfo = '!wPkgPath!\\!buildInfoFile!'"

    if not exist "!wPkgPath!\\!buildInfoFile!" (
        echo information about build is not available.
        
        set /a EXIT_CODE=%ERROR_FILE_NOT_FOUND%
        goto endpoint
    )

    type "!wPkgPath!\\!buildInfoFile!"
    goto endpoint
)

if not exist !wzTarget! (
    echo Something went wrong. Try to use another keys.

    set /a EXIT_CODE=%ERROR_FILE_NOT_FOUND%
    goto endpoint
)

call :dbgprint "wRootPath = " wRootPath
call :dbgprint "wAction = " wAction
call :dbgprint "wMgrArgs = " wMgrArgs

if defined gMsbPath (
    call :dbgprint "Use specific MSBuild tools " gMsbPath

    set msbuildPath=!gMsbPath!
    goto rundxp
)

:: defines msbuildPath automatically
call :msbnetf
if "!ERRORLEVEL!"=="0" goto rundxp

echo MSBuild tools was not found. Try with `-msb` key. `-help` for details.
set /a EXIT_CODE=%ERROR_FILE_NOT_FOUND%
goto endpoint


:: - - -
:: Wizard
:rundxp

if not defined msbuildPath (
    echo Something went wrong. Use `-debug` key for details.

    set /a EXIT_CODE=%ERROR_FILE_NOT_FOUND%
    goto endpoint
)

set xMSBuild="!msbuildPath!"

if not defined xmgrtest (
    call :dbgprint "Target: " xMSBuild wzTarget
    !xMSBuild! /nologo /v:m /m:4 !wzTarget!
)


:: - - -
:: Post-actions
:endpoint

if defined xmgrtest (
    echo Running Tests ... "!xmgrtest!"
    !xMSBuild! /nologo /v:m /m:4 "!xmgrtest!"
    exit /B 0
)

:: keep it as the last one-line command before final exit!
if defined mgrUp (
    (copy /B/Y "!wPkgPath!\\DllExport.bat" "!fManager!" > nul) && ( echo Manager has been updated. & exit /B !EXIT_CODE! ) || ( echo -mgr-up failed. & exit /B %ERRORLEVEL% )
)

exit /B !EXIT_CODE!


:: Functions
:: ::

:: - - -
:: Tools from .NET Framework - .net 4.0, ...
:msbnetf

call :dbgprint "trying via MSBuild tools from .NET Framework - .net 4.0, ..."

for %%v in (4.0, 3.5, 2.0) do (
    call :rtools %%v Y & if [!Y!]==[1] exit /B 0
)
call :dbgprint "msbnetf: unfortunately we didn't find anything."
exit /B %ERROR_FILE_NOT_FOUND%
:: :msbnetf

:rtools
call :dbgprint "checking of version: %1"
    
for /F "usebackq tokens=2* skip=2" %%a in (
    `reg query "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSBuild\ToolsVersions\%1" /v MSBuildToolsPath 2^> nul`
) do if exist %%b (
    call :dbgprint "found: %%b"
        
    set msbuildPath=%%b
    call :msbfound
    set /a %2=1
    exit /B 0
)
set /a %2=0
exit /B 0
:: :rtools

:msbfound
set msbuildPath=!msbuildPath!\MSBuild.exe
exit /B 0
:: :msbfound

:dbgprint
if defined dxpDebug (
    set msgfmt=%1
    set msgfmt=!msgfmt:~0,-1! 
    set msgfmt=!msgfmt:~1!
    echo.[%TIME% ] !msgfmt! !%2! !%3!
)
exit /B 0
:: :dbgprint

:trim
:: Usage: 1: in/out
call :rtrim %1
call :ltrim %1
exit /B 0
:: :trim

:rtrim
:: Usage: 1: in/out
call :_trim %1 "-=1"
exit /B 0
:: :rtrim

:ltrim
:: Usage: 1: in/out
call :_trim %1 "+=1"
exit /B 0
:: :ltrim

:_trim
:: Algo-v2 avoids problems with special symbols.
:: Usage: 1: in/out; 2: "-=1" or "+=1"
:: 
:: remark: this is a more stable way instead of
:: - for() lot of problems especially for '='.
:: - %* is better than for() but too hard to protect 
::      this: '&!~`@#$^(+)_=-;[.]{,}
set ins=z!%1!z
if "%~2"=="-=1" (set "_rt=1") else (set "_rt=")

if defined _rt (
    set /a "i=-2"
) else (
    set /a "i=1"
)

:_tpos
if "!ins:~%i%,1!"==" " (
    set /a "i%~2"
    goto _tpos
)
if defined _rt set /a "i+=1"

if defined _rt (
    set "%1=!ins:~1,%i%!"
) else (
    set "%1=!ins:~%i%,-1!"
)
exit /B 0
:: :_trim


:gntpoint
setlocal disableDelayedExpansion 

:: ========================= GetNuTool =========================

