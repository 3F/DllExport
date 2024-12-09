::! https://github.com/3F/DllExport
@echo off & echo Incomplete script. Compile it first using build.bat: github.com/3F/DllExport >&2 & exit /B 1

set "dpnx0=%~dpnx0"
set args=%*   ::&:

:: Escaping '^' is not identical for all cases (DllExport ... vs call DllExport ...)
set ddargs=%* ::&:
if defined args (
    if defined __p_call (
        set ddargs=%ddargs:^^=^%
    ) else (
        set args=%args:^=^^%
    )
)
set wMgrArgs=%ddargs%

:: When ~ !args! and "!args!"
:: # call DllExport  ^  - ^
:: #      DllExport  ^  - empty
:: # call DllExport  ^^ - ^^
:: #      DllExport  ^^ - ^

:: When ~ %args%  (disableDelayedExpansion)
:: # call DllExport  ^  - ^^
:: #      DllExport  ^  - ^
:: # call DllExport  ^^ - ^^^^
:: #      DllExport  ^^ - ^^

:: Do not use: ~ "%args%" or %args%  + (enableDelayedExpansion)


:: [I] from scripts:
:: # call DllExport  %%%% - %
:: # call DllExport  %%   - empty
:: # call DllExport  %    - empty
:: # DllExport       %%   - %
:: # DllExport       %    - empty

:: [II] from command-line:
:: # call DllExport   %  -  %
:: # DllExport        %  -  %

:: PARSER NOTE: keep \r\n because `&` requires enableDelayedExpansion
set esc=%args:!= L %  ::&:
set esc=%esc:^= T %   ::&:
setlocal enableDelayedExpansion

:: https://github.com/3F/DllExport/issues/88
:: For '&':
:: 1. "_=%*" or _="%*"
:: 2.1. _=%* Works correctly only for delayed evaluation inside "...", i.e. don't use "args=%_% "
:: 2.2. Mostly can be safely called only as !args!, i.e. try do not use %args%
::
:: For '!':
:: 1. %~dpnx0 and %cd% in enableDelayedExpansion mode.
:: 1.1. Derivative assignments can be used safely only as !cd!, i.e. a=!cd! ... b=!a! but not as b=%a%
:: !cd! -ok
:: %~dpnx0 via !dpnx0! -ok

:: https://github.com/3F/hMSBuild/issues/7
set "E_CARET=^"
set "esc=!esc:%%=%%%%!"
set "esc=!esc:&=%%E_CARET%%&!"

:: - - -
:: Settings by default

set "dxpVersion=$-pkg-version-$"
set "wAction=Configure"

:: - - - 

set "dxpName=DllExport"
set "tWizard=tools/net.r_eg.DllExport.Wizard.targets"
set "dxpPackages=packages"
set "pkgSrv=https://www.nuget.org/api/v2/package/"
set "buildInfoFile=build_info.txt"
set "fManager=!dpnx0!"
set "wRootPath=%~dp0"

:: -
:: bitwise parameters

set /a wDxpOpt=0

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
set "khMSBuild="


set /a ERROR_SUCCESS=0
set /a ERROR_FAILED=1
set /a ERROR_FILE_NOT_FOUND=2
set /a ERROR_PATH_NOT_FOUND=3

:: Current exit code for endpoint
set /a EXIT_CODE=0

:: - - -
:: Initialization of user arguments

if not defined args (
    if defined wAction goto action
    goto usage
)

:: /? will cause problems for the call commands below, so we just escape this via supported alternative:
set esc=!esc:/?=/h!

:: process arguments through hMSBuild
call :inita arg esc amax
goto commands

:usage

