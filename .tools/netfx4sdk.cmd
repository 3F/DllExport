::! netfx4sdk (c) Denis Kuzmin <x-3F@outlook.com> github.com/3F
@echo off&set "tt=%~dp0"&set tu=%*&set/atv=0
setlocal enableDelayedExpansion&if not defined tu goto ui
set tw=!tu:/?=-h!&call :uj uf tw ug&goto uk
:ui
echo.&echo netfx4sdk 2.0.0.1625+f22d500&echo Copyright (c) 2021-2025  Denis Kuzmin ^<x-3F@outlook.com^> github/3F&echo Copyright (c) netfx4sdk contributors&echo.&echo Under the MIT License https://github.com/3F/netfx4sdk&echo.&echo ....&echo Keys&echo.&echo  -mode {value}&echo    * system            - (Recommended) Hack using assemblies for Windows.&echo    * package           - Apply remote package. Read [About modes] below.&echo    * sys               - Alias to 'system'.&echo    * pkg               - Alias to 'package'.&echo    * system-or-package - Fallback to 'system'. Use 'package' if 'system' failed.&echo    * package-or-system - Fallback to 'package'. Use 'system' if 'package' failed.&echo    * sys-or-pkg        - Alias to 'system-or-package'.&echo    * pkg-or-sys        - Alias to 'package-or-system'.&echo.&echo. -tfm {version}&echo    * 4.0 - Process for .NET Framework 4.0 (default)&echo    * 2.0, 3.5, 4.5, 4.6, 4.7, 4.8&echo    * 4.5.1, 4.5.2, 4.6.1, 4.6.2, 4.7.1, 4.7.2, 4.8.1&echo.&echo  -force      - Aggressive behavior when applying etc.&echo  -rollback   - Rollback applied modifications.&echo  -global     - To use the global toolset, like hMSBuild.&echo  -no-mklink  - Use direct copying instead of mklink (junction / symbolic).&echo  -stub       - Use a stub instead of actual processing.&echo  -sdk-root   - Custom path to the SDK root directory.&echo  -no-acl     - Do not copy ownership and ACL information when direct copying.&echo.&echo  -pkg-version {arg} - Specific package version in pkg mode. Where {arg}:&echo      * 1.0.3 ...&echo      * latest - (keyword) To use latest version;&echo.&echo  -debug    - To show debug information.&echo  -version  - Display version of %~nx0.
echo  -help     - Display this help. Aliases: -help -h -?&echo.&echo ...........&echo About modes&echo.&echo '-mode sys' highly recommended because:&echo  [++] All modules are under Windows support.&echo  [+] It does not require internet connection (portable).&echo  [+] No decompression required (faster) compared to package mode.&echo  [-] This is behavior-based hack;&echo      Report or please fix us if something:&echo      https://github.com/3F/netfx4sdk&echo.&echo '-mode pkg' will try to apply remote package to the environment.&echo  [-] Officially dropped support since VS2022.&echo  [-] Requires internet connection to receive ~30 MB via GetNuTool.&echo  [-] Requires decompression of received data to 178 MB before use.&echo  [+] Well known official behavior.&echo.&echo ...................&echo %~n0 -mode sys&echo %~n0 -rollback&echo %~n0 -debug -force -mode package&echo %~n0 -mode pkg -pkg-version 1.0.2&echo %~n0 -mode pkg -tfm 4.5&echo %~n0 -global -mode pkg -tfm 3.5 -no-mklink -force&echo call %~n0 -mode sys ^|^| call %~n0 -mode pkg&echo %~n0 -mode sys-or-pkg&echo %~n0 -mode pkg -tfm 3.5 -sdk-root "path\to" -no-mklink -no-acl&goto ul
:uk
set "tx=1.0.3"&set "ty=2.0 3.5 4.0 4.5 4.6 4.7 4.8 4.5.1 4.5.2 4.6.1 4.6.2 4.7.1 4.7.2 4.8.1"&set "tz=system sys package pkg system-or-package sys-or-pkg package-or-system pkg-or-sys"&set "ta="&set "tb="&set "tc="&set "td="&set "te="&set "tf="&set "tg="&set "th="&set "ti="&set "tj="&set "tk="&set "tl="&set/atm=0
:um
set tn=!uf[%tm%]!&(if [!tn!]==[-help] (goto ui)else if [!tn!]==[-h] (goto ui)else if [!tn!]==[-?] (goto ui ))&(if [!tn!]==[-debug] (set ta=1
goto un)else if [!tn!]==[-mode] (set/a"tm+=1"&call :u0 uf[!tm!] v
call :u1 !v! tz || (echo Mode '!v!' is not allowed. Use one of %tz%>&2&goto u2 )&(if "!v!"=="system" (set "tb=sys")else if "!v!"=="package" (set "tb=pkg")else if "!v!"=="system-or-package" (set "tb=sys-or-pkg")else if "!v!"=="package-or-system" (set "tb=pkg-or-sys")else (set "tb=!v!" ))&(if "!tb!"=="sys-or-pkg" (set "tb=sys"
set "tj=pkg")else if "!tb!"=="pkg-or-sys" (set "tb=pkg"&set "tj=sys"))&goto un)else if [!tn!]==[-rollback] (set tc=1&goto un)else if [!tn!]==[-pkg-version] (set/a"tm+=1"&call :u0 uf[!tm!] v
set tx=!v!&call :u3 "set package version:" v&goto un)else if [!tn!]==[-version] (@echo 2.0.0.1625+f22d500&goto ul)else if [!tn!]==[-no-mklink] (set tf=1&goto un)else if [!tn!]==[-no-acl] (set tl=1&goto un)else if [!tn!]==[-stub] (set ti=1&goto un)else if [!tn!]==[-sdk-root] (set/a"tm+=1"&call :u0 uf[!tm!] v
set "tk=!v!"&(if not defined tk (goto u2 ))&(if not exist "!tk!" (set/atv=1203&goto ul ))&(if not "!tk:~-1!"=="\" (set "tk=!tk!\" ))
goto un)else if [!tn!]==[-global] (set te=1&goto un)else if [!tn!]==[-force] (set td= a forced&goto un)else if [!tn!]==[-tfm] (set/a"tm+=1"&call :u0 uf[!tm!] v
call :u1 !v! ty || (echo Version '!v!' is not allowed. Use one of %ty%>&2&goto u2 )
set "tg=net!v:.=!"&set "th=v!v!"&goto un)else (
:u2
call :u4 "Invalid key or value for '!tn!'"&set/atv=1200&goto ul ))
:un
set/a"tm+=1"&if %tm% LSS !ug! goto um
:u5
call :u6 4.0&(if defined tc (call :u3 "activated!td! rollback")else (call :u3 "run!td! action:" tb))
set "t0=%ProgramFiles(x86)%"&if not exist "!t0!" set "t0=%ProgramFiles%"
(if defined tk (set "t0=!tk!")else (set "t0=!t0!\Reference Assemblies\Microsoft\Framework\.NETFramework\"))
set "t1=!t0!!th!"&set "t2=!t1!.%~nx0"&call :u3 "!tg! !th!" t1&(if defined tc ((if not exist "!t2!" if not defined td (echo There's nothing to rollback.
if exist "!t1!" echo Use '-force' key to delete !tg! without restrictions.
goto ul))&call :u7 "rmdir" /Q/S "!t1!" 2>nul&(if exist "!t2!" (call :u3 "ren " t2 th
(call :u7 "ren" "!t2!" !th! 2>nul ) || (set/atv=1100&goto ul )))
echo Rollback completed.&goto ul))&(if exist "!t2!" (echo %~nx0 has already been applied before. There's nothing to do anymore.
echo Use '-rollback' key to re-apply with another mode if needed.&exit/B0))&(if exist "!t1!\mscorlib.dll" ((if not defined td (echo The Developer Pack was found successfully. There's nothing to do here at all.
echo Use '-force' key to suppress the restriction if you really know what you're doing.&set/atv=0&goto ul))
call :u3 "Suppress found SDK " t1))&(if not defined tb (set/atv=1000&goto ul ))&(if defined te (set "t3=hMSBuild")else (set t3="%~dp0hMSBuild" ))
call :u8 t3 "-version" || (set/atv=1003&goto ul )&(call :u9 uh&call :uo uh 2,4,0 ) || (set/atv=1002&goto ul)&(if not defined tf (mklink 2>nul>nul&(if !ERRORLEVEL! EQU 9009 (call :u4 "'-no-mklink' is activated because links are not supported in this environment."
set "tf=2"))))
:up
call :u3 "kNoMklink " tf&(if "!tb!"=="sys" ((if not "!th!"=="v4.0" (set/atv=1202&goto ul))
echo Applying hack using assemblies for Windows ...&call :u8 t3 "-no-less-4 -no-vswhere -no-vs -only-path -notamd64"&set/atv=!ERRORLEVEL!&if !tv! NEQ 0 goto ul
call :u9 t4&call :u7 "mkdir" "!t1!" 2>nul&call :uq "!t1!" "!t2!" + || (set/atv=1004&goto ul)
set t4=!t4:MSBuild.exe=!&set t4=!t4:\\=\!&call :u3 "lDir " t4&(if not exist "!t4!" (set/atv=3&goto ul ))
for /F "tokens=*" %%i in ('dir /B "!t4!*.dll"') do call :ur "!t4!%%i" "!t1!\%%i"
for /F "tokens=*" %%i in ('dir /B "!t4!WPF\*.dll"') do call :ur "!t4!WPF\%%i" "!t1!\%%i"
set "t5=!t1!\RedistList"&call :u7 "mkdir" "!t5!" 2>nul
set t6=^<?xml version="1.0" encoding="utf-8"?^>^<FileList Redist="Microsoft-Windows-CLRCoreComp.4.0" Name=".NET Framework 4" RuntimeVersion="4.0" ToolsVersion="4.0" /^>&call :us t6 "!t5!\FrameworkList.xml"&set "t5=!t1!\PermissionSets"&call :u7 "mkdir" "!t5!" 2>nul
set t6=^<PermissionSet version="1" class="System.Security.PermissionSet" Unrestricted="true" /^>&call :us t6 "!t5!\FullTrust.xml")else if "!tb!"=="pkg" (set t7=Microsoft.NETFramework.ReferenceAssemblies.!tg!&echo Applying .NET Framework !th! package ...&set t8=%~nx0.!tg!.%tx%&(if "%tx%"=="latest" (set "tx=")else (set "tx=/%tx%" ))
set t9=!t3!&if defined ta set t9=!t3! -debug
call !t9! -GetNuTool "!t7!!tx!:!t8!" || (set/atv=1400&goto ul)
set "to=packages\!t8!\build\.NETFramework\!th!"&call :u3 "dpkg " to&(if not exist "!to!" (set/atv=1001&goto ul))
call :u7 "mkdir" "!t1!" 2>nul&call :u7 "ren" "!t1!" !th!.%~nx0 2>nul || (set/atv=1004&goto ul)
call :vt "!to!" "!t1!"))&set/atv=!ERRORLEVEL!&goto ul
:ul
(if !tv! EQU 0 (echo Done.
exit/B0))&call :u4 "Failed: !tv!"&set "tp=https://github.com/3F/hMSBuild"&(if !tv! EQU 3 (call :u4 "File or path was not found, use -debug")else if !tv! EQU 1000 (call :u4 "Mode '-mode' is not specified, use -help")else if !tv! EQU 1001 (call :u4 "Wrong or unknown data in the specified '-mode !tb!'")else if !tv! EQU 1002 (call :u4 "Unsupported hMSBuild version !uh!, update !tp!")else if !tv! EQU 1003 (call :u4 "hMSBuild is not found. Try -global key or visit !tp!")else if !tv! EQU 1100 (call :u4 "Something went wrong. Try to restore manually: !t2!")else if !tv! EQU 1202 (call :u4 ".NET Framework !th! is not supported in the selected '-mode !tb!'")else if !tv! EQU 1400 (call :u4 "Failed network or there are no permissions to complete '-mode !tb!'")else if !tv! EQU 1004 (call :u4 "Unauthorized access. Make sure you have read/write permissions to the folders listed in '-debug'. Try run %~nx0 as administrator or use '-no-acl'")else if !tv! EQU 1203 (call :u4 "The path specified in '-sdk-root' does not exist: !tk!. Try as -sdk-root `!tk!` or make sure.")else if !tv! EQU 1005 (call :u4 "Failed XCOPY. Try '-no-acl' key to avoid `Access denied` errors; Use '-debug' / '-stub' for details.")else if !tv! EQU 1200 (exit/B!tv!)else (call :u4 "Something went wrong. Use '-debug' and/or '-stub' keys to inspect the problem."))&(if defined tj if defined th (echo.&echo Switch to '!tj!' mode for second attempt due to '-mode !tb!-or-!tj!'
set "tb=!tj!" & set "tj="
goto up))&exit/B!tv!
:uq
(if not defined ti if not defined ta (call :vu "%~1" "%~2" "%~3" >nul
exit/B))&call :vu "%~1" "%~2" "%~3"&exit/B
:vu
(set "tq=%~1" & set "tr=%~2" & set "ts=%~3" )&(if defined ts (set "ts=/E")else (set "ts=" ))&(if not defined ti (call :u3 "xcp !ts!" tq tr ))&(if defined tl (set "ut=")else (set "ut=/O/X" ))
set uu=xcopy "%tq%" "%tr%" !ts!/I/Q/H/K!ut!/Y
(if defined tf (call :u7 !uu! || exit/B1005)else (call :u7 !uu!/B 2>nul || call :u7 !uu! || exit/B1005))
exit/B
:uo
(for /F "tokens=1,2,3 delims=." %%a in ("!%~1!") do (if %%a LSS %~2 exit/B1
(if %%a EQU %~2 (if %%b LSS %~3 exit/B1
if %%b EQU %~3 if %%c LSS %~4 exit/B1))))&exit/B0
:ur
(if not defined ti if not defined ta (call :vv "%~1" "%~2" >nul
exit/B))&call :vv "%~1" "%~2"&exit/B
:vv
(if defined tf (call :uq "%~1" "%~2*")else (call :u7 "mklink" "%~2" "%~1"))
exit/B
:vt
(if defined tf (call :uq "%~1" "%~2" +)else (call :u7 "mklink" /J "%~2" "%~1"))
exit/B
:u6
set "uv=%~1"&if not defined tg set "tg=net!uv:.=!"
if not defined th set "th=v!uv!"
exit/B0
:u1
(for %%t in (!%~2!) do (if "%~1"=="%%t" exit/B0))&exit/B1
:u4
set uw=%~1&set uw=!uw:`="!
echo   [*] WARN: !uw! >&2
exit/B0
:u3
(if defined ta (set "ux=%~1" &echo [ %TIME% ] !ux! !%2! !%3!))
exit/B0
:uj
set uy=!%2!
:vw
(for /F "tokens=1* delims==" %%a in ("!uy!") do (if "%%~b"=="" (call :vx %1 !uy! %3&exit/B0)else set uy=%%a E %%b))&goto vw
:vx
set "uz=%~1"&set/atm=-1
:vy
set/atm+=1&set %uz%[!tm!]=%~2&set %uz%{!tm!}=%2&if "%~4" NEQ "" shift&goto vy
set %3=!tm!&exit/B0
:u0
set ua=!%1!
if not defined ua set %2=&exit/B0
set %2=!ua!&exit/B0
:u8
set "ub=!%~1! %~2"&call :u3 "invoke: " ub&set "ub=!ub! 2^>^&1 ^&call echo %%^^ERRORLEVEL%%"&set/auc=0&for /F "tokens=*" %%i in ('!ub!') do 2>nul (set/auc+=1
set msg[!uc!]=%%i&call :u3 "# !uc!  : %%i")&if not "%3"=="" set %3=!msg[%uc%]!
exit/B!msg[%uc%]!
:u9
set "%1=!msg[1]!"&exit/B0
:u7
(set "ud=%~1" & shift )
set ue=%1 %2 %3 %4 %5 %6 %7 %8 %9&(if not defined ti (call !ud! !ue!&exit/B ))
echo !ud! !ue!&exit/B0
:us
(if defined ti (echo !%~1!^> "%~2")else (echo !%~1!> "%~2" ))
exit/B0
