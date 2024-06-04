@echo off
:: netfx4sdk 1.2.0.35758+0a15d42
:: Copyright (c) 2021-2024  Denis Kuzmin <x-3F@outlook.com> github/3F
:: Copyright (c) netfx4sdk contributors https://github.com/3F/netfx4sdk
set "oo=%~dp0"
set op=%*
set /a oq=0
setlocal enableDelayedExpansion&if not defined op goto o9
set or=!op:/?=-h!
call :po o6 or o7
goto pp
:o9
echo.&echo netfx4sdk 1.2.0.35758+0a15d42&echo Copyright (c) 2021-2024  Denis Kuzmin ^<x-3F@outlook.com^> github/3F&echo Copyright (c) netfx4sdk contributors https://github.com/3F/netfx4sdk&echo.&echo ....&echo Keys&echo.&echo  -mode {value}&echo   * system   - (Recommended) Hack using assemblies for windows.&echo   * package  - Apply obsolete remote package. Read [About modes] below.&echo   * sys      - Alias to `system`&echo   * pkg      - Alias to `package`&echo.&echo  -force    - Aggressive behavior when applying etc.&echo  -rollback - Rollback applied modifications.&echo  -global   - To use the global toolset, like hMSBuild.&echo.&echo  -pkg-version {arg} - Specific package version. Where {arg}:&echo      * 1.0.3 ...&echo      * latest - (keyword) To use latest version;&echo.&echo  -debug    - To show debug information.&echo  -version  - Display version of %~nx0.&echo  -help     - Display this help. Aliases: -help -h -?&echo.&echo ...........&echo About modes&echo.&echo `-mode sys` highly recommended because&echo  [++] All modules are under windows support.&echo  [+] It does not require internet connection (portable).&echo  [+] No decompression required (faster) compared to package mode.&echo  [-] This is behavior-based hack;&echo      Report or please fix us if something:&echo      https://github.com/3F/netfx4sdk&echo.&echo `-mode package` will try to apply obsolete package to the environment.&echo  [-] Officially dropped support since VS2022.&echo  [-] Requires internet connection to receive ~30 MB via GetNuTool.&echo  [-] Requires decompression of received data to 178 MB before use.&echo  [+] Well known official behavior.&echo.&echo ...................&echo netfx4sdk -mode sys
echo netfx4sdk -rollback&echo netfx4sdk -debug -force -mode package&echo netfx4sdk -mode pkg -pkg-version 1.0.2&goto pq
:pp
set "os=v4.0"
set "ot=1.0.3"
set "ou=" & set "ov=" & set "ow=" & set "ox=" & set "oy="
set /a oz=0
:pr
set oa=!o6[%oz%]!
if [!oa!]==[-help] (goto o9)else if [!oa!]==[-h] (goto o9)else if [!oa!]==[-?] (goto o9 )
if [!oa!]==[-debug] (set ou=1
goto ps)else if [!oa!]==[-mode] (set /a "oz+=1" & call :pt o6[!oz!] v
if not "!v!"=="sys" if not "!v!"=="system" if not "!v!"=="pkg" if not "!v!"=="package" goto pu
if "!v!"=="system" (set "ov=sys")else if "!v!"=="package" (set "ov=pkg")else set "ov=!v!"
goto ps)else if [!oa!]==[-rollback] (set ow=1
goto ps)else if [!oa!]==[-pkg-version] (set /a "oz+=1" & call :pt o6[!oz!] v
set ot=!v!
call :pv "set package version:" v
goto ps)else if [!oa!]==[-version] (@echo 1.2.0.35758+0a15d42
goto pq)else if [!oa!]==[-global] (set oy=1
goto ps)else if [!oa!]==[-force] (set ox=1
goto ps)else (
:pu
call :pw "Incorrect key or value for `!oa!`"
set /a oq=1
goto pq
)
:ps
set /a "oz+=1" & if %oz% LSS !o7! goto pr
:px
call :pv "run action... " ov ox
set ob=%ProgramFiles(x86)%
if not exist "%ob%" set ob=%ProgramFiles%
set ob=%ob%\Reference Assemblies\Microsoft\Framework\.NETFramework\
set oc=%ob%%os%
set od=%oc%.%~nx0
if defined ow (if not exist "%od%" (echo There's nothing to rollback.
goto pq)
rmdir /Q/S "%oc%" 2>nul
call :pv "ren " od os
( ren "%od%" %os% 2>nul ) || (set /a oq=1100 & goto pq )
echo Rollback completed.&goto pq)
if exist "%od%" (echo %~nx0 has already been applied before. There's nothing to do anymore.
echo Use `-rollback` key to re-apply with another mode if needed.&exit/B0)
if exist "%oc%\mscorlib.dll" (if not defined ox (echo The Developer Pack was found successfully. There's nothing to do here at all.
echo Use `-force` key to suppress the restriction if you really know what you are doing.&set /a oq=0 & goto pq)
call :pv "Suppress found SDK " oc)
if not defined ov (set /a oq=1000 & goto pq )
if defined oy (set "oe=hMSBuild")else set oe="%~dp0hMSBuild"
call :py oe "-version" || (set /a oq=1003 & goto pq )
call :pz o8 & if !o8! LSS 2.4 (set /a oq=1002 & goto pq)
if "%ov%"=="sys" (echo Apply hack using assemblies for windows ...
call :py oe "-no-less-4 -no-vswhere -no-vs -only-path -notamd64"
set /a oq=%ERRORLEVEL% & if !oq! NEQ 0 goto pq
call :pz of
call :pa "%oc%" "%od%" || goto pq
set of=!of:msbuild.exe=!
call :pv "lDir " of
if not exist "!of!" (set /a oq=3 & goto pq )
mkdir "%oc%" 2>nul
for /F "tokens=*" %%i in ('dir /B "!of!*.dll"') do mklink "%oc%\%%i" "!of!%%i" >nul 2>nul
for /F "tokens=*" %%i in ('dir /B "!of!WPF\*.dll"') do mklink "%oc%\%%i" "!of!WPF\%%i" >nul 2>nul
set "og=%oc%\RedistList" & mkdir "!og!" 2>nul
echo ^<?xml version="1.0" encoding="utf-8"?^>^<FileList Redist="Microsoft-Windows-CLRCoreComp.4.0" Name=".NET Framework 4" RuntimeVersion="4.0" ToolsVersion="4.0" /^>> "!og!\FrameworkList.xml"&set "og=%oc%\PermissionSets" & mkdir "!og!" 2>nul
echo ^<PermissionSet oh="1" class="System.Security.PermissionSet" Unrestricted="true" /^>> "!og!\FullTrust.xml")else if "%ov%"=="pkg" (set oi=Microsoft.NETFramework.ReferenceAssemblies.net40&echo Apply !oi! package ...&set oj=%~nx0.%ot%
if "%ot%"=="latest" (set "ot=")else set ot=/%ot%
if defined ou set oe=!oe! -debug
call !oe! -GetNuTool /p:ngpackages="!oi!!ot!:!oj!"
set "ok=packages\!oj!\build\.NETFramework\%os%"
call :pv "dpkg " ok
if not exist "!ok!" (set /a oq=1001 & goto pq)
ren "%oc%" %os%.%~nx0 2>nul
mklink /J "%oc%" "!ok!")
echo Done.&set /a oq=0
goto pq
:pq
if !oq! NEQ 0 (call :pw "Failed: !oq!"
if !oq! EQU 3 (call :pw "File or path was not found, use -debug")else if !oq! EQU 1000 (call :pw "Mode is not specified")else if !oq! EQU 1001 (call :pw "Wrong or unknown data in specified mode")else if !oq! EQU 1002 (call :pw "Unsupported version of hMSBuild")else if !oq! EQU 1003 (call :pw "hMSBuild is not found, try -global")else if !oq! EQU 1100 (call :pw "Something went wrong. Try to restore manually: %od%"))
exit/B!oq!
:pa
set "ol=%~1" & set "om=%~2"
call :pv "xcp " ol om
set on=xcopy "%ol%" "%om%" /E/I/Q/H/K/O/X
%on%/B 2>nul>nul || %on% >nul || exit/B1001
exit/B0
:pw
echo   [*] WARN: %~1 >&2
exit/B0
:pv
if defined ou (set "o0=%~1" & echo [ %TIME% ] !o0! !%2! !%3!)
exit/B0
:po
set o1=!%2!
:pb
for /F "tokens=1* delims==" %%a in ("!o1!") do (if "%%~b"=="" (call :pc %1 !o1! %3 & exit/B0)else set o1=%%a E %%b)
goto pb
:pc
set "o2=%~1"
set /a oz=-1
:pd
set /a oz+=1
set %o2%[!oz!]=%~2
set %o2%{!oz!}=%2
if "%~4" NEQ "" shift & goto pd
set %3=!oz!
exit/B0
:pt
set o3=!%1!
set %2=!o3!
exit/B0
:py
set "o4=!%~1! %~2"
call :pv "invoke: " o4
set "o4=!o4! 2^>^&1 ^&call echo %%^^ERRORLEVEL%%"
set /a o5=0
for /F "tokens=*" %%i in ('!o4!') do 2>nul (set /a o5+=1
set msg[!o5!]=%%i
call :pv "# !o5!  : %%i")
if not "%3"=="" set %3=!msg[%o5%]!
exit/B!msg[%o5%]!
:pz
set "%1=!msg[1]!"
exit/B0
