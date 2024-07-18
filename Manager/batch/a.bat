@echo off
:: Copyright (c) 2015  Denis Kuzmin <x-3F@outlook.com> github/3F
:: Part of https://github.com/3F/GetNuTool

if "%~1"=="" echo Empty function name & exit /B 1
call :%~1 %2 %3 %4 %5 %6 %7 %8 %9 & exit /B !ERRORLEVEL!

:initAppVersion
    :: [1] - Optional postfix.
    for /F "tokens=*" %%i in (..\.version) do set "appversion%~1=%%i"
exit /B

:invoke
    ::  (1) - Command.
    :: &(2) - Input arguments inside "..." via variable.
    :: &[3] - Return code.
    :: !!0+ - Error code from (1)

    set "cmd=%~1 !%2!"

    :: NOTE: Use delayed !cmd! instead of %cmd% inside `for /F` due to
    :: `=` (equal sign, which cannot be escaped as `^=` when runtime evaluation %cmd%)

    set "cmd=!cmd! 2^>^&1 ^&call echo %%^^ERRORLEVEL%%"
    set /a msgIdx=0

    for /F "tokens=*" %%i in ('!cmd!') do 2>nul (
        set /a msgIdx+=1
        set msg[!msgIdx!]=%%i
    )

    if not "%3"=="" set %3=!msg[%msgIdx%]!
exit /B !msg[%msgIdx%]!

:execute
    ::  (1) - Command.
    :: !!0+ - Error code from (1)

    call :invoke "%~1" nul retcode
exit /B !retcode!

:startExTest
    ::  (1) - Logic via :label name
    ::  (2) - Input arguments to core inside "...". Use ` sign to apply " double quotes inside "...".
    ::  [3] - Expected return code. Default, 0.
    :: !!1  - Error code 1 if app's error code is not equal [2] as expected.

    set "tArgs=%~2"
    if "%~3"=="" ( set /a exCode=0 ) else set /a exCode=%~3

    if "!tArgs!" NEQ "" set tArgs=!tArgs:`="!

    set /a gcount+=1
    echo.
    echo - - - - - - - - - - - -
    echo Test #%gcount% @ %TIME%
    echo - - - - - - - - - - - -
    echo keys: !tArgs!
    echo.

    set callback=%~1 & shift

    goto %callback%
    :_logicExTestEnd

    if "!retcode!" NEQ "%exCode%" call :failTest & exit /B 1
exit /B 0

:startTest
    ::  (1) - Input arguments to core inside "...". Use ` sign to apply " double quotes inside "...".
    ::  [2] - Expected return code. Default, 0.
    :: !!1  - Error code 1 if app's error code is not equal [2] as expected.

    call :startExTest _logicStartTest %*
    exit /B
    :_logicStartTest
        call :invoke "%wdir%%exec%" tArgs retcode

goto _logicExTestEnd
:: :startTest

:startABTest
    ::   (1) - Input arguments inside "...". Use ` sign to apply " double quotes inside "...".
    ::   (2) - A command
    ::   (3) - B command
    ::  &(4) - Result from (2) A
    ::  &(5) - Result from (3) B

    set "exA=%2" & set "exB=%3"
    set "_4=%4"
    set "_5=%5"

    call :startExTest _logicStartABTest %1
    exit /B
    :_logicStartABTest
        call :invoke !exA! tArgs retcodeA & call :getMsgAt 1 outA
        call :invoke !exB! tArgs retcodeB & call :getMsgAt 1 outB

        set %_4%=!outA! !retcodeA!
        set %_5%=!outB! !retcodeB!
        set /a retcode=0

goto _logicExTestEnd
:: :startABTest

:startABStreamTest
    ::    (1) - Input arguments inside "...". Use ` sign to apply " double quotes inside "...".
    ::    (2) - A command
    ::    (3) - B command
    ::    [4] - Expected return code. Default, 0.
    ::  &*[5] - Result stream from (2) A; e.g. !%argname%[n+]!

    set "exA=%2" & set "exB=%3"
    set "_5=%5"

    call :startExTest _logicStartABStreamTest "-debug %~1" %~4
    exit /B
    :_logicStartABStreamTest

        :: Disables time [ 21:50:25.46 ] as [ - ]
        set "TIME=-"

        call :invoke !exA! tArgs retcodeA
        call :cloneStreamAs _streamA
        call :invoke !exB! tArgs retcodeB

        set /a retcode=%retcodeB%
        call :eqOriginStreamWithOrFail _streamA 1 || set /a retcode=1

        set "TIME="
        if defined _5 set %_5%=_streamA

goto _logicExTestEnd
:: :startABStreamTest

:abStreamTest
    ::    (1) - Input arguments inside "...". Use ` sign to apply " double quotes inside "...".
    ::    (2) - A command
    ::    (3) - B command
    ::    [4] - Expected return code. Default, 0.

    call :startABStreamTest "%~1" "%~2" "%~3" %~4 || exit /B 1
    call :completeTest
exit /B 0

