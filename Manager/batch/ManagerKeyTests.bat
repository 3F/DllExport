@echo off
:: Copyright (c) 2016  Denis Kuzmin <x-3F@outlook.com> github/3F
:: Tests. Part of https://github.com/3F/DllExport

setlocal enableDelayedExpansion
call a isNotEmptyOrWhitespaceOrFail %~1 || exit /B1

set /a gcount=!%~1! & set /a failedTotal=!%~2!
set "exec=%~3" & set "wdir=%~4"

:::::::::::::::::: :::::::::::::: :::::::::::::::::::::::::
:: Init

set "mgrFile=%exec%"
set "tmapFile=..\maps\%mgrFile%.bat.map.targets"

:: NOTE: $version$ managed via vsSolutionBuildEvent.bat
set exec=%mgrFile% -tests "ManagerKeyTests.targets" -pkg-link "..\..\DllExport.$version$.nupkg"


:::::::::::::::::: :::::::::::::: :::::::::::::::::::::::::
:: Tests


    ::_______ ------ ______________________________________

        call a startTest "-help" || goto x
            call a msgOrFailAt 0 "" || goto x

            if not defined appversionDxp call a failTest "Empty *appversionDxp" & goto x
            if not "%appversionDxp%"=="off" (
                call a msgOrFailAt 1 ".NET DllExport %appversionDxp%" || goto x
            )
            call a msgOrFailAt 5 "github.com/3F/DllExport" || goto x
        call a completeTest
    ::_____________________________________________________


    ::_______ ------ ______________________________________

        call a startTest "-version" || goto x
            if not defined appversionDxp call a failTest "Empty *appversionDxp" & goto x
            if not "%appversionDxp%"=="off" (
                call a msgOrFailAt 1 "%appversionDxp%" || goto x
            )
        call a completeTest
    ::_____________________________________________________


    ::_______ ------ ______________________________________

        call :testHeaders "-?" || goto x

        call :testHeaders "-h" || goto x

        call :testHeaders "/?" || goto x

        call :testHeaders "/h" || goto x
    ::_____________________________________________________


    set "dataCase=basic"

    ::_______ ------ ______________________________________

    call :checkActionTest "Configure" || goto x

    call :checkActionTest "Update" || goto x

    call :checkActionTest "Restore" || goto x

    call :checkActionTest "Export" || goto x

    call :checkActionTest "Recover" || goto x

    call :checkActionTest "Unset" || goto x

    call :checkActionTest "Upgrade" || goto x
    ::_____________________________________________________


    ::_______ ------ ______________________________________

    :: before/after -action
    call :checkTest "-force -action Configure -eng" "kForce: 1" || goto x
    ::_____________________________________________________


    ::_______ ------ ______________________________________

    :: -sln-dir {path} without double quotes
    call :checkTest "-action Configure -sln-dir directory123" "wSlnDir: directory123" || goto x
    ::_____________________________________________________


    ::_______ ------ ______________________________________

    :: -sln-dir {path} with double quotes
    call :checkTest "-action Configure -sln-dir `directory 123`" "wSlnDir: directory 123" || goto x
    ::_____________________________________________________


    ::_______ ------ ______________________________________

    :: -sln-dir {path} with special symbols
    call a startTest "-action Configure -sln-dir `crazy' dir&name356~@#$(+)_-;[.]{,}`" || goto x
        call a findInStreamOrFail "wSlnDir: crazy' dir&name356~@#$(+)_-;[.]{,}" 1,n || goto x
    call a completeTest
    ::_____________________________________________________


    ::_______ ------ ______________________________________

    :: check special symbols for -sln-dir -sln-file, -metacor, -metalib, -dxp-target
    call :testSpecSymbols || ( call a failTest & goto x )
    call a completeTest
    ::_____________________________________________________


    ::_______ ------ ______________________________________

    call :checkTest "-no-mgr" "wDxpOpt: 1"
    ::_____________________________________________________


    ::_______ ------ ______________________________________

    :: -sln-dir {path} for path \/: symbols
    call :checkTest "-action Configure -sln-dir `D:\\dir1/dir2/` -eng" "wSlnDir: D:\\dir1/dir2/" || goto x
    ::_____________________________________________________


    ::_______ ------ ______________________________________

    :: check -dxp-version, -server, -proxy
    call ..\%exec% -action Default -dxp-version 1.7.4 >nul
    call a startTest "-action Default -dxp-version 1.7.4 -server `https://127.0.0.1:8082/` -proxy `guest:1234@127.0.0.1:7428`" || goto x
        call a findInStreamOrFail "dxpVersion: 1.7.4" || goto x
        call a findInStreamOrFail "pkgSrv: https://127.0.0.1:8082/" || goto x
        call a findInStreamOrFail "proxy: guest:1234@127.0.0.1:7428" || goto x
    call a completeTest
    ::_____________________________________________________


    ::_______ ------ ______________________________________

    call a startTest "-action Default -pe-exp-list `bin\Debug\regXwild.dll`" || goto x
        call a findInStreamOrFail "peExpList: bin\Debug\regXwild.dll" || goto x
    call a completeTest
    ::_____________________________________________________


:::::::::::::
call :cleanup

:::::::::::::::::: :::::::::::::: :::::::::::::::::::::::::
::
:x
endlocal & set /a %1=%gcount% & set /a %2=%failedTotal%
if !failedTotal! EQU 0 exit /B 0
exit /B 1

:cleanup

exit /B 0

:checkActionTest
    ::  (1) - the Action value.
    call :checkTest "-action %~1" "wAction: %~1"
exit /B

:checkTest
    ::  (1) - actual key.
    ::  (2) - check output value.

    call a startTest "%~1" || exit /B
        call a findInStreamOrFail "Running Tests ..." 1,n|| exit /B
        @REM call a msgOrFailAt 3 "%~2" || exit /B
        call a findInStreamOrFail "%~2" !n! || exit /B
    call a completeTest
exit /B

:testSpecSymbols

    set /a gcount+=1
    echo.
    echo - - - - - - - - - - - -
    echo Test #%gcount% @ %TIME%
    echo - - - - - - - - - - - -
    echo.

    set /a msgIdx=-1
    set "__p_call=1"
    setlocal disableDelayedExpansion

        rem possible: ~`!@#$%^&()_+=-;'[]{}.,
        rem not allowed: \/:*?"<>|

        rem note about `%`
            :: [I] from scripts:
            :: # call DllExport  %%%% - %
            :: # call DllExport  %%   - empty
            :: # call DllExport  %    - empty
            :: # DllExport       %%   - %
            :: # DllExport       %    - empty

            :: [II] from command-line:
            :: # call DllExport   %  -  %
            :: # DllExport        %  -  %

        set _arg="crazy' dir&name!356~`@#$^(+)_=-;[.]{,%%%%}"

        set "testCase=SpecSymbols"
            call ..\%exec% -action Configure -sln-dir %_arg% -eng -sln-file %_arg% -metacor %_arg% -metalib %_arg% -dxp-target %_arg% -packages %_arg%
            set /a retCode=%ERRORLEVEL%
        set "testCase="

    set "__p_call="
    setlocal enableDelayedExpansion
exit /B %retCode%

:testHeaders
    ::  (1) - supported keys

    call a startTest "%~1" || exit /B
        if not "%appversionDxp%"=="off" (
            call a msgOrFailAt 1 ".NET DllExport %appversionDxp%" || exit /B
        )
        call a msgOrFailAt 5 "github.com/3F/DllExport" || exit /B
    call a completeTest
exit /B