echo.
echo .NET DllExport $-version-$
echo Copyright (c) 2009-2015  Robert Giesecke
echo Copyright (c) 2016-2024  Denis Kuzmin ^<x-3F@outlook.com^> github/3F
echo.
echo MIT License
echo https://github.com/3F/DllExport
echo.
echo.
echo Usage: DllExport [keys] or built-in [-GetNuTool ... or -hMSBuild ...]
echo.
echo Keys
echo ----
echo -action {type} - Specified action for Wizard. Where {type}:
echo   * Configure - To configure DllExport for specific projects.
echo   * Update    - To update pkg reference for already configured projects.
echo   * Restore   - To restore configured DllExport.
echo   * Export    - To export configured projects data.
echo   * Recover   - To re-configure projects using predefined data.
echo                `RecoverInit` to initial setup.
echo   * Unset     - To unset all data from specified projects.
echo   * Upgrade   - Aggregates an Update action with additions for upgrading.
echo.
echo -sln-dir {path}    - Path to directory with .sln files to be processed.
echo -sln-file {path}   - Optional predefined .sln file to be processed.
echo -metalib {path}    - Relative path to meta library.
echo -metacor {path}    - Relative path to meta core library.
echo -dxp-target {path} - Relative path to entrypoint wrapper of the main core.
echo -dxp-version {num} - Specific version of %~n0. Where {num}:
echo   * Versions: 1.7.4 ...
echo   * Keywords: 
echo     `actual` - Unspecified local/latest remote version; 
echo                ( Only if you know what you are doing )
echo.
echo -msb {path}           - Full path to specific MSBuild Tools.
echo -hMSBuild {args}      - Access to hMSBuild (built-in) https://github.com/3F/hMSBuild
echo -packages {path}      - A common directory for packages.
echo -server {url}         - Url for searching remote packages.
echo -proxy {cfg}          - To use proxy. The format: [usr[:pwd]@]host[:port]
echo -pkg-link {uri}       - Direct link to package from the source via specified URI.
echo -force                - Aggressive behavior, e.g. like removing pkg when updating.
echo -no-mgr               - Do not use %~n0 for automatic restore the remote package.
echo -mgr-up               - Updates %~n0 to version from '-dxp-version'.
echo -wz-target {path}     - Relative path to entrypoint wrapper of the main wizard.
echo -pe-exp-list {module} - To list all available exports from PE32/PE32+ module.
echo -eng                  - Try to use english language for all build messages.
echo -GetNuTool {args}     - Access to GetNuTool (built-in) https://github.com/3F/GetNuTool
echo -debug                - To show additional information.
echo -version              - Displays version for which (together with) it was compiled.
echo -build-info           - Displays actual build information from selected %~n0.
echo -help                 - Displays this help. Aliases: -help -h /h -? /?
echo.
echo Flags
echo -----
echo  __p_call - To use the call-type logic when invoking %~n0
echo.
echo Samples
echo -------
echo %~n0 -action Configure -force -pkg-link https://host/v1.7.4.nupkg
echo %~n0 -action Restore -sln-file "Conari.sln"
echo %~n0 -proxy guest:1234@10.0.2.15:7428 -action Configure
echo %~n0 -pe-exp-list bin\Debug\regXwild.dll
echo.
echo %~n0 -mgr-up -dxp-version 1.7.4
echo %~n0 -action Upgrade -dxp-version 1.7.4
echo.
echo %~n0 -GetNuTool "Conari;regXwild;Fnv1a128"
echo %~n0 -hMSBuild ~x ~c Release
echo %~n0 -GetNuTool vsSolutionBuildEvent/1.16.1:../SDK ^& SDK\GUI

goto endpoint

:: - - -
:: Logic for user arguments
:commands

set /a idx=0

:loopargs
set key=!arg[%idx%]!

    :: The help command

    if [!key!]==[-help] ( goto usage ) else if [!key!]==[-h] ( goto usage ) else if [!key!]==[-?] ( goto usage ) else if [!key!]==[/h] ( goto usage )

    :: Available keys

    if [!key!]==[-debug] ( 

        set dxpDebug=1

        goto continue
    ) else if [!key!]==[-action] ( set /a "idx+=1" & call :eval arg[!idx!] v
        
        set wAction=!v!
        for %%g in (Restore, Configure, Update, Export, Recover, RecoverInit, Unset, Upgrade, Default) do (
            if /I "!v!"=="%%g" goto continue
        )

        echo Unknown -action !v!
        exit /B %ERROR_FAILED%
        
    ) else if [!key!]==[-sln-dir] ( set /a "idx+=1" & call :eval arg[!idx!] v
        
        set wSlnDir=!v!

        goto continue
    ) else if [!key!]==[-sln-file] ( set /a "idx+=1" & call :eval arg[!idx!] v
        
        set wSlnFile=!v!

        goto continue
    ) else if [!key!]==[-metalib] ( set /a "idx+=1" & call :eval arg[!idx!] v
        
        set wMetaLib=!v!

        goto continue
    ) else if [!key!]==[-metacor] ( set /a "idx+=1" & call :eval arg[!idx!] v
        
        set wMetaCor=!v!

        goto continue
    ) else if [!key!]==[-dxp-target] ( set /a "idx+=1" & call :eval arg[!idx!] v
        
        set wDxpTarget=!v!

        goto continue
    ) else if [!key!]==[-dxp-version] ( set /a "idx+=1" & call :eval arg[!idx!] v
        
        set dxpVersion=!v!

        goto continue
    ) else if [!key!]==[-msb] ( set /a "idx+=1" & call :eval arg[!idx!] v
        
        set gMsbPath=!v!

        goto continue
    ) else if [!key!]==[-packages] ( set /a "idx+=1" & call :eval arg[!idx!] v
        
        set dxpPackages=!v!

        goto continue
    ) else if [!key!]==[-server] ( set /a "idx+=1" & call :eval arg[!idx!] v
        
        set pkgSrv=!v!

        goto continue
    ) else if [!key!]==[-proxy] ( set /a "idx+=1" & call :eval arg[!idx!] v
        
        set proxy=!v!
        set wProxy=!v!

        goto continue
    ) else if [!key!]==[-pkg-link] ( set /a "idx+=1" & call :eval arg[!idx!] v
        
        set pkgLink=!v!
        set dxpVersion=!key!

        goto continue
    ) else if [!key!]==[-force] ( 

        set kForce=1

        goto continue
    ) else if [!key!]==[-no-mgr] ( 

        set /a wDxpOpt^|=1

        goto continue
    ) else if [!key!]==[-mgr-up] ( 

        set mgrUp=1

        goto continue
    ) else if [!key!]==[-wz-target] ( set /a "idx+=1" & call :eval arg[!idx!] v
        
        set tWizard=!v!

        goto continue
    ) else if [!key!]==[-pe-exp-list] ( set /a "idx+=1" & call :eval arg[!idx!] v
        
        set peExpList=!v!

        goto continue
    ) else if [!key!]==[-eng] ( 

        chcp 437 >nul

        goto continue
    ) else if [!key!]==[-GetNuTool] ( 

        call :ktoolinit -GetNuTool 10
        set /a EXIT_CODE=!ERRORLEVEL! & goto endpoint

    ) else if [!key!]==[-hMSBuild] (

        set khMSBuild=1
        call :ktoolinit -hMSBuild 9
        set /a EXIT_CODE=!ERRORLEVEL! & goto endpoint

    ) else if [!key!]==[-version] ( 

        @echo $-version-$  %__dxp_pv%
        goto endpoint

    ) else if [!key!]==[-build-info] ( 

        set buildInfo=1

        goto continue
    ) else if [!key!]==[-tests] ( set /a "idx+=1" & call :eval arg[!idx!] v
        
        set xmgrtest=!v!

        goto continue
    ) else ( 
        echo Incorrect key: !key!
        set /a EXIT_CODE=%ERROR_FAILED%
        goto endpoint
    )

