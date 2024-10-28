@echo off
:: hMSBuild 2.4.1.54329+caba551
:: Copyright (c) 2017-2024  Denis Kuzmin <x-3F@outlook.com> github/3F
:: Copyright (c) hMSBuild contributors https://github.com/3F/hMSBuild
set "hh=%~dp0"&set hi=%*&if not defined hi setlocal enableDelayedExpansion & goto i0
if not defined __p_call set hi=%hi:^=^^%
set hj=%hi:!= L %
set hj=%hj:^= T %
setlocal enableDelayedExpansion&set "hk=^"&set "hj=!hj:%%=%%%%!"&set "hj=!hj:&=%%hk%%&!"
:i0
set "hl=2.8.4"&set hm=%temp%\hMSBuild_vswhere&set "hn="&set "ho="&set "hp="&set "hq="&set "hr="&set "hs="&set "ht="&set "hu="&set "hv="&set "hw="&set "hx="&set "hy="&set "hz="&set "ha="&set "hb="&set /a hc=0&if not defined hi goto i1
set hj=!hj:/?=/h!&call :i2 ic hj id&goto i3
:i4
echo.&echo hMSBuild 2.4.1.54329+caba551&echo Copyright (c) 2017-2024  Denis Kuzmin ^<x-3F@outlook.com^> github/3F&echo Copyright (c) hMSBuild contributors https://github.com/3F/hMSBuild&echo.&echo Under the MIT License https://github.com/3F/hMSBuild&echo.&echo Syntax: %~n0 [keys to %~n0] [keys to MSBuild.exe or GetNuTool]&echo.&echo Keys&echo ~~~~&echo  -no-vs        - Disable searching from Visual Studio.&echo  -no-netfx     - Disable searching from .NET Framework.&echo  -no-vswhere   - Do not search via vswhere.&echo  -no-less-15   - Do not include versions less than 15.0 (install-API/2017+)&echo  -no-less-4    - Do not include versions less than 4.0 (Windows XP+)&echo.&echo  -priority {IDs} - 15+ Non-strict components preference: C++ etc.&echo                    Separated by space "a b c" https://aka.ms/vs/workloads&echo.&echo  -vswhere {v}&echo   * 2.6.7 ...&echo   * latest - To get latest remote vswhere.exe&echo   * local  - To use only local&echo             (.bat;.exe /or from +15.2.26418.1 VS-build)&echo.&echo  -no-cache         - Do not cache vswhere for this request.&echo  -reset-cache      - To reset all cached vswhere versions before processing.&echo  -cs               - Adds to -priority C# / VB Roslyn compilers.&echo  -vc               - Adds to -priority VC++ toolset.&echo  ~c {name}         - Alias to p:Configuration={name}&echo  ~p {name}         - Alias to p:Platform={name}&echo  ~x                - Alias to m:NUMBER_OF_PROCESSORS-1 v:m&echo  -notamd64         - To use 32bit version of found msbuild.exe if it's possible.&echo  -stable           - It will ignore possible beta releases in last attempts.&echo  -eng              - Try to use english language for all build messages.
echo  -GetNuTool {args} - Access to GetNuTool core. https://github.com/3F/GetNuTool&echo  -only-path        - Only display fullpath to found MSBuild.&echo  -force            - Aggressive behavior for -priority, -notamd64, etc.&echo  -vsw-as "args..." - Reassign default commands to vswhere if used.&echo  -debug            - To show additional information from %~n0&echo  -version          - Display version of %~n0.&echo  -help             - Display this help. Aliases: -? -h&echo.&echo.&echo MSBuild switches&echo ~~~~~~~~~~~~~~~~&echo   /help or /? or /h&echo   Use /... if %~n0 overrides some -... MSBuild switches&echo.&echo.&echo Try to execute:&echo   %~n0 -only-path -no-vs -notamd64 -no-less-4&echo   %~n0 -debug ~x ~c Release&echo   %~n0 -GetNuTool "Conari;regXwild;Fnv1a128"&echo   %~n0 -GetNuTool vsSolutionBuildEvent/1.16.0:../SDK ^& SDK\GUI
echo   %~n0 -cs -no-less-15 /t:Rebuild&goto i5
:i3
set "hd="&set /a he=0
:i6
set hf=!ic[%he%]!&(if [!hf!]==[-help] (goto i4)else if [!hf!]==[-h] (goto i4)else if [!hf!]==[-?] (goto i4 ))&(if [!hf!]==[-nocachevswhere] (call :i7 -nocachevswhere -no-cache -reset-cache
set hf=-no-cache)else if [!hf!]==[-novswhere] (call :i7 -novswhere -no-vswhere&set hf=-no-vswhere)else if [!hf!]==[-novs] (call :i7 -novs -no-vs&set hf=-no-vs)else if [!hf!]==[-nonet] (call :i7 -nonet -no-netfx&set hf=-no-netfx)else if [!hf!]==[-vswhere-version] (call :i7 -vswhere-version -vswhere&set hf=-vswhere)else if [!hf!]==[-vsw-version] (call :i7 -vsw-version -vswhere&set hf=-vswhere)else if [!hf!]==[-vsw-priority] (call :i7 -vsw-priority -priority&set hf=-priority))&(if [!hf!]==[-debug] (set ht=1
goto i8)else if [!hf!]==[-GetNuTool] (call :i9 "accessing to GetNuTool ..."&(for /L %%p in (0,1,8181)do (if "!hj:~%%p,10!"=="-GetNuTool" (set hg=!hj:~%%p!
call :jh !hg:~10!&set /a hc=!ERRORLEVEL!&goto i5)))&call :i9 "!hf! is corrupted: " hj&set /a hc=1&goto i5)else if [!hf!]==[-no-vswhere] (set hq=1&goto i8)else if [!hf!]==[-no-cache] (set hr=1&goto i8)else if [!hf!]==[-reset-cache] (set hs=1&goto i8)else if [!hf!]==[-no-vs] (set ho=1&goto i8)else if [!hf!]==[-no-less-15] (set ha=1&set hp=1&goto i8)else if [!hf!]==[-no-less-4] (set hb=1&goto i8)else if [!hf!]==[-no-netfx] (set hp=1&goto i8)else if [!hf!]==[-notamd64] (set hn=1&goto i8)else if [!hf!]==[-only-path] (set hu=1&goto i8)else if [!hf!]==[-eng] (chcp 437 >nul&goto i8)else if [!hf!]==[-vswhere] (set /a "he+=1" & call :ji ic[!he!] v
set hl=!v!&call :i9 "selected vswhere version:" v&set hv=1&goto i8)else if [!hf!]==[-version] (echo 2.4.1.54329+caba551&goto i5)else if [!hf!]==[-priority] (set /a "he+=1" & call :ji ic[!he!] v
set hw=!v! !hw!&goto i8)else if [!hf!]==[-vsw-as] (set /a "he+=1" & call :ji ic[!he!] v
set hx=!v!&goto i8)else if [!hf!]==[-cs] (set hw=Microsoft.VisualStudio.Component.Roslyn.Compiler !hw!&goto i8)else if [!hf!]==[-vc] (set hw=Microsoft.VisualStudio.Component.VC.Tools.x86.x64 !hw!&goto i8)else if [!hf!]==[~c] (set /a "he+=1" & call :ji ic[!he!] v
set hd=!hd! /p:Configuration="!v!"&goto i8)else if [!hf!]==[~p] (set /a "he+=1" & call :ji ic[!he!] v
set hd=!hd! /p:Platform="!v!"&goto i8)else if [!hf!]==[~x] (set /a h0=NUMBER_OF_PROCESSORS - 1&set hd=!hd! /v:m /m:!h0!&goto i8)else if [!hf!]==[-stable] (set hy=1&goto i8)else if [!hf!]==[-force] (set hz=1&goto i8)else (call :i9 "non-handled key: " ic{%he%}&set hd=!hd! !ic{%he%}!))
:i8
set /a "he+=1" & if %he% LSS !id! goto i6
:i1
(if defined hs (call :i9 "resetting vswhere cache"
rmdir /S/Q "%hm%" 2>nul))&(if not defined hq if not defined ho (call :jj ie
if defined ie goto jk))&(if not defined ho if not defined ha (call :jl ie
if defined ie goto jk))&(if not defined hp (call :jm ie
if defined ie goto jk))
echo MSBuild tools was not found. Use `-debug` key for details.>&2
set /a hc=2&goto i5
:jk
(if defined hu (echo !ie!
goto i5))&set h1="!ie!"&echo hMSBuild: !h1!&if not defined hd goto jn
set hd=%hd: T =^%
set hd=%hd: L =^!%
set hd=!hd: E ==!
:jn
call :i9 "Arguments: " hd&!h1! !hd!
set /a hc=%ERRORLEVEL%&goto i5
:i5
exit/B!hc!
:jj
call :i9 "try vswhere..."&(if defined hv if not "!hl!"=="local" (call :jo h8 h2
call :jp h8 if h2&set %1=!if!&exit/B0))&call :jq h8&set "h2="&(if not defined h8 ((if "!hl!"=="local" (set "%1=" & exit/B2))
call :jo h8 h2))&call :jp h8 if h2&set %1=!if!&exit/B0
:jq
set h3=!hh!vswhere&call :jr h3 ig&if defined ig set "%1=!h3!" & exit/B0
set h4=Microsoft Visual Studio\Installer&if exist "%ProgramFiles(x86)%\!h4!" set "%1=%ProgramFiles(x86)%\!h4!\vswhere" & exit/B0
if exist "%ProgramFiles%\!h4!" set "%1=%ProgramFiles%\!h4!\vswhere" & exit/B0
call :i9 "local vswhere is not found."&set "%1="&exit/B3
:jo
(if defined hr (set h5=!hm!\_mta\%random%%random%vswhere)else (set h5=!hm!
(if defined hl (set h5=!h5!\!hl!))))
call :i9 "tvswhere: " h5&(if "!hl!"=="latest" (set h6=vswhere)else (set h6=vswhere/!hl!))
set h7=/p:ngpackages="!h6!:vswhere" /p:ngpath="!h5!"&call :i9 "GetNuTool call: " h7&setlocal&set __p_call=1&(if defined ht (call :jh !h7!)else (call :jh !h7! >nul))
endlocal&set "%1=!h5!\vswhere\tools\vswhere"&set "%2=!h5!"&exit/B0
:jp
set "h8=!%1!"&set "h9=!%3!"&call :jr h8 h8&(if not defined h8 (call :i9 "vswhere tool does not exist"
set "%2=" & exit/B1))
call :i9 "vswbin: " h8&set "ih="&set "ii="&set ij=!hw!&if not defined hx set hx=-products * -latest
call :i9 "assign command: `!hx!`"
:js
call :i9 "attempts with filter: !ij!; `!ih!`"&set "ik=" & set "il="
(for /F "usebackq tokens=1* delims=: " %%a in (`"!h8!" -nologo !ih! -requires !ij! Microsoft.Component.MSBuild !hx!`) do (if /I "%%~a"=="installationPath" set ik=%%~b
if /I "%%~a"=="installationVersion" set il=%%~b
(if defined ik if defined il (call :jt ik il ii
if defined ii goto ju
set "ik=" & set "il="))))&(if not defined hy if not defined ih (set ih=-prerelease
goto js))&(if defined ij (set im=Tools was not found for: !ij!
(if defined hz (call :i9 "Ignored via -force. !im!"
set "ii=" & goto ju))
call :jv "!im!"&set "ij=" & set "ih="
goto js))
:ju
(if defined h9 if defined hr (call :i9 "reset vswhere " h9
rmdir /S/Q "!h9!"))&set %2=!ii!&exit/B0
:jt
set ik=!%1!&set il=!%2!&call :i9 "vspath: " ik&call :i9 "vsver: " il&(if not defined il (call :i9 "nothing to see via vswhere"
set "%3=" & exit/B3))
(for /F "tokens=1,2 delims=." %%a in ("!il!") do (set il=%%~a.0))&if !il! geq 16 set il=Current
if not exist "!ik!\MSBuild\!il!\Bin" set "%3=" & exit/B3
set in=!ik!\MSBuild\!il!\Bin&call :i9 "found path via vswhere: " in&(if exist "!in!\amd64" (call :i9 "found /amd64"
set in=!in!\amd64))&call :jw in in&set %3=!in!&exit/B0
:jl
call :i9 "Searching from Visual Studio - 2015, 2013, ..."&(for %%v in (14.0,12.0)do (call :jx %%v Y & (if defined Y (set %1=!Y!
exit/B0))))&call :i9 "-vs: not found"&set "%1="&exit/B0
:jm
call :i9 "Searching from .NET Framework - .NET 4.0, ..."&(for %%v in (4.0,3.5,2.0)do (call :jx %%v Y & (if defined Y (set %1=!Y!
exit/B0)else if defined hb (goto :jy ))))
:jy
call :i9 "-netfx: not found"&set "%1="&exit/B0
:jx
call :i9 "check %1"&(for /F "usebackq tokens=2* skip=2" %%a in (`reg query "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSBuild\ToolsVersions\%1" /v MSBuildToolsPath 2^> nul`) do (if exist %%b (set in=%%~b
call :i9 ":msbfound " in&call :jw in if&set %2=!if!&exit/B0)))&set "%2="&exit/B0
:jw
set in=!%~1!\MSBuild.exe&if exist "!in!" set "%2=!in!"
(if not defined hn (exit/B0))
set io=!in:Framework64=Framework!&set io=!io:\amd64=!&(if exist "!io!" (call :i9 "Return 32bit version because of -notamd64 key."
set %2=!io!&exit/B0))&(if defined hz (call :i9 "Ignored via -force. Only 64bit version was found for -notamd64"
set "%2=" & exit/B0))
if not "%2"=="" call :jv "Return 64bit version. Found only this."
exit/B0
:jr
call :i9 "bat/exe: " %1&if exist "!%1!.bat" set %2="!%1!.bat" & exit/B0
if exist "!%1!.exe" set %2="!%1!.exe" & exit/B0
set "%2="&exit/B0
:i7
call :jv "'%~1' is obsolete. Use: %~2 %~3"&exit/B0
:jv
echo   [*] WARN: %~1 >&2
exit/B0
:i9
(if defined ht (set "ip=%~1" & echo [ %TIME% ] !ip! !%2! !%3!))
exit/B0
:i2
set iq=!%2!
:jz
(for /F "tokens=1* delims==" %%a in ("!iq!") do (if "%%~b"=="" (call :ja %1 !iq! %3 & exit/B0)else set iq=%%a E %%b))&goto jz
:ja
set "ir=%~1"&set /a he=-1
:jb
set /a he+=1&set %ir%[!he!]=%~2&set %ir%{!he!}=%2&if "%~4" NEQ "" shift & goto jb
set %3=!he!&exit/B0
:ji
set is=!%1!
set "is=%is: T =^%"
set "is=%is: L =^!%"
set is=!is: E ==!
set %2=!is!&exit/B0
:jh
setlocal disableDelayedExpansion&@echo off
:: GetNuTool /shell/batch edition
:: Copyright (c) 2015-2024  Denis Kuzmin <x-3F@outlook.com> github/3F
:: https://github.com/3F/GetNuTool
set it=gnt.core&set iu="%temp%\%it%1.9.0%random%%random%"&if "%~1"=="-unpack" goto jc
if "%~1"=="-msbuild" goto jd
set iv=%*&setlocal enableDelayedExpansion&set "iw=%~1 "&set ix=!iw:~0,1!&if "!ix!" NEQ " " if !ix! NEQ / set iv=/p:ngpackages=!iv!
set "iy=%msb.gnt.cmd%"&if defined iy goto je
set iz=hMSBuild&if exist msb.gnt.cmd set iz=msb.gnt.cmd
for /F "tokens=*" %%i in ('%iz% -only-path 2^>^&1 ^&call echo %%^^ERRORLEVEL%%') do 2>nul (if not defined iy (set iy="%%i")else set ia=%%i)
if .%ia%==.0 if exist !iy! goto je
(for %%v in (4.0,14.0,12.0,3.5,2.0)do (for /F "usebackq tokens=2* skip=2" %%a in (`reg query "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSBuild\ToolsVersions\%%v" /v MSBuildToolsPath 2^> nul`) do (if exist %%b (set iy="%%~b\MSBuild.exe"
(if exist !iy! (if %%v NEQ 3.5 if %%v NEQ 2.0 goto je
echo Override engine or contact for legacy support %%v&exit/B120))))))&echo Engine is not found. Try with hMSBuild 1>&2
exit/B2
:jd
echo This feature is disabled in current version >&2
exit/B120
:je
set ib=/noconlog&if "%debug%"=="true" set ib=/v:q
call :jf&call :jg "/help" "-help" "/h" "-h" "/?" "-?"&call !iy! %iu% /nologo /noautorsp !ib! /p:wpath="%cd%/" !iv!&set ia=!ERRORLEVEL!&del /Q/F %iu%&exit/B!ia!
:jc
set iu="%cd%\%it%"&echo Generating a %it% at %cd%\...
:jf
setlocal disableDelayedExpansion
<nul set/P="">%iu%&set -=ngconfig&set [=Condition&set ]=packages.config&set ;=ngserver&set .=package&set ,=GetNuTool&set :=wpath&set +=TaskCoreDllPath&set {=Exists&set }=MSBuildToolsPath&set _=Microsoft.Build.Tasks.&set a=MSBuildToolsVersion&set b=Target&set c=tmode&set d=ParameterGroup&set e=Reference&set f=System&set g=Namespace&set h=Console.WriteLine(&set i=string&set j=return&set k=Console.Error.WriteLine(&set l=string.IsNullOrEmpty(&set m=foreach&set n=Attribute&set o=Append&set p=Path&set q=Combine&set r=Length&set s=false&set t=ToString&set u=SecurityProtocolType&set v=ServicePointManager.SecurityProtocol&set w=Credentials&set x=Directory&set y=CreateDirectory&set z=Console.Write(&set $=using&set #=FileMode&set @=FileAccess&set `=StringComparison&set ?=StartsWith
<nul set/P=^<?xml version="1.0" encoding="utf-8"?^>^<Project ToolsVersion="4.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003"^>^<PropertyGroup^>^<%-% %[%="'$(%-%)'==''"^>%]%;.tools\%]%^</%-%^>^<%;% %[%="'$(%;%)'==''"^>https://www.nuget.org/api/v2/%.%/^</%;%^>^<ngpath %[%="'$(ngpath)'==''"^>packages^</ngpath^>^<%,%^>1.9.0.1956+bb83b59^</%,%^>^<%:% %[%="'$(%:%)'==''"^>$(MSBuildProjectDirectory)^</%:%^>^<%+% %[%="%{%('$(%}%)\%_%v$(%a%).dll')"^>$(%}%)\%_%v$(%a%).dll^</%+%^>^<%+% %[%="'$(%+%)'=='' and %{%('$(%}%)\%_%Core.dll')"^>$(%}%)\%_%Core.dll^</%+%^>^</PropertyGroup^>^<%b% Name="get" BeforeTargets="Build"^>^<d %c%="get"/^>^</%b%^>^<%b% Name="grab"^>^<d %c%="grab"/^>^</%b%^>^<%b% Name="pack"^>^<d %c%="pack"/^>^</%b%^>^<UsingTask TaskName="d" TaskFactory="CodeTaskFactory" AssemblyFile="$(%+%)"^>^<%d%^>^<%c%/^>^</%d%^>^<Task^>^<%e% Include="%f%.Xml"/^>^<%e% Include="%f%.Xml.Linq"/^>^<%e% Include="WindowsBase"/^>^<Using %g%="%f%"/^>^<Using %g%="%f%.IO"/^>^<Using %g%="%f%.IO.Packaging"/^>^<Using %g%="%f%.Linq"/^>^<Using %g%="%f%.Net"/^>^<Using %g%="%f%.Xml.Linq"/^>^<Code Type="Fragment" Language="cs"^>^<![CDATA[if("$(logo)"!="no")%h%"\nGetNuTool $(%,%)\n(c) 2015-2024  Denis Kuzmin <x-3F@outlook.com> github/3F\n");var d="{0} is not found ";var e=new %i%[]{"/_rels/","/%.%/","/[Content_Types].xml"};Action^<%i%,object^>f=(g,h)=^>{if("$(debug)".Trim()=="true")%h%g,h);};Func^<%i%,XElement^>i=j=^>{try{%j% XDocument.Load(j).Root;}catch(Exception k){%k%k.Message);throw;}};Func^<%i%,%i%[]^>l=m=^>m.Split(new[]{m.Contains('^|')?'^|':';'},(StringSplitOptions)1);if(%c%=="get"^|^|%c%=="grab"){var n=@"$(ngpackages)";var o=new StringBuilder();if(%l%n)){Action^<%i%^>p=q=^>{%m%(var r in i(q).Descendants("%.%")){var s=r.%n%("id");var t=r.%n%("version");var u=r.%n%("output");var v=r.%n%("sha1");if(s==null){%k%"{0} is corrupted",q);%j%;}o.%o%(s.Value);if(t!=null)o.%o%("/"+t.Value);if(v!=null)o.%o%("?"+v.Value);if(u!=null)o.%o%(":">>%iu%
<nul set/P=+u.Value);o.%o%(';');}};%m%(var q in l(@"$(%-%)")){var w=%p%.%q%(@"$(%:%)",q);if(File.%{%(w)){p(w);}else f(d,w);}if(o.%r%^<1){%k%"Empty .config + ngpackages");%j% %s%;}n=o.%t%();}var x=@"$(ngpath)";var y=@"$(proxycfg)";%m%(var z in Enum.GetValues(typeof(%u%)).Cast^<%u%^>()){try{%v%^|=z;}catch(NotSupportedException){}}if("$(ssl3)"!="true")%v%^&=~(%u%)(48^|192^|768);Func^<%i%,WebProxy^>D=q=^>{var E=q.Split('@');if(E.%r%^<=1)%j% new WebProxy(E[0],%s%);var F=E[0].Split(':');%j% new WebProxy(E[1],%s%){%w%=new NetworkCredential(F[0],F.%r%^>1?F[1]:null)};};Func^<%i%,%i%^>G=H=^>{var I=%p%.GetDirectoryName(H);if(!%x%.%{%(I))%x%.%y%(I);%j% H;};Func^<%i%,%i%,%i%,%i%,bool^>J=(K,L,H,v)=^>{var M=%p%.GetFullPath(%p%.%q%(@"$(%:%)",H??L??""));if(%x%.%{%(M)^|^|File.%{%(M)){%h%"{0} use {1}",L,M);%j% true;}%z%K+" ... ");var N=%c%=="grab";var O=N?G(M):%p%.%q%(%p%.GetTempPath(),Guid.NewGuid().%t%());%$%(var P=new WebClient()){try{if(!%l%y)){P.Proxy=D(y);}P.Headers.Add("User-Agent","%,%/$(%,%)");P.UseDefaultCredentials=true;if(P.Proxy!=null^&^&P.Proxy.%w%==null){P.Proxy.%w%=CredentialCache.DefaultCredentials;}P.DownloadFile(@"$(%;%)"+K,O);}catch(Exception k){%k%k.Message);%j% %s%;}}%h%M);if(v!=null){%z%"{0} ... ",v);%$%(var Q=%f%.Security.Cryptography.SHA1.Create()){o.Clear();%$%(var R=new FileStream(O,(%#%)3,(%@%)1))%m%(var S in Q.ComputeHash(R))o.%o%(S.%t%("x2"));%z%o.%t%());if(!o.%t%().Equals(v,(%`%)5)){%h%"[x]");%j% %s%;}%h%);}}if(N)%j% true;%$%(var r=ZipPackage.Open(O,(%#%)3,(%@%)1)){%m%(var T in r.GetParts()){var U=Uri.UnescapeDataString(T.Uri.OriginalString);if(e.Any(V=^>U.%?%(V,(%`%)4)))continue;var W=%p%.%q%(M,U.TrimStart('/'));f("- {0}",U);%$%(var X=T.GetStream((%#%)3,(%@%)1))%$%(var Y=File.OpenWrite(G(W))){try{X.CopyTo(Y);}catch(FileFormatException){f("[x]?crc: {0}",W);}}}}File.Delete(O);%j% true;};%m%(var r in l(n)){var Z=r.Split(new[]{':'},2);var K=Z[0].Split(new[]{'?'},2);var H=Z.%r%^>1?Z[1]:null;var L=K[0].Replace(>>%iu%
<nul set/P='/','.');if(!%l%x)){H=%p%.%q%(x,H??L);}if(!J(K[0],L,H,K.%r%^>1?K[1]:null)^&^&"$(break)".Trim()!="no")%j% %s%;}}else if(%c%=="pack"){var a=".nuspec";var b="metadata";var c="id";var A="version";var I=%p%.%q%(@"$(%:%)",@"$(ngin)");if(!%x%.%{%(I)){%k%d,I);%j% %s%;}var B=%x%.GetFiles(I,"*"+a).FirstOrDefault();if(B==null){%k%d+I,a);%j% %s%;}%h%"{0} use {1}",a,B);var C=i(B).Elements().FirstOrDefault(V=^>V.Name.LocalName==b);if(C==null){%k%d,b);%j% %s%;}var _=new %f%.Collections.Generic.Dictionary^<%i%,%i%^>();Func^<%i%,%i%^>dd=de=^>_.ContainsKey(de)?_[de]:"";%m%(var df in C.Elements())_[df.Name.LocalName.ToLower()]=df.Value;if(dd(c).%r%^>100^|^|!%f%.Text.RegularExpressions.Regex.IsMatch(dd(c),@"^\w+(?:[_.-]\w+)*$")){%k%"Invalid id");%j% %s%;}var dg=%i%.Format("{0}.{1}.nupkg",dd(c),dd(A));var dh=%p%.%q%(@"$(%:%)",@"$(ngout)");if(!%i%.IsNullOrWhiteSpace(dh)){if(!%x%.%{%(dh)){%x%.%y%(dh);}dg=%p%.%q%(dh,dg);}%h%"Creating %.% {0} ...",dg);%$%(var r=Package.Open(dg,(%#%)2)){var di=new Uri(%i%.Format("/{0}{1}",dd(c),a),(UriKind)2);r.CreateRelationship(di,0,"http://schemas.microsoft.com/packaging/2010/07/manifest");%m%(var dj in %x%.GetFiles(I,"*.*",(SearchOption)1)){if(e.Any(V=^>dj.%?%(%p%.%q%(I,V.Trim('/')),(%`%)4)))continue;var dk=dj.%?%(I,(%`%)5)?dj.Substring(I.%r%).TrimStart(%p%.DirectorySeparatorChar):dj;f("+ {0}",dk);var T=r.CreatePart(PackUriHelper.CreatePartUri(new Uri(%i%.Join("/",dk.Split('\\','/').Select(Uri.EscapeDataString)),(UriKind)2)),"application/octet",(CompressionOption)1);%$%(var dl=T.GetStream())%$%(var dm=new FileStream(dj,(%#%)3,(%@%)1)){dm.CopyTo(dl);}}var dn=r.PackageProperties;dn.Creator=dd("authors");dn.Description=dd("description");dn.Identifier=dd(c);dn.Version=dd(A);dn.Keywords=dd("tags");dn.Title=dd("title");dn.LastModifiedBy="%,%/$(%,%)";}}else %j% %s%;]]^>^</Code^>^</Task^>^</UsingTask^>^<%b% Name="Build" DependsOnTargets="%,%"/^>^</Project^>>>%iu%
endlocal&exit/B0
:jg
if defined iv set iv=!iv:%~1=!
if "%~2" NEQ "" shift & goto jg
exit/B0