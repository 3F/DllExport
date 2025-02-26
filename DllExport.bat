::! https://github.com/3F/DllExport
@echo off
set "oo=%~dpnx0"&set op=%*
set oq=%*
(if defined op ((if defined __p_call (set oq=%oq:^^=^%)else (set op=%op:^=^^%))))
set wMgrArgs=%oq%&set or=%op:!= L %
set or=%or:^= T %
setlocal enableDelayedExpansion&set "os=^"&set "or=!or:%%=%%%%!"&set "or=!or:&=%%os%%&!"
set "ot=1.8.0"&set "wAction=Configure"&set "ou=DllExport"&set "ov=tools/net.r_eg.DllExport.Wizard.targets"&set "ow=packages"&set "ox=https://www.nuget.org/api/v2/package/"&set "oy=build_info.txt"&set "oz=!oo!"&set "wRootPath=%~dp0"&set /a wDxpOpt=0&set "oa="&set "ob="&set "oc="&set "od="&set "oe="&set "of="&set "og="&set "oh="&set "oi="&set "oj="&set /a ok=0&(if not defined op (if defined wAction goto pr
goto ps))&set or=!or:/?=/h!&call :inita pp or pq&goto pt
:ps
echo.&echo .NET DllExport 1.8.0.17264+f4deb0e&echo Copyright (c) 2009-2015  Robert Giesecke&echo Copyright (c) 2016-2025  Denis Kuzmin ^<x-3F@outlook.com^> github/3F&echo.&echo MIT License&echo https://github.com/3F/DllExport&echo.&echo.&echo Usage: DllExport [keys] or built-in [-GetNuTool ... or -hMSBuild ...]&echo.&echo Keys&echo ----&echo -action {type} - Specified action for Wizard. Where {type}:&echo   * Configure - To configure DllExport for specific projects.&echo   * Update    - To update pkg reference for already configured projects.&echo   * Restore   - To restore configured DllExport.&echo   * Export    - To export configured projects data.&echo   * Recover   - To re-configure projects using predefined data.&echo                `RecoverInit` to initial setup.&echo   * Unset     - To unset all data from specified projects.&echo   * Upgrade   - Aggregates an Update action with additions for upgrading.&echo.&echo -sln-dir {path}    - Path to directory with .sln files to be processed.&echo -sln-file {path}   - Optional predefined .sln file to be processed.&echo -metalib {path}    - Relative path to meta library.&echo -metacor {path}    - Relative path to meta core library.&echo -dxp-target {path} - Relative path to entrypoint wrapper of the main core.&echo -dxp-version {num} - Specific version of %~n0. Where {num}:&echo   * Versions: 1.7.4 ...&echo   * Keywords:&echo     `actual` - Unspecified local/latest remote version;&echo                ( Only if you know what you are doing )&echo.&echo -msb {path}           - Full path to specific MSBuild Tools.&echo -hMSBuild {args}      - Access to hMSBuild (built-in) https://github.com/3F/hMSBuild&echo -packages {path}      - A common directory for packages.
echo -server {url}         - Url for searching remote packages.&echo -proxy {cfg}          - To use proxy. The format: [usr[:pwd]@]host[:port]&echo -pkg-link {uri}       - Direct link to package from the source via specified URI.&echo -force                - Aggressive behavior, e.g. like removing pkg when updating.&echo -no-mgr               - Do not use %~n0 for automatic restore the remote package.&echo -mgr-up               - Updates %~n0 to version from '-dxp-version'.&echo -wz-target {path}     - Relative path to entrypoint wrapper of the main wizard.&echo -pe {args}            - To work with PE32/PE32+ module. -pe -help&echo -eng                  - Try to use english language for all build messages.&echo -GetNuTool {args}     - Access to GetNuTool (built-in) https://github.com/3F/GetNuTool&echo -debug                - To show additional information.&echo -version              - Displays version for which (together with) it was compiled.&echo -build-info           - Displays actual build information from selected %~n0.&echo -help                 - Displays this help. Aliases: -help -h /h -? /?&echo.&echo Flags&echo -----&echo  __p_call - To use the call-type logic when invoking %~n0&echo.&echo Samples&echo -------&echo %~n0 -action Configure -force -pkg-link https://host/v1.7.4.nupkg&echo %~n0 -action Restore -sln-file "Conari.sln"&echo %~n0 -proxy guest:1234@10.0.2.15:7428 -action Configure&echo %~n0 -pe -list-all -hex -i bin\regXwild.dll&echo.&echo %~n0 -mgr-up -dxp-version 1.7.4&echo %~n0 -action Upgrade -dxp-version 1.7.4&echo.&echo %~n0 -GetNuTool "Conari;regXwild;Fnv1a128"&echo %~n0 -hMSBuild ~x ~c Release&echo %~n0 -GetNuTool vsSolutionBuildEvent/1.16.1:../SDK ^& SDK\GUI
goto pu
:pt
set /a ol=0
:pv
set om=!pp[%ol%]!&(if [!om!]==[-help] (goto ps)else if [!om!]==[-h] (goto ps)else if [!om!]==[-?] (goto ps)else if [!om!]==[/h] (goto ps ))&(if [!om!]==[-debug] (set oa=1
goto pw)else if [!om!]==[-action] (set /a "ol+=1" & call :px pp[!ol!] v
set wAction=!v!&(for %%g in (Restore, Configure, Update, Export, Recover, RecoverInit, Unset, Upgrade, Default) do (if /I "!v!"=="%%g" goto pw))&echo Unknown -action !v!&exit/B1)else if [!om!]==[-sln-dir] (set /a "ol+=1" & call :px pp[!ol!] v
set wSlnDir=!v!&goto pw)else if [!om!]==[-sln-file] (set /a "ol+=1" & call :px pp[!ol!] v
set wSlnFile=!v!&goto pw)else if [!om!]==[-metalib] (set /a "ol+=1" & call :px pp[!ol!] v
set wMetaLib=!v!&goto pw)else if [!om!]==[-metacor] (set /a "ol+=1" & call :px pp[!ol!] v
set wMetaCor=!v!&goto pw)else if [!om!]==[-dxp-target] (set /a "ol+=1" & call :px pp[!ol!] v
set wDxpTarget=!v!&goto pw)else if [!om!]==[-dxp-version] (set /a "ol+=1" & call :px pp[!ol!] v
set ot=!v!&goto pw)else if [!om!]==[-msb] (set /a "ol+=1" & call :px pp[!ol!] v
set oc=!v!&goto pw)else if [!om!]==[-packages] (set /a "ol+=1" & call :px pp[!ol!] v
set ow=!v!&goto pw)else if [!om!]==[-server] (set /a "ol+=1" & call :px pp[!ol!] v
set ox=!v!&goto pw)else if [!om!]==[-proxy] (set /a "ol+=1" & call :px pp[!ol!] v
set oh=!v!&set wProxy=!v!&goto pw)else if [!om!]==[-pkg-link] (set /a "ol+=1" & call :px pp[!ol!] v
set od=!v!&set ot=!om!&goto pw)else if [!om!]==[-force] (set of=1&goto pw)else if [!om!]==[-no-mgr] (set /a wDxpOpt^|=1
goto pw)else if [!om!]==[-mgr-up] (set og=1&goto pw)else if [!om!]==[-wz-target] (set /a "ol+=1" & call :px pp[!ol!] v
set ov=!v!&goto pw)else if [!om!]==[-pe-exp-list] (set /a "ol+=1" & call :px pp[!ol!] v
set oe=-list -pemodule "!v!"&goto pw)else if [!om!]==[-eng] (chcp 437 >nul&goto pw)else if [!om!]==[-GetNuTool] (call :py -GetNuTool 10&set /a ok=!ERRORLEVEL! & goto pu)else if [!om!]==[-hMSBuild] (set oj=1
call :py -hMSBuild 9&set /a ok=!ERRORLEVEL! & goto pu)else if [!om!]==[-pe] (set oe=1
goto pr)else if [!om!]==[-version] (@echo 1.8.0.17264+f4deb0e  %__dxp_pv%&goto pu)else if [!om!]==[-build-info] (set ob=1&goto pw)else if [!om!]==[-tests] (set /a "ol+=1" & call :px pp[!ol!] v
set oi=!v!&goto pw)else (echo Incorrect key: !om!&set /a ok=1&goto pu))
:pw
set /a "ol+=1" & if %ol% LSS !pq! goto pv
:pr
call :pz "dxpName = " ou&call :pz "dxpVersion = " ot&call :pz "-sln-dir = " wSlnDir&call :pz "-sln-file = " wSlnFile&call :pz "-metalib = " wMetaLib&call :pz "-metacor = " wMetaCor&call :pz "-dxp-target = " wDxpTarget&call :pz "-wz-target = " ov&call :pz "#opt " wDxpOpt&(if defined ot ((if "!ot!"=="actual" (set "ot="))))
set wPkgVer=!ot!&(if z%wAction%==zUpgrade (call :pz "Upgrade is on"
set og=1&set of=1))&call :pa ow&set "ow=!ow!\\"&set "on=!ou!"&set "wPkgPath=!ow!!ou!"&(if defined ot (set "on=!on!/!ot!"
set "wPkgPath=!wPkgPath!.!ot!"))&(if defined of ((if exist "!wPkgPath!" (call :pz "Removing the old version. '-force' key rule. " wPkgPath
rmdir /S/Q "!wPkgPath!"))))&set o0="!wPkgPath!\\!ov!"&call :pz "wPkgPath = " wPkgPath&(if not exist !o0! ((if exist "!wPkgPath!" (call :pz "Trying to replace obsolete version ... " wPkgPath
rmdir /S/Q "!wPkgPath!"))&call :pz "-pkg-link = " od&call :pz "-server = " ox&(if defined od (set ox=!od!
(if "!ox::=!"=="!ox!" (set ox=!cd!/!ox!))&(if "!wPkgPath::=!"=="!wPkgPath!" (set "on=:../!wPkgPath!")else (set "on=:!wPkgPath!"))))&(if defined oc (set msb.gnt.cmd=!oc!))
set o1=-GetNuTool "!on!" /p:ngserver="!ox!" /p:ngpath="!ow!" /p:proxycfg="!oh!"&call :pb o1 "no"))&(if defined oe (if !oe! NEQ 1 set or=-pe!oe!
call :py -pe 3&set /a ok=!ERRORLEVEL! & goto pu))&(if defined ob (call :pz "buildInfo = " wPkgPath oy
(if not exist "!wPkgPath!\\!oy!" (echo information about build is not available.
set /a ok=2&goto pu))&type "!wPkgPath!\\!oy!"
goto pu))&call :pz "wRootPath = " wRootPath&call :pz "wAction = " wAction&call :pz "wMgrArgs = " wMgrArgs&call :pz "wzTarget = " o0&set o2=/nologo /noautorsp&(if not defined oi ((if not exist !o0! (echo Target cannot be initialized. Try to use another keys.
set /a ok=2&goto pu))&(if defined oc (call :pz "Use specific MSBuild Tools: " oc
(if not exist "!oc!" (echo MSBuild Tools was not found. Check -msb key.>&2
set /a ok=2&goto pu))&call "!oc!" !o0! !o2! /v:m /m:4)else (set o3=~x -cs !o0! !o2!&call :pb o3))))&(if !ERRORLEVEL! NEQ 0 (echo Something went wrong. Use `-debug` key for details.
set /a ok=2))
:pu
(if defined oi (echo Running Tests ... "!oi!"
set o3=~x -cs "!oi!" !o2!&call :pb o3&exit/B!ERRORLEVEL!))&(if defined og ((copy /B/Y "!wPkgPath!\\DllExport.bat" "!oz!" > nul) && ( echo Manager has been updated. & exit/B0 ) || ( (echo -mgr-up failed:!ok! 1>&2) & exit/B1 )))
exit/B!ok!
:px
call :eva %*&exit/B
:py
set om=%~1&set /a o4=%~2&call :pz "accessing to !om! ..."
(for /L %%p in (0,1,8181)do (if "!or:~%%p,%o4%!"=="!om!" (set o5=!or:~%%p!
set o6=!o5:~%o4%!&(if defined oj (call :pb o6)else if defined oe (call "!wPkgPath!\\tools\\PeViewer" !o6!)else (set o6=-GetNuTool !o6!
call :pb o6))&exit/B!ERRORLEVEL!)))&call :pz "!om! is corrupted: " or&exit/B1
:pb
set o7=!%~1!&if not defined logo set "logo=%~2"
call :pz "invoke a built-in hMSBuild: " o7 logo&(if defined oj (if defined oa set o7=-debug !o7!))
call :pc !o7!&exit/B!ERRORLEVEL!
:pz
(if defined oa (set "o8=%~1" & echo [ %TIME% ] !o8! !%2! !%3!))
exit/B0
:pa
call :pd %1&call :pe %1&exit/B0
:pd
call :pf %1 "-=1"&exit/B0
:pe
call :pf %1 "+=1"&exit/B0
:pf
set o9=z!%1!z&(if "%~2"=="-=1" (set "po=1")else (set "po="))&(if defined po (set /a "i=-2")else (set /a "i=1"))
:pg
(if "!o9:~%i%,1!"==" " (set /a "i%~2"
goto pg))&if defined po set /a "i+=1"
(if defined po (set "%1=!o9:~1,%i%!")else (set "%1=!o9:~%i%,-1!"))
exit/B0
:pc
setlocal disableDelayedExpansion&set _kH=%~n0 -hMSBuild
::! hMSBuild 2.4.1.19080+57a01fe
::! Copyright (c) 2017-2024  Denis Kuzmin <x-3F@outlook.com> github/3F
::! Copyright (c) hMSBuild contributors https://github.com/3F/hMSBuild
set "hh=%~dp0"&set hi=%*&if not defined hi setlocal enableDelayedExpansion & goto i0
if not defined __p_call set hi=%hi:^=^^%
set hj=%hi:!= L %
set hj=%hj:^= T %
setlocal enableDelayedExpansion&set "hk=^"&set "hj=!hj:%%=%%%%!"&set "hj=!hj:&=%%hk%%&!"
:i0
set "hl=2.8.4"&set hm=%temp%\hMSBuild_vswhere&set "hn="&set "ho="&set "hp="&set "hq="&set "hr="&set "hs="&set "ht="&set "hu="&set "hv="&set "hw="&set "hx="&set "hy="&set "hz="&set "ha="&set "hb="&set /a hc=0&if not defined hi goto i1
set hj=!hj:/?=/h!&call :inita ic hj id&goto i2
:i3
echo.&echo hMSBuild 2.4.1.19080+57a01fe&echo Copyright (c) 2017-2024  Denis Kuzmin ^<x-3F@outlook.com^> github/3F&echo Copyright (c) hMSBuild contributors https://github.com/3F/hMSBuild&echo.&echo Under the MIT License https://github.com/3F/hMSBuild&echo.&echo Syntax: %_kH% [keys to %_kH%] [keys to MSBuild.exe or GetNuTool]&echo.&echo Keys&echo ~~~~&echo  -no-vs        - Disable searching from Visual Studio.&echo  -no-netfx     - Disable searching from .NET Framework.&echo  -no-vswhere   - Do not search via vswhere.&echo  -no-less-15   - Do not include versions less than 15.0 (install-API/2017+)&echo  -no-less-4    - Do not include versions less than 4.0 (Windows XP+)&echo.&echo  -priority {IDs} - 15+ Non-strict components preference: C++ etc.&echo                    Separated by space "a b c" https://aka.ms/vs/workloads&echo.&echo  -vswhere {v}&echo   * 2.6.7 ...&echo   * latest - To get latest remote vswhere.exe&echo   * local  - To use only local&echo             (.bat;.exe /or from +15.2.26418.1 VS-build)&echo.&echo  -no-cache         - Do not cache vswhere for this request.&echo  -reset-cache      - To reset all cached vswhere versions before processing.&echo  -cs               - Adds to -priority C# / VB Roslyn compilers.&echo  -vc               - Adds to -priority VC++ toolset.&echo  ~c {name}         - Alias to p:Configuration={name}&echo  ~p {name}         - Alias to p:Platform={name}&echo  ~x                - Alias to m:NUMBER_OF_PROCESSORS-1 v:m&echo  -notamd64         - To use 32bit version of found msbuild.exe if it's possible.&echo  -stable           - It will ignore possible beta releases in last attempts.&echo  -eng              - Try to use english language for all build messages.
echo  -GetNuTool {args} - Access to GetNuTool core. https://github.com/3F/GetNuTool&echo  -only-path        - Only display fullpath to found MSBuild.&echo  -force            - Aggressive behavior for -priority, -notamd64, etc.&echo  -vsw-as "args..." - Reassign default commands to vswhere if used.&echo  -debug            - To show additional information from %_kH%&echo  -version          - Display version of %_kH%.&echo  -help             - Display this help. Aliases: -? -h&echo.&echo.&echo MSBuild switches&echo ~~~~~~~~~~~~~~~~&echo   /help or /? or /h&echo   Use /... if %_kH% overrides some -... MSBuild switches&echo.&echo.&echo Try to execute:&echo   %_kH% -only-path -no-vs -notamd64 -no-less-4&echo   %_kH% -debug ~x ~c Release&echo   %_kH% -GetNuTool "Conari;regXwild;Fnv1a128"&echo   %_kH% -GetNuTool vsSolutionBuildEvent/1.16.1:../SDK ^& SDK\GUI
echo   %_kH% -cs -no-less-15 /t:Rebuild&goto i4
:i2
set "hd="&set /a he=0
:i5
set hf=!ic[%he%]!&(if [!hf!]==[-help] (goto i3)else if [!hf!]==[-h] (goto i3)else if [!hf!]==[-?] (goto i3 ))&(if [!hf!]==[-nocachevswhere] (call :i6 -nocachevswhere -no-cache -reset-cache
set hf=-no-cache)else if [!hf!]==[-novswhere] (call :i6 -novswhere -no-vswhere&set hf=-no-vswhere)else if [!hf!]==[-novs] (call :i6 -novs -no-vs&set hf=-no-vs)else if [!hf!]==[-nonet] (call :i6 -nonet -no-netfx&set hf=-no-netfx)else if [!hf!]==[-vswhere-version] (call :i6 -vswhere-version -vswhere&set hf=-vswhere)else if [!hf!]==[-vsw-version] (call :i6 -vsw-version -vswhere&set hf=-vswhere)else if [!hf!]==[-vsw-priority] (call :i6 -vsw-priority -priority&set hf=-priority))&(if [!hf!]==[-debug] (set ht=1
goto i7)else if [!hf!]==[-GetNuTool] (call :i8 "accessing to GetNuTool ..."&(for /L %%p in (0,1,8181)do (if "!hj:~%%p,10!"=="-GetNuTool" (set hg=!hj:~%%p!
call :i9 !hg:~10!&set /a hc=!ERRORLEVEL!&goto i4)))&call :i8 "!hf! is corrupted: " hj&set /a hc=1&goto i4)else if [!hf!]==[-no-vswhere] (set hq=1&goto i7)else if [!hf!]==[-no-cache] (set hr=1&goto i7)else if [!hf!]==[-reset-cache] (set hs=1&goto i7)else if [!hf!]==[-no-vs] (set ho=1&goto i7)else if [!hf!]==[-no-less-15] (set ha=1&set hp=1&goto i7)else if [!hf!]==[-no-less-4] (set hb=1&goto i7)else if [!hf!]==[-no-netfx] (set hp=1&goto i7)else if [!hf!]==[-notamd64] (set hn=1&goto i7)else if [!hf!]==[-only-path] (set hu=1&goto i7)else if [!hf!]==[-eng] (chcp 437 >nul&goto i7)else if [!hf!]==[-vswhere] (set /a "he+=1" & call :jh ic[!he!] v
set hl=!v!&call :i8 "selected vswhere version:" v&set hv=1&goto i7)else if [!hf!]==[-version] (echo 2.4.1.19080+57a01fe&goto i4)else if [!hf!]==[-priority] (set /a "he+=1" & call :jh ic[!he!] v
set hw=!v! !hw!&goto i7)else if [!hf!]==[-vsw-as] (set /a "he+=1" & call :jh ic[!he!] v
set hx=!v!&goto i7)else if [!hf!]==[-cs] (set hw=Microsoft.VisualStudio.Component.Roslyn.Compiler !hw!&goto i7)else if [!hf!]==[-vc] (set hw=Microsoft.VisualStudio.Component.VC.Tools.x86.x64 !hw!&goto i7)else if [!hf!]==[~c] (set /a "he+=1" & call :jh ic[!he!] v
set hd=!hd! /p:Configuration="!v!"&goto i7)else if [!hf!]==[~p] (set /a "he+=1" & call :jh ic[!he!] v
set hd=!hd! /p:Platform="!v!"&goto i7)else if [!hf!]==[~x] (set /a h0=NUMBER_OF_PROCESSORS - 1&set hd=!hd! /v:m /m:!h0!&goto i7)else if [!hf!]==[-stable] (set hy=1&goto i7)else if [!hf!]==[-force] (set hz=1&goto i7)else (call :i8 "non-handled key: " ic{%he%}&set hd=!hd! !ic{%he%}!))
:i7
set /a "he+=1" & if %he% LSS !id! goto i5
:i1
(if defined hs (call :i8 "resetting vswhere cache"
rmdir /S/Q "%hm%" 2>nul))&(if not defined hq if not defined ho (call :ji ie
if defined ie goto jj))&(if not defined ho if not defined ha (call :jk ie
if defined ie goto jj))&(if not defined hp (call :jl ie
if defined ie goto jj))
echo MSBuild tools was not found. Use `-debug` key for details.>&2
set /a hc=2&goto i4
:jj
(if defined hu (echo !ie!
goto i4))&set h1="!ie!"&echo hMSBuild: !h1!&if not defined hd goto jm
set hd=%hd: T =^%
set hd=%hd: L =^!%
set hd=!hd: E ==!
:jm
call :i8 "Arguments: " hd&!h1! !hd!
set /a hc=%ERRORLEVEL%&goto i4
:i4
exit/B!hc!
:eva
call :jh %*&exit/B
:ji
call :i8 "try vswhere..."&(if defined hv if not "!hl!"=="local" (call :jn h8 h2
call :jo h8 if h2&set %1=!if!&exit/B0))&call :jp h8&set "h2="&(if not defined h8 ((if "!hl!"=="local" (set "%1=" & exit/B2))
call :jn h8 h2))&call :jo h8 if h2&set %1=!if!&exit/B0
:jp
set h3=!hh!vswhere&call :jq h3 ig&if defined ig set "%1=!h3!" & exit/B0
set h4=Microsoft Visual Studio\Installer&if exist "%ProgramFiles(x86)%\!h4!" set "%1=%ProgramFiles(x86)%\!h4!\vswhere" & exit/B0
if exist "%ProgramFiles%\!h4!" set "%1=%ProgramFiles%\!h4!\vswhere" & exit/B0
call :i8 "local vswhere is not found."&set "%1="&exit/B3
:jn
(if defined hr (set h5=!hm!\_mta\%random%%random%vswhere)else (set h5=!hm!
(if defined hl (set h5=!h5!\!hl!))))
call :i8 "tvswhere: " h5&(if "!hl!"=="latest" (set h6=vswhere)else (set h6=vswhere/!hl!))
set h7="!h6!:vswhere" /p:ngpath="!h5!"&call :i8 "GetNuTool call: " h7&setlocal&set __p_call=1&(if defined ht (call :i9 !h7!)else (call :i9 !h7! >nul))
endlocal&set "%1=!h5!\vswhere\tools\vswhere"&set "%2=!h5!"&exit/B0
:jo
set "h8=!%1!"&set "h9=!%3!"&call :jq h8 h8&(if not defined h8 (call :i8 "vswhere tool does not exist"
set "%2=" & exit/B1))
call :i8 "vswbin: " h8&set "ih="&set "ii="&set ij=!hw!&if not defined hx set hx=-products * -latest
call :i8 "assign command: `!hx!`"
:jr
call :i8 "attempts with filter: !ij!; `!ih!`"&set "ik=" & set "il="
(for /F "usebackq tokens=1* delims=: " %%a in (`"!h8!" -nologo !ih! -requires !ij! Microsoft.Component.MSBuild !hx!`) do (if /I "%%~a"=="installationPath" set ik=%%~b
if /I "%%~a"=="installationVersion" set il=%%~b
(if defined ik if defined il (call :js ik il ii
if defined ii goto jt
set "ik=" & set "il="))))&(if not defined hy if not defined ih (set ih=-prerelease
goto jr))&(if defined ij (set im=Tools was not found for: !ij!
(if defined hz (call :i8 "Ignored via -force. !im!"
set "ii=" & goto jt))
call :ju "!im!"&set "ij=" & set "ih="
goto jr))
:jt
(if defined h9 if defined hr (call :i8 "reset vswhere " h9
rmdir /S/Q "!h9!"))&set %2=!ii!&exit/B0
:js
set ik=!%1!&set il=!%2!&call :i8 "vspath: " ik&call :i8 "vsver: " il&(if not defined il (call :i8 "nothing to see via vswhere"
set "%3=" & exit/B3))
(for /F "tokens=1,2 delims=." %%a in ("!il!") do (set il=%%~a.0))&if !il! geq 16 set il=Current
if not exist "!ik!\MSBuild\!il!\Bin" set "%3=" & exit/B3
set in=!ik!\MSBuild\!il!\Bin&call :i8 "found path via vswhere: " in&(if exist "!in!\amd64" (call :i8 "found /amd64"
set in=!in!\amd64))&call :jv in in&set %3=!in!&exit/B0
:jk
call :i8 "Searching from Visual Studio - 2015, 2013, ..."&(for %%v in (14.0,12.0)do (call :jw %%v Y & (if defined Y (set %1=!Y!
exit/B0))))&call :i8 "-vs: not found"&set "%1="&exit/B0
:jl
call :i8 "Searching from .NET Framework - .NET 4.0, ..."&(for %%v in (4.0,3.5,2.0)do (call :jw %%v Y & (if defined Y (set %1=!Y!
exit/B0)else if defined hb (goto :jx ))))
:jx
call :i8 "-netfx: not found"&set "%1="&exit/B0
:jw
call :i8 "check %1"&(for /F "usebackq tokens=2* skip=2" %%a in (`reg query "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSBuild\ToolsVersions\%1" /v MSBuildToolsPath 2^> nul`) do (if exist %%b (set in=%%~b
call :i8 ":msbfound " in&call :jv in if&set %2=!if!&exit/B0)))&set "%2="&exit/B0
:jv
set in=!%~1!\MSBuild.exe&if exist "!in!" set "%2=!in!"
(if not defined hn (exit/B0))
set io=!in:Framework64=Framework!&set io=!io:\amd64=!&(if exist "!io!" (call :i8 "Return 32bit version because of -notamd64 key."
set %2=!io!&exit/B0))&(if defined hz (call :i8 "Ignored via -force. Only 64bit version was found for -notamd64"
set "%2=" & exit/B0))
if not "%2"=="" call :ju "Return 64bit version. Found only this."
exit/B0
:jq
call :i8 "bat/exe: " %1&if exist "!%1!.bat" set %2="!%1!.bat" & exit/B0
if exist "!%1!.exe" set %2="!%1!.exe" & exit/B0
set "%2="&exit/B0
:i6
call :ju "'%~1' is obsolete. Use: %~2 %~3"&exit/B0
:ju
echo   [*] WARN: %~1 >&2
exit/B0
:i8
(if defined ht (set "ip=%~1" & echo [ %TIME% ] !ip! !%2! !%3!))
exit/B0
:inita
set iq=!%2!&set iq=!iq:""=!
:jy
(for /F "tokens=1* delims==" %%a in ("!iq!") do (if "%%~b"=="" (call :jz %1 !iq! %3 & exit/B0)else set iq=%%a E %%b))&goto jy
:jz
set "ir=%~1"&set /a he=-1
:ja
set /a he+=1&set %ir%[!he!]=%~2&set %ir%{!he!}=%2&if "%~4" NEQ "" shift & goto ja
set %3=!he!&exit/B0
:jh
set is=!%1!
set "is=%is: T =^%"
set "is=%is: L =^!%"
set is=!is: E ==!
set %2=!is!&exit/B0
:i9
setlocal disableDelayedExpansion
::! GetNuTool /shell/batch edition
::! Copyright (c) 2015-2024  Denis Kuzmin <x-3F@outlook.com> github/3F
::! https://github.com/3F/GetNuTool
set it=gnt.core&set iu="%temp%\%it%1.9.0%random%%random%"&if "%~1"=="-unpack" goto jb
if "%~1"=="-msbuild" goto jc
set iv=%*&setlocal enableDelayedExpansion&set "iw=%~1 "&set ix=!iw:~0,1!&if "!ix!" NEQ " " if !ix! NEQ / set iv=/p:ngpackages=!iv!
set "iy=%msb.gnt.cmd%"&if defined iy goto jd
set iz=hMSBuild&if exist msb.gnt.cmd set iz=msb.gnt.cmd
for /F "tokens=*" %%i in ('%iz% -only-path 2^>^&1 ^&call echo %%^^ERRORLEVEL%%') do 2>nul (if not defined iy (set iy="%%i")else set ia=%%i)
if .%ia%==.0 if exist !iy! goto jd
(for %%v in (4.0,14.0,12.0,3.5,2.0)do (for /F "usebackq tokens=2* skip=2" %%a in (`reg query "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSBuild\ToolsVersions\%%v" /v MSBuildToolsPath 2^> nul`) do (if exist %%b (set iy="%%~b\MSBuild.exe"
(if exist !iy! (if %%v NEQ 3.5 if %%v NEQ 2.0 goto jd
echo Override engine or contact for legacy support %%v&exit/B120))))))&echo Engine is not found. Try with hMSBuild 1>&2
exit/B2
:jc
echo This feature is disabled in current version >&2
exit/B120
:jd
set ib=/noconlog&if "%debug%"=="true" set ib=/v:q
call :je&call :jf "/help" "-help" "/h" "-h" "/?" "-?"&call !iy! %iu% /nologo /noautorsp !ib! /p:wpath="%cd%/" !iv!&set ia=!ERRORLEVEL!&del /Q/F %iu%&exit/B!ia!
:jb
set iu="%cd%\%it%"&echo Generating a %it% at %cd%\...
:je
setlocal disableDelayedExpansion
<nul set/P="">%iu%&set -=ngconfig&set [=Condition&set ]=packages.config&set ;=ngserver&set .=package&set ,=GetNuTool&set :=wpath&set +=TaskCoreDllPath&set {=Exists&set }=MSBuildToolsPath&set _=Microsoft.Build.Tasks.&set a=MSBuildToolsVersion&set b=Target&set c=tmode&set d=ParameterGroup&set e=Reference&set f=System&set g=Namespace&set h=Console.WriteLine(&set i=string&set j=return&set k=Console.Error.WriteLine(&set l=string.IsNullOrEmpty(&set m=foreach&set n=Attribute&set o=Append&set p=Path&set q=Combine&set r=Length&set s=false&set t=ToString&set u=SecurityProtocolType&set v=ServicePointManager.SecurityProtocol&set w=Credentials&set x=Directory&set y=CreateDirectory&set z=Console.Write(&set $=using&set #=FileMode&set @=FileAccess&set `=StringComparison&set ?=StartsWith
<nul set/P=^<?xml version="1.0" encoding="utf-8"?^>^<Project ToolsVersion="4.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003"^>^<PropertyGroup^>^<%-% %[%="'$(%-%)'==''"^>%]%;.tools\%]%^</%-%^>^<%;% %[%="'$(%;%)'==''"^>https://www.nuget.org/api/v2/%.%/^</%;%^>^<ngpath %[%="'$(ngpath)'==''"^>packages^</ngpath^>^<%,%^>1.9.0.31242+517122d^</%,%^>^<%:% %[%="'$(%:%)'==''"^>$(MSBuildProjectDirectory)^</%:%^>^<%+% %[%="%{%('$(%}%)\%_%v$(%a%).dll')"^>$(%}%)\%_%v$(%a%).dll^</%+%^>^<%+% %[%="'$(%+%)'=='' and %{%('$(%}%)\%_%Core.dll')"^>$(%}%)\%_%Core.dll^</%+%^>^</PropertyGroup^>^<%b% Name="get" BeforeTargets="Build"^>^<d %c%="get"/^>^</%b%^>^<%b% Name="grab"^>^<d %c%="grab"/^>^</%b%^>^<%b% Name="pack"^>^<d %c%="pack"/^>^</%b%^>^<UsingTask TaskName="d" TaskFactory="CodeTaskFactory" AssemblyFile="$(%+%)"^>^<%d%^>^<%c%/^>^</%d%^>^<Task^>^<%e% Include="%f%.Xml"/^>^<%e% Include="%f%.Xml.Linq"/^>^<%e% Include="WindowsBase"/^>^<Using %g%="%f%"/^>^<Using %g%="%f%.IO"/^>^<Using %g%="%f%.IO.Packaging"/^>^<Using %g%="%f%.Linq"/^>^<Using %g%="%f%.Net"/^>^<Using %g%="%f%.Xml.Linq"/^>^<Code Type="Fragment" Language="cs"^>^<![CDATA[if("$(logo)"!="no")%h%"\nGetNuTool $(%,%)\n(c) 2015-2024  Denis Kuzmin <x-3F@outlook.com> github/3F\n");var d="{0} is not found ";var e=new %i%[]{"/_rels/","/%.%/","/[Content_Types].xml"};Action^<%i%,object^>f=(g,h)=^>{if("$(debug)".Trim()=="true")%h%g,h);};Func^<%i%,XElement^>i=j=^>{try{%j% XDocument.Load(j).Root;}catch(Exception k){%k%k.Message);throw;}};Func^<%i%,%i%[]^>l=m=^>m.Split(new[]{m.Contains('^|')?'^|':';'},(StringSplitOptions)1);if(%c%=="get"^|^|%c%=="grab"){var n=@"$(ngpackages)";var o=new StringBuilder();if(%l%n)){Action^<%i%^>p=q=^>{%m%(var r in i(q).Descendants("%.%")){var s=r.%n%("id");var t=r.%n%("version");var u=r.%n%("output");var v=r.%n%("sha1");if(s==null){%k%"{0} is corrupted",q);%j%;}o.%o%(s.Value);if(t!=null)o.%o%("/"+t.Value);if(v!=null)o.%o%("?"+v.Value);if(u!=null)o.%o%(":">>%iu%
<nul set/P=+u.Value);o.%o%(';');}};%m%(var q in l(@"$(%-%)")){var w=%p%.%q%(@"$(%:%)",q);if(File.%{%(w)){p(w);}else f(d,w);}if(o.%r%^<1){%k%"Empty .config + ngpackages");%j% %s%;}n=o.%t%();}var x=@"$(ngpath)";var y=@"$(proxycfg)";%m%(var z in Enum.GetValues(typeof(%u%)).Cast^<%u%^>()){try{%v%^|=z;}catch(NotSupportedException){}}if("$(ssl3)"!="true")%v%^&=~(%u%)(48^|192^|768);Func^<%i%,WebProxy^>D=q=^>{var E=q.Split('@');if(E.%r%^<=1)%j% new WebProxy(E[0],%s%);var F=E[0].Split(':');%j% new WebProxy(E[1],%s%){%w%=new NetworkCredential(F[0],F.%r%^>1?F[1]:null)};};Func^<%i%,%i%^>G=H=^>{var I=%p%.GetDirectoryName(H);if(!%x%.%{%(I))%x%.%y%(I);%j% H;};Func^<%i%,%i%,%i%,%i%,bool^>J=(K,L,H,v)=^>{var M=%p%.GetFullPath(%p%.%q%(@"$(%:%)",H??L??""));if(%x%.%{%(M)^|^|File.%{%(M)){%h%"{0} use {1}",L,M);%j% true;}%z%K+" ... ");var N=%c%=="grab";var O=N?G(M):%p%.%q%(%p%.GetTempPath(),Guid.NewGuid().%t%());%$%(var P=new WebClient()){try{if(!%l%y)){P.Proxy=D(y);}P.Headers.Add("User-Agent","%,%/$(%,%)");P.UseDefaultCredentials=true;if(P.Proxy!=null^&^&P.Proxy.%w%==null){P.Proxy.%w%=CredentialCache.DefaultCredentials;}P.DownloadFile(@"$(%;%)"+K,O);}catch(Exception k){%k%k.Message);%j% %s%;}}%h%M);if(v!=null){%z%"{0} ... ",v);%$%(var Q=%f%.Security.Cryptography.SHA1.Create()){o.Clear();%$%(var R=new FileStream(O,(%#%)3,(%@%)1))%m%(var S in Q.ComputeHash(R))o.%o%(S.%t%("x2"));%z%o.%t%());if(!o.%t%().Equals(v,(%`%)5)){%h%"[x]");%j% %s%;}%h%);}}if(N)%j% true;%$%(var r=ZipPackage.Open(O,(%#%)3,(%@%)1)){%m%(var T in r.GetParts()){var U=Uri.UnescapeDataString(T.Uri.OriginalString);if(e.Any(V=^>U.%?%(V,(%`%)4)))continue;var W=%p%.%q%(M,U.TrimStart('/'));f("- {0}",U);%$%(var X=T.GetStream((%#%)3,(%@%)1))%$%(var Y=File.OpenWrite(G(W))){try{X.CopyTo(Y);}catch(FileFormatException){f("[x]?crc: {0}",W);}}}}File.Delete(O);%j% true;};%m%(var r in l(n)){var Z=r.Split(new[]{':'},2);var K=Z[0].Split(new[]{'?'},2);var H=Z.%r%^>1?Z[1]:null;var L=K[0].Replace(>>%iu%
<nul set/P='/','.');if(!%l%x)){H=%p%.%q%(x,H??L);}if(!J(K[0],L,H,K.%r%^>1?K[1]:null)^&^&"$(break)".Trim()!="no")%j% %s%;}}else if(%c%=="pack"){var a=".nuspec";var b="metadata";var c="id";var A="version";var I=%p%.%q%(@"$(%:%)",@"$(ngin)");if(!%x%.%{%(I)){%k%d,I);%j% %s%;}var B=%x%.GetFiles(I,"*"+a).FirstOrDefault();if(B==null){%k%d+I,a);%j% %s%;}%h%"{0} use {1}",a,B);var C=i(B).Elements().FirstOrDefault(V=^>V.Name.LocalName==b);if(C==null){%k%d,b);%j% %s%;}var _=new %f%.Collections.Generic.Dictionary^<%i%,%i%^>();Func^<%i%,%i%^>dd=de=^>_.ContainsKey(de)?_[de]:"";%m%(var df in C.Elements())_[df.Name.LocalName.ToLower()]=df.Value;if(dd(c).%r%^>100^|^|!%f%.Text.RegularExpressions.Regex.IsMatch(dd(c),@"^\w+(?:[_.-]\w+)*$")){%k%"Invalid id");%j% %s%;}var dg=%i%.Format("{0}.{1}.nupkg",dd(c),dd(A));var dh=%p%.%q%(@"$(%:%)",@"$(ngout)");if(!%i%.IsNullOrWhiteSpace(dh)){if(!%x%.%{%(dh)){%x%.%y%(dh);}dg=%p%.%q%(dh,dg);}%h%"Creating %.% {0} ...",dg);%$%(var r=Package.Open(dg,(%#%)2)){var di=new Uri(%i%.Format("/{0}{1}",dd(c),a),(UriKind)2);r.CreateRelationship(di,0,"http://schemas.microsoft.com/packaging/2010/07/manifest");%m%(var dj in %x%.GetFiles(I,"*.*",(SearchOption)1)){if(e.Any(V=^>dj.%?%(%p%.%q%(I,V.Trim('/')),(%`%)4)))continue;var dk=dj.%?%(I,(%`%)5)?dj.Substring(I.%r%).TrimStart(%p%.DirectorySeparatorChar):dj;f("+ {0}",dk);var T=r.CreatePart(PackUriHelper.CreatePartUri(new Uri(%i%.Join("/",dk.Split('\\','/').Select(Uri.EscapeDataString)),(UriKind)2)),"application/octet",(CompressionOption)1);%$%(var dl=T.GetStream())%$%(var dm=new FileStream(dj,(%#%)3,(%@%)1)){dm.CopyTo(dl);}}var dn=r.PackageProperties;dn.Creator=dd("authors");dn.Description=dd("description");dn.Identifier=dd(c);dn.Version=dd(A);dn.Keywords=dd("tags");dn.Title=dd("title");dn.LastModifiedBy="%,%/$(%,%)";}}else %j% %s%;]]^>^</Code^>^</Task^>^</UsingTask^>^<%b% Name="Build" DependsOnTargets="%,%"/^>^</Project^>>>%iu%
endlocal&exit/B0
:jf
if defined iv set iv=!iv:%~1=!
if "%~2" NEQ "" shift & goto jf
exit/B0