:startVFTest
    ::  (1) - Input core application.
    ::  (2) - Input arguments to core inside "...". Use ` sign to apply " double quotes inside "...".
    ::  (3) - Full path to actual data in the file system.
    :: &(4) - Return actual data.

    set _exapp="%~1"
    set _lwrap="%~3"
    set "_4=%4"

    call :startExTest _logicStartVFTest %2
    exit /B
    :_logicStartVFTest
        call :invoke %_exapp% tArgs retcode
        for /f "usebackq tokens=*" %%i in (`type %_lwrap%`) do set "%_4%=%%i"

goto _logicExTestEnd
:: :startVFTest

:completeTest
    call :cprint 27 [Passed]
exit /B 0

:failTest
    :: [1] - Optional message string.

    set /a "failedTotal+=1"

    if not "%~1"=="" echo %~1
    call :printStream failed
    echo. & call :cprint 47 [Failed]
exit /B 0

:printStream
    if "!msgIdx!"=="" exit /B 1
    for /L %%i in (0,1,!msgIdx!) do echo (%%i) *%~1: !msg[%%i]!
exit /B 0

:printStreamAB
    :: &(1) - Stream name to print together with origin.

    if "!msgIdx!"=="" exit /B 1
    for /L %%i in (0,1,!msgIdx!) do (
        echo `!_streamA[%%i]!` & echo `!msg[%%i]!`  & echo --
    )
exit /B 0

:failStreamsTest
    :: &(1) - Stream name to print together with origin.
    ::  [2] - Don't count in the total counter if 1.

    if "%~2" NEQ "1" set /a "failedTotal+=1"
    call :printStreamAB %~1
exit /B 0

:contains
    :: &(1) - Input string via variable
    ::  (2) - Substring to check. Use ` instead of " and do NOT use =(equal sign) since it's not protected.
    :: &(3) - Result, 1 if found.

    :: TODO: L-39 protect from `=` like the main module does; or compare in parts using `#`

    set "input=!%~1!"

    if "%~2"=="" if "!input!"=="" set /a %3=1 & exit /B 0
    if "!input!"=="" if not "%~2"=="" set /a %3=0 & exit /B 0

    set "input=!input:"=`!"
    set "cmp=!input:%~2=!"

    if "!cmp!" NEQ "!input!" ( set /a %3=1 ) else set /a %3=0
exit /B 0

:printMsgAt
    ::  (1) - index at msg
    ::  [2] - color attribute via :color call
    ::  [3] - prefixed message at the same line
    :: !!1  - Error code 1 if &(1) is empty or not valid.

    call :getMsgAt %~1 _msgstr || exit /B 1

    if not "%~2"=="" (
        call :cprint %~2 "%~3!_msgstr!"

    ) else echo !_msgstr!
exit /B 0

:getMsgAt
    ::  (1) - index at msg
    :: &(2) - result string
    :: !!1  - Error code 1 if &(1) is empty or not valid.

    if "%~1"=="" exit /B 1
    if %msgIdx% LSS %~1 exit /B 1
    if %~1 LSS 0 exit /B 1

    set %2=!msg[%~1]!
exit /B 0

:msgAt
    ::  (1) - index at msg
    ::  (2) - substring to check
    :: &(3) - result, 1 if found.

    set /a %3=0
    call :getMsgAt %~1 _msgstr || exit /B 0

    call :contains _msgstr "%~2" _n & set /a %3=!_n!
exit /B 0

:msgOrFailAt
    ::  (1) - index at msg
    ::  (2) - substring to check
    :: !!1  - Error code 1 if the message is not found at the specified index.

    call :msgAt %~1 "%~2" _n & if .!_n! NEQ .1 call :failTest & exit /B 1
exit /B 0

:checkFs
    ::  (1) - Path to directory that must be available.
    ::  (2) - Path to the file that must exist.
    :: !!1  - Error code 1 if the directory or file does not exist.

    if not exist "%~1" call :failTest & exit /B 1
    if not exist "%~1\%~2" call :failTest & exit /B 1
exit /B 0

:checkFsBase
    ::  (1) - Path to directory that must be available.
    ::  (2) - Path to the file that must exist.
    :: !!1  - Error code 1 if the directory or file does not exist.

    call :checkFs "%basePkgDir%%~1" "%~2"
exit /B

:checkFsNo
    ::  (1) - Path to the file or directory that must NOT exist.
    :: !!1  - Error code 1 if the specified path exists.

    if exist "%~1" call :failTest & exit /B 1
exit /B 0

:checkFsBaseNo
    ::  (1) - Path to the file or directory that must NOT exist.
    :: !!1  - Error code 1 if the specified path exists.

    call :checkFsNo "%basePkgDir%%~1"
exit /B

:unsetDir
    :: (1) - Path to directory.
    call :isStrNotEmptyOrWhitespaceOrFail "%~1" || exit /B 1
    rmdir /S/Q "%~1" 2>nul
exit /B 0

:unsetPackage
    :: (1) - Package directory.
    call :unsetDir "%basePkgDir%%~1"
exit /B 0

:unsetFile
    :: (1) - File name.
    call :isStrNotEmptyOrWhitespaceOrFail "%~1" || exit /B 1
    del /Q "%~1" 2>nul
exit /B 0

:unsetNupkg
    :: (1) - Nupkg file name.
    call :unsetFile "%~1"
exit /B 0

:checkFsNupkg
    ::  (1) - Nupkg file name.
    :: !!1  - Error code 1 if the input (1) does not exist.

    if not exist "%~1" call :failTest & exit /B 1
exit /B 0

:findInStream
    ::  (1) - substring to check
    ::  [2] - Start index, 0 by default.
    :: &[3] - Return index or -1 if not found.
    :: !!1  - Error code 1 if failed.
    :: !!3  - Error code 3 if not found.

    if "%~2"=="" (set /a _sidx=0) else set /a _sidx=%~2
    if %_sidx% LSS 0 exit /B 1
    if %msgIdx% LSS %_sidx% exit /B 1

    for /L %%i in (%_sidx%,1,!msgIdx!) do (
        call :msgAt %%i "%~1" _n & if .!_n! EQU .1 (
            if not "%~3"=="" set /a %3=%%i
            exit /B 0
        )
    )
    if not "%~3"=="" set /a %3=-1
exit /B 3

:findInStreamOrFail
    ::  (1) - substring to check
    ::  [2] - Start index, 0 by default.
    :: &[3] - Return index or -1 if not found.
    :: !!1  - Error code 1 if failed.

    call :findInStream "%~1" %~2 %~3 || ( call :failTest & exit /B 1 )
exit /B 0

:failIfInStream
    ::  (1) - substring to check
    :: !!1  - Error code 1 if the input (1) was not found.

    call :findInStream "%~1" n & if .!n! EQU .1 ( call :failTest & exit /B 1 )
exit /B 0

:print
    :: (1) - Input string.

    :: NOTE: delayed `dmsg` because symbols like `)`, `(` ... requires protection after expansion. L-32
    set "dmsg=%~1" & echo [ %TIME% ] !dmsg!
exit /B 0

:cprint
    :: (1) - color attribute via :color call
    :: (2) - Input string.

    call :color %~1 "%~2" & echo.
exit /B 0

:color
    :: (1) - color attribute, {background} | {foreground}
            :: 0 = Black       8 = Gray
            :: 1 = Blue        9 = Light Blue
            :: 2 = Green       A = Light Green
            :: 3 = Aqua        B = Light Aqua
            :: 4 = Red         C = Light Red
            :: 5 = Purple      D = Light Purple
            :: 6 = Yellow      E = Light Yellow
            :: 7 = White       F = Bright White

    :: (2) - Input string.

    <nul set/P= >"%~2"
    findstr /a:%~1  "%~2" nul
    del "%~2">nul
exit /B 0

:isNotEmptyOrWhitespace
    :: &(1) - Input variable.
    :: !!1  - Error code 1 if &(1) is empty or contains only whitespace characters.

    set "_v=!%~1!"
    if not defined _v exit /B 1

    set _v=%_v: =%
    if not defined _v exit /B 1

    :: e.g. set a="" not set "a="
exit /B 0

:sha1At0
    ::  (1) - Stream index.
    :: &(2) - sha1 result.
    set %2=!msg[%~1]:~4,40!
exit /B 0

:sha1At
    ::  (1) - Stream index.
    :: &(2) - sha1 result.
    set %2=!msg[%~1]:~45,40!
exit /B 0

:errargs
    echo.
    echo. Incorrect arguments. >&2
exit /B 1

:isNotEmptyOrWhitespaceOrFail
    :: &(1) - Input variable.
    :: !!1  - Error code 1 if &(1) is empty or contains only whitespace characters.
    call :isNotEmptyOrWhitespace %1 || (call :errargs & exit /B 1)
exit /B 0

:isStrNotEmptyOrWhitespaceOrFail
    :: (1) - Input string.
    :: !!1  - Error code 1 if (1) is empty or contains only whitespace characters.
    set "_wstrv=%~1"
    call :isNotEmptyOrWhitespaceOrFail _wstrv
exit /B

:cloneStreamAs
    :: &(1) - Destination.

    for /L %%i in (0,1,!msgIdx!) do set "%~1[%%i]=!msg[%%i]!"
exit /B

:eqOriginStreamWith
    :: &(1) - Current stream with stream from (1).
    :: &(2) - Return 1 if both streams are equal.

    for /L %%i in (0,1,!msgIdx!) do (
        if not "!%~1[%%i]!"=="!msg[%%i]!" ( set "%2=0" & exit /B 0 )
    )
    set "%2=1"
exit /B 0

:eqOriginStreamWithOrFail
    :: &(1) - Current stream with stream from (1).
    ::  [2] - Don't count in the total counter if 1.

    call :eqOriginStreamWith %~1 _r & if !_r! EQU 0 (
        call :failStreamsTest %~1 %~2 & exit /B 1
    )
exit /B 0

:disableAppVersion
    :: [1] - Optional postfix.
    set "appversion%~1=off"
exit /B 0