:continue
set /a "idx+=1" & if %idx% LSS !amax! goto loopargs


:: - - -
:: Main 
:action

call :dbgprint "dxpName = " dxpName
call :dbgprint "dxpVersion = " dxpVersion
call :dbgprint "-sln-dir = " wSlnDir
call :dbgprint "-sln-file = " wSlnFile
call :dbgprint "-metalib = " wMetaLib
call :dbgprint "-metacor = " wMetaCor
call :dbgprint "-dxp-target = " wDxpTarget
call :dbgprint "-wz-target = " tWizard
call :dbgprint "#opt " wDxpOpt

if defined dxpVersion (
    if "!dxpVersion!"=="actual" (
        set "dxpVersion="
    )
)
set wPkgVer=!dxpVersion!

if z%wAction%==zUpgrade (
    call :dbgprint "Upgrade is on"
    set mgrUp=1
    set kForce=1
)

call :trim dxpPackages
set "dxpPackages=!dxpPackages!\\"

set "reqPkg=!dxpName!"
set "wPkgPath=!dxpPackages!!dxpName!"

if defined dxpVersion (
    set "reqPkg=!reqPkg!/!dxpVersion!"
    set "wPkgPath=!wPkgPath!.!dxpVersion!"
)

if defined kForce (
    if exist "!wPkgPath!" (
        call :dbgprint "Removing the old version. '-force' key rule. " wPkgPath
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
        
        :: Relative path support to local
        if "!pkgSrv::=!"=="!pkgSrv!" (
            set pkgSrv=!cd!/!pkgSrv!
        )
        
        :: https://github.com/3F/GetNuTool/issues/6
        if "!wPkgPath::=!"=="!wPkgPath!" (
            set "reqPkg=:../!wPkgPath!"
        ) else (
            set "reqPkg=:!wPkgPath!"
        )
    )

    :: https://github.com/3F/DllExport/issues/74
    if defined gMsbPath (
        set msb.gnt.cmd=!gMsbPath!
    )

    set _gntC=-GetNuTool "!reqPkg!" /p:ngserver="!pkgSrv!" /p:ngpath="!dxpPackages!" /p:proxycfg="!proxy!"
    call :invokeCore _gntC "no"
)

if defined peExpList (
    "!wPkgPath!\\tools\\PeViewer.exe" -list -pemodule "!peExpList!"

    set /a EXIT_CODE=%ERRORLEVEL%
    goto endpoint
)

if defined buildInfo (
    call :dbgprint "buildInfo = " wPkgPath buildInfoFile

    if not exist "!wPkgPath!\\!buildInfoFile!" (
        echo information about build is not available.
        
        set /a EXIT_CODE=%ERROR_FILE_NOT_FOUND%
        goto endpoint
    )

    type "!wPkgPath!\\!buildInfoFile!"
    goto endpoint
)

call :dbgprint "wRootPath = " wRootPath
call :dbgprint "wAction = " wAction
call :dbgprint "wMgrArgs = " wMgrArgs
call :dbgprint "wzTarget = " wzTarget

:: do not use rsp due to possible https://github.com/3F/DllExport/issues/223
set argsWz=/nologo /noautorsp

if not defined xmgrtest (

    if not exist !wzTarget! (
        echo Target cannot be initialized. Try to use another keys.

        set /a EXIT_CODE=%ERROR_FILE_NOT_FOUND%
        goto endpoint
    )

    if defined gMsbPath (
        call :dbgprint "Use specific MSBuild Tools: " gMsbPath

        if not exist "!gMsbPath!" (
            echo MSBuild Tools was not found. Check -msb key.>&2
            set /a EXIT_CODE=%ERROR_FILE_NOT_FOUND%
            goto endpoint
        )

        :: keep "!gMsbPath!" inside quotes because of removing in :loopargs
        call "!gMsbPath!" !wzTarget! !argsWz! /v:m /m:4

    ) else (
        set _hmsbC=~x -cs !wzTarget! !argsWz!
        call :invokeCore _hmsbC
    )
)

if !ERRORLEVEL! NEQ 0 (
    echo Something went wrong. Use `-debug` key for details.
    set /a EXIT_CODE=%ERROR_FILE_NOT_FOUND%
)

:: - - -
:: Post-actions
:endpoint

if defined xmgrtest ( 
    echo Running Tests ... "!xmgrtest!"

    set _hmsbC=~x -cs "!xmgrtest!" !argsWz!
    call :invokeCore _hmsbC

    exit /B !ERRORLEVEL!
)

:: keep it as the last one-line command before final exit!
if defined mgrUp (
    (copy /B/Y "!wPkgPath!\\DllExport.bat" "!fManager!" > nul) && ( echo Manager has been updated. & exit /B 0 ) || ( (echo -mgr-up failed:!EXIT_CODE! 1>&2) & exit /B 1 )
)

exit /B !EXIT_CODE!


:: - - - - - - -
:: API & binding
:: - - - - - - -

    :: /hMSBuild  (#) :inita

    :: /hMSBuild  (~) :eval
    :eval
        call :eva %*
    exit /B

:: - - -

:: Functions
:: ::

:: - - -
:: Initializer for tools such GetNuTool, hMSBuild, etc.
:ktoolinit
:: 1 - requested tool
:: 2 - length of the key
:: esq - prepared arguments for tool
:: wPkgPath - root pkg path
set key=%~1
set /a klen=%~2

    call :dbgprint "accessing to !key! ..."
    ::&:
    
    :: invoke a tool with arguments from right side
    for /L %%p IN (0,1,8181) DO (
        if "!esc:~%%p,%klen%!"=="!key!" (

            set found=!esc:~%%p!

            :: TODO: up compressor for: call !xktool! !found:~%klen%!
            set kargs=!found:~%klen%!
            
            if defined khMSBuild (

                call :invokeCore kargs

            ) else (

                set kargs=-GetNuTool !kargs!
                call :invokeCore kargs
            )
            
            exit /B !ERRORLEVEL!
        )
    )

    call :dbgprint "!key! is corrupted: " esc

exit /B %ERROR_FAILED%
:: :ktoolinit

:invokeCore {in:uneval} {in:string}
    :: (1) - Input keys to core
    :: [2] - logo option if used

    set _ic=!%~1!

    :: Note, `logo` can be overridden at the top level, for example in tests, etc.
    if not defined logo set "logo=%~2"
    call :dbgprint "invoke a built-in hMSBuild: " _ic logo

    if defined khMSBuild (
        if defined dxpDebug set _ic=-debug !_ic!
    )

    call :hMSBuild !_ic!

exit /B !ERRORLEVEL!
:: :invokeCore

:dbgprint {in:str} [{in:uneval}, [{in:uneval}]]
    if defined dxpDebug (
        :: NOTE: delayed `dmsg` because symbols like `)`, `(` ... requires protection after expansion. /hMSBuild Y-32
        set "dmsg=%~1" & echo [ %TIME% ] !dmsg! !%2! !%3!
    )
exit /B 0
:: :dbgprint

:trim {in/out:str}
    call :rtrim %1
    call :ltrim %1
exit /B 0
:: :trim

:rtrim {in/out:str}
    call :_trim %1 "-=1"
exit /B 0
:: :rtrim

:ltrim {in/out:str}
    call :_trim %1 "+=1"
exit /B 0
:: :ltrim

:_trim {in/out:str} {in:type}
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


:hMSBuild
setlocal disableDelayedExpansion
