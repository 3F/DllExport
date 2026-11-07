::! hMSBuild (c) Denis Kuzmin <x-3F@outlook.com> github.com/3F
@echo off&set _0=MSBuild&set "hh=%~dp0"&set hi=%*&if not defined hi setlocal enableDelayedExpansion&goto iz
if not defined __p_call set hi=%hi:^=^^%
set hj=%hi:!= L %
set hj=%hj:^= T %
setlocal enableDelayedExpansion&set "hk=^"&set "hj=!hj:%%=%%%%!"&set "hj=!hj:&=%%hk%%&!"
:iz
set "hl=3.1.7"&set hm=%temp%\h%_0%_vswhere&set "hn="&set "ho="&set "hp="&set "hq="&set "hr="&set "hs="&set "ht="&set "hu="&set "hv="&set "hw="&set "hx="&set "hy="&set "hz="&set "ha="&set "hb="&set "hc=HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\%_0%\ToolsVersions\"&set/ahd=0&if not defined hi goto ia
set hj=!hj:/?=/h!&call :inita iu hj iv&goto ib
:ic
echo.&echo h%_0% 2.5.0.3303+ae68d39&echo Copyright (c) 2017-2025  Denis Kuzmin ^<x-3F@outlook.com^> github/3F&echo Copyright (c) h%_0% contributors&echo.&echo Under the MIT License https://github.com/3F/h%_0%&echo.&echo Syntax: %~n0 [keys to %~n0] [keys to %_0%.exe or GetNuTool]&echo.&echo Keys&echo ~~~~&echo  -no-vs        - Disable searching from Visual Studio.&echo  -no-netfx     - Disable searching from .NET Framework.&echo  -no-vswhere   - Do not search via vswhere.&echo  -no-less-15   - Do not include versions less than 15.0 (install-API/2017+)&echo  -no-less-4    - Do not include versions less than 4.0 (Windows XP+)&echo.&echo  -priority {IDs} - 15+ Non-strict components preference: C++ etc.&echo                    Separated by space "a b c" https://aka.ms/vs/workloads&echo.&echo  -vswhere {v}&echo   * 2.6.7 ...&echo   * latest - To get latest remote vswhere.exe&echo   * local  - To use only local&echo             (.bat;.exe /or from +15.2.26418.1 VS-build)&echo.&echo  -no-cache         - Do not cache vswhere for this request.&echo  -reset-cache      - To reset all cached vswhere versions before processing.&echo  -cs               - Adds to -priority C# / VB Roslyn compilers.&echo  -vc               - Adds to -priority VC++ toolset.&echo  ~c {name}         - Alias to p:Configuration={name}&echo  ~p {name}         - Alias to p:Platform={name}&echo  ~x                - Alias to m:NUMBER_OF_PROCESSORS-1 v:m&echo  -notamd64         - Prefer 32-bit instead of 64-bit %_0%.exe.&echo  -stable           - It will ignore possible beta releases in last attempts.&echo  -eng              - Try to use english language for all build messages.&echo.&echo  -GetNuTool {args} - Access to GetNuTool (built-in) https://github.com/3F/GetNuTool
echo                      %~n0 -GetNuTool -help&echo                      %~n0 -GetNuTool ~/p:use=documentation&echo.&echo  -only-path        - Only display fullpath to found %_0%.&echo  -force            - Aggressive behavior for -priority, -notamd64, etc.&echo  -vsw-as "args..." - Reassign default commands to vswhere if used.&echo  -debug            - To show additional information from %~n0&echo  -version          - Display version of %~n0.&echo  -help             - Display this help. Aliases: -? -h&echo.&echo.&echo Original %_0%&echo ~~~~~~~~~~~~~~~~&echo   /help or /? or /h&echo   Use /... if %~n0 overrides -...&echo.&echo.&echo Examples:&echo   %~n0 -only-path -no-vs -notamd64 -no-less-4&echo   %~n0 -debug ~x ~c Release&echo   %~n0 -GetNuTool "Conari;regXwild;Fnv1a128"&echo   %~n0 -GetNuTool vsSolutionBuildEvent/1.16.1:../SDK ^& SDK\GUI&echo   %~n0 -GetNuTool ~^& svc.gnt&echo   %~n0 -cs -no-less-15 /t:Rebuild&goto H
:ib
set "he="&set/ahf=0
:id
set hg=!iu[%hf%]!&(if [!hg!]==[-help] (goto ic)else if [!hg!]==[-h] (goto ic)else if [!hg!]==[-?] (goto ic ))&(if [!hg!]==[-nocachevswhere] (call :ie -nocachevswhere -no-cache -reset-cache
set hg=-no-cache)else if [!hg!]==[-novswhere] (call :ie -novswhere -no-vswhere&set hg=-no-vswhere)else if [!hg!]==[-novs] (call :ie -novs -no-vs&set hg=-no-vs)else if [!hg!]==[-nonet] (call :ie -nonet -no-netfx&set hg=-no-netfx)else if [!hg!]==[-vswhere-version] (call :ie -vswhere-version -vswhere&set hg=-vswhere)else if [!hg!]==[-vsw-version] (call :ie -vsw-version -vswhere&set hg=-vswhere)else if [!hg!]==[-vsw-priority] (call :ie -vsw-priority -priority&set hg=-priority))&(if [!hg!]==[-debug] (set ht=1
goto if)else if [!hg!]==[-GetNuTool] (call :ig "accessing built-in GetNuTool ..."&(for /L %%p in (0,1,8181)do (if "!hj:~%%p,10!"=="-GetNuTool" (set h0=!hj:~%%p!
call :i0 !h0:~10!&set/ahd=!ERRORLEVEL!&goto H)))&call :ig "!hg! is corrupted: " hj&set/ahd=1&goto H)else if [!hg!]==[-no-vswhere] (set hq=1&goto if)else if [!hg!]==[-no-cache] (set hr=1&goto if)else if [!hg!]==[-reset-cache] (set hs=1&goto if)else if [!hg!]==[-no-vs] (set ho=1&goto if)else if [!hg!]==[-no-less-15] (set ha=1&set hp=1&goto if)else if [!hg!]==[-no-less-4] (set hb=1&goto if)else if [!hg!]==[-no-netfx] (set hp=1&goto if)else if [!hg!]==[-notamd64] (set hn=1&goto if)else if [!hg!]==[-only-path] (set hu=1&goto if)else if [!hg!]==[-eng] (chcp 437 >nul&goto if)else if [!hg!]==[-vswhere] (set/a"hf+=1"&call :i1 iu[!hf!] v
set hl=!v!&call :ig "selected vswhere version:" v&set hv=1&goto if)else if [!hg!]==[-version] (echo 2.5.0.3303+ae68d39&goto H)else if [!hg!]==[-priority] (set/a"hf+=1"&call :i1 iu[!hf!] v
set hw=!v! !hw!&goto if)else if [!hg!]==[-vsw-as] (set/a"hf+=1"&call :i1 iu[!hf!] v
set hx=!v!&goto if)else if [!hg!]==[-cs] (set hw=Microsoft.VisualStudio.Component.Roslyn.Compiler !hw!&goto if)else if [!hg!]==[-vc] (set hw=Microsoft.VisualStudio.Component.VC.Tools.x86.x64 !hw!&goto if)else if [!hg!]==[~c] (set/a"hf+=1"&call :i1 iu[!hf!] v
set he=!he! /p:Configuration="!v!"&goto if)else if [!hg!]==[~p] (set/a"hf+=1"&call :i1 iu[!hf!] v
set he=!he! /p:Platform="!v!"&goto if)else if [!hg!]==[~x] (set/ah1=NUMBER_OF_PROCESSORS-1&set he=!he! /v:m /m:!h1!&goto if)else if [!hg!]==[-stable] (set hy=1&goto if)else if [!hg!]==[-force] (set hz=1&goto if)else (call :ig "non-handled key: " iu{%hf%}&set he=!he! !iu{%hf%}!))
:if
set/a"hf+=1"&if %hf% LSS !iv! goto id
:ia
(if defined hs (call :ig "resetting vswhere cache"
rmdir /S/Q "%hm%" 2>nul))&(if not defined hq if not defined ho (call :i2 iw
if defined iw goto i3))&(if not defined ho if not defined ha (call :i4 iw
if defined iw goto i3))&(if not defined hp (call :i5 iw
if defined iw goto i3))
echo %_0% tools was not found. Use `-debug` key for details.>&2
set/ahd=2&goto H
:i3
(if defined hu (echo !iw!
goto H))&set h2="!iw!"&echo h%_0%: !h2!&if not defined he goto i6
set he=%he: T =^%
set he=%he: L =^!%
set he=!he: E ==!
:i6
call :ig "Arguments: " he&call !h2! !he!&set/ahd=!ERRORLEVEL!
:H
exit/B!hd!
:eva
call :i1 %*&exit/B
:i2
call :ig "try vswhere..."&(if defined hv if not "!hl!"=="local" (call :i7 h9 h3
call :i8 h9 ix h3&set %1=!ix!&exit/B0))
call :i9 h9&set "h3="&(if not defined h9 ((if "!hl!"=="local" (set "%1="&exit/B2))
call :i7 h9 h3))&call :i8 h9 ix h3&set %1=!ix!&exit/B0
:i9
set h4=!hh!vswhere&call :jh h4 iy&if defined iy set "%1=!h4!"&exit/B0
set h5=Microsoft Visual Studio\Installer&if exist "%ProgramFiles(x86)%\!h5!" set "%1=%ProgramFiles(x86)%\!h5!\vswhere"&exit/B0
if exist "%ProgramFiles%\!h5!" set "%1=%ProgramFiles%\!h5!\vswhere"&exit/B0
call :ig "local vswhere is not found."&set "%1="&exit/B3
:i7
(if defined hr (set h6=!hm!\_mta\%random%%random%vswhere)else (set h6=!hm!
(if defined hl (set h6=!h6!\!hl!))))
call :ig "tvswhere: " h6&(if "!hl!"=="latest" (set h7=vswhere)else (set h7=vswhere/!hl!))
set h8="!h7!:vswhere" /p:ngpath="!h6!"&call :ig "GetNuTool call: " h8
setlocal&set __p_call=1&(if defined ht (call :i0 !h8!)else (call :i0 !h8! >nul))
endlocal&set "%1=!h6!\vswhere\tools\vswhere"&set "%2=!h6!"&exit/B0
:i8
set "h9=!%1!"&set "ih=!%3!"&call :jh h9 h9&(if not defined h9 (call :ig "vswhere tool does not exist"
set "%2="&exit/B1))
call :ig "vswbin: " h9&set "ii="&set "ij="&set ik=!hw!&if not defined hx set hx=-products * -latest
call :ig "assign command: `!hx!`"
:ji
call :ig "attempts with filter: !ik!; `!ii!`"&set "il=" & set "im="
(for /F "usebackq tokens=1* delims=: " %%a in (`!h9! -nologo !ii! -requires !ik! Microsoft.Component.%_0% !hx!`) do (if /I "%%~a"=="installationPath" set il=%%~b
if /I "%%~a"=="installationVersion" set im=%%~b
(if defined il if defined im (call :jj il im ij
if defined ij goto jk
set "il=" & set "im="))))&(if not defined hy if not defined ii (set ii=-prerelease
goto ji))&(if defined ik (set in=Tools was not found for: !ik!
(if defined hz (call :ig "Ignored via -force. !in!"
set "ij="&goto jk))
call :jl "!in!"&set "ik=" & set "ii="
goto ji))
:jk
(if defined ih if defined hr (call :ig "reset vswhere " ih
rmdir /S/Q "!ih!"))&set %2=!ij!&exit/B0
:jj
set il=!%1!&set im=!%2!&call :ig "vspath: " il&call :ig "vsver: " im&(if not defined im (call :ig "nothing to see via vswhere"
set "%3="&exit/B3))
(for /F "tokens=1,2 delims=." %%a in ("!im!") do (set im=%%~a.0))&if !im! geq 16 set im=Current
if not exist "!il!\%_0%\!im!\Bin" set "%3="&exit/B3
set io=!il!\%_0%\!im!\Bin&call :ig "found path via vswhere: " io&(if exist "!io!\amd64" (call :ig "found /amd64"
set io=!io!\amd64))&call :jm io io&set %3=!io!&exit/B0
:i4
call :ig "Searching from Visual Studio - 2015, 2013, ..."&(for %%v in (14.0,12.0)do (call :jn %%v Y&(if defined Y (set %1=!Y!
exit/B0))))&call :ig "-vs: not found"&set "%1="&exit/B0
:i5
call :ig "Searching from .NET Framework - .NET 4.0, ..."&(for %%v in (4.0,3.5,2.0)do (call :jn %%v Y&(if defined Y (set %1=!Y!
exit/B0)else if defined hb (goto :jo ))))
:jo
call :ig "-netfx: not found"&set "%1="&exit/B0
:jn
call :ig "check %1"&(for /F "usebackq tokens=2* skip=2" %%a in (`reg query "%hc%%1" /v %_0%ToolsPath 2^>nul`)do (if exist %%b (set io=%%~b
call :ig ":msbfound " io&call :jm io ix&set %2=!ix!&exit/B0)))&set "%2="&exit/B0
:jm
set io=!%~1!\%_0%.exe&if exist "!io!" set "%2=!io!"
(if not defined hn (exit/B0))
set ip=!io:Framework64=Framework!&set ip=!ip:\amd64=!&(if exist "!ip!" (call :ig "Return 32bit version because of -notamd64 key."
set %2=!ip!&exit/B0))&(if defined hz (call :ig "Ignored via -force. Only 64bit version was found for -notamd64"
set "%2="&exit/B0))
if not "%2"=="" call :jl "Return 64bit version. Found only this."
exit/B0
:jh
call :ig "bat/exe: " %1&(if exist "!%1!.bat" (set %2="!%1!.bat")else if exist "!%1!.exe" (set %2="!%1!.exe")else (set "%2=" ))
exit/B0
:ie
call :jl "'%~1' is obsolete. Use: %~2 %~3"&exit/B0
:jl
echo   [*] WARN: %~1>&2
exit/B0
:ig
(if defined ht (set "iq=%~1" &echo [ %TIME% ] !iq! !%2! !%3!))
exit/B0
:inita
set ir=!%2!&set ir=!ir:""=!
:jp
(for /F "tokens=1* delims==" %%a in ("!ir!") do (if "%%~b"=="" (call :jq %1 !ir! %3&exit/B0)else set ir=%%a E %%b))&goto jp
:jq
set "is=%~1"&set/ahf=-1
:jr
set/ahf+=1&set %is%[!hf!]=%~2&set %is%{!hf!}=%2&if "%~4" NEQ "" shift&goto jr
set %3=!hf!&exit/B0
:i1
set it=!%1!
if not defined it set %2=&exit/B0
set "it=%it: T =^%"
set "it=%it: L =^!%"
set it=!it: E ==!
if "%~3" EQU "1" set it=!it:`="!
set %2=!it!&exit/B0
:i0
setlocal disableDelayedExpansion
::! GetNuTool (c) Denis Kuzmin <x-3F@outlook.com> github.com/3F
set ee=gnt.core&set ef="%temp%\%ee%1.10.0%random%%random%"&set -=GetNuTool
setlocal enableDelayedExpansion&set eg=%*&set eh=%~1&if "!eh!"=="-unpack" goto eo
if "!eh!"=="" call :ep
set "ei=!eg!"&call :eq "/help" "-help" "/h" "-h" "/?" "-?"&set ej=%-%/1.10.0 /p:info=no;use=&if "!ei!" NEQ "!eg!" set eg=~!ej!?
set ek="!eg:~0,1!"&if !ek!=="+" call :er install
if !ek!=="*" call :er run
if !ek!=="~" call :er touch
if defined eg if !ek! NEQ "/" if !ek! NEQ "-" set eg=/p:ngpackages=!eg!
set "el="&for /F "tokens=*" %%i in ('h%_0%.cmd -only-path 2^>^&1') do 2>nul set el="%%i"
if exist !el! goto es
for %%v in (4.0,14.0,12.0)do (for /F "usebackq tokens=2* skip=2" %%a in (`reg query "%hc%%%v" /v %_0%ToolsPath 2^>nul`)do (set el="%%~b\%_0%.exe"
if exist !el! goto es))
echo [x]Engine>&2
exit/B2
:es
set em=/noconlog&if "%debug%"=="true" set em=/v:q
call :et&call !el! %ef% /nologo /noautorsp !em! /p:wpath="%cd%/" !eg!&set en=!ERRORLEVEL!&del /Q/F %ef%&exit/B!en!
:eo
set ef="%cd%\%ee%"&echo +%ee% %cd%\
:et
setlocal disableDelayedExpansion
<nul set/P=>%ef%&set [=TaskCoreDllPath&set ]=Exists&set ;=%_0%ToolsPath&set .=Microsoft.Build.Tasks.&set ,=%_0%ToolsVersion&set :=tmode&set +=System&set {=Using&set }=Namespace&set _=Console.WriteLine(&set a=);var&set b=String&set c=package&set d=Environment&set e=){var&set f=);if(&set g=.IsNullOr&set h=foreach&set i=(var&set j=Attribute&set k==null&set l=Console.Error.WriteLine(&set m=return&set n=Append&set o=Path&set p=Combine&set q=Length&set r=false&set s=SecurityProtocol&set t=ServicePointManager&set u=Credential&set v=Directory&set w=.Create&set x=Console.Write(&set y=using&set z=Exception&set $=FileMode&set #=FileAccess&set @=Comparison&set `=StartsWith&set ?=)continue;var&set !=http://schemas.microsoft.com/&set _1=Reference Include="
<nul set/P=^<?xml version="1.0"?^>^<Project ToolsVersion="4.0" xmlns="%!%developer/%_0%/2003"^>^<PropertyGroup^>^<%-%^>1.10.0^</%-%^>^<%[% Condition="%]%('$(%;%)\%.%v$(%,%).dll')"^>$(%;%)\%.%v$(%,%).dll^</%[%^>^<%[% Condition="'$(%[%)'=='' and %]%('$(%;%)\%.%Core.dll')"^>$(%;%)\%.%Core.dll^</%[%^>^</PropertyGroup^>>>%ef%
for %%t in (get,install,run,touch,grab,pack)do <nul set/P=^<Target Name="%%t"^>^<K %:%="%%t"/^>^</Target^>>>%ef%
<nul set/P=^<%{%Task TaskName="K" TaskFactory="CodeTaskFactory" AssemblyFile="$(%[%)"^>^<ParameterGroup^>^<%:%/^>^</ParameterGroup^>^<Task^>^<%_1%%+%.Xml"/^>^<%_1%%+%.Xml.Linq"/^>^<%_1%WindowsBase"/^>^<%{% %}%="%+%"/^>^<%{% %}%="%+%.Text"/^>^<%{% %}%="%+%.IO"/^>^<%{% %}%="%+%.IO.Packaging"/^>^<%{% %}%="%+%.Linq"/^>^<%{% %}%="%+%.Net"/^>^<%{% %}%="%+%.Xml.Linq"/^>^<%{% %}%="%+%.Diagnostics"/^>^<Code Type="Fragment" Language="cs"^>^<![CDATA[if("$(logo)"!="no")%_%"\n%-% $(%-%)\n(c) 2015-2025  Denis Kuzmin <x-3F@outlook.com> github/3F\n"%a% K=" is not found ";var L=new %b%[]{"/_rels/","/%c%/","/[Content_Types].xml"};Action^<%b%^>M=N=^>{if("$(debug)"=="true")%_%N);};Func^<%b%,%b%^>O=P=^>P==""?null:P;var Q=O(@"$(wpath)")??@"$(%_0%Project%v%)";%d%.Current%v%=Q;Action^<%b%,%b%^>R=%d%.Set%d%Variable;R("%-%","$(%-%)");R("use","$(use)");R("debug","$(debug)"%a% S="id";var T="version";var U="$(info)"!="no";if(%:%!="pack"%e% V=@"$(ng%c%s)";var W=new %b%Builder(%f%%b%%g%WhiteSpace(V)%e% X=O(@"$(ngconfig)")??@"%c%s.config;.tools\%c%s.config";Action^<%b%^>Y=Z=^>{%h%%i% D in XDocument.Load(Z).Root.Descendants("%c%")%e% E=D.%j%(S%a% F=D.%j%(T%a% G=D.%j%("output"%a% H=D.%j%("sha1"%f%E=%k%){%l%"Invalid "+Z);%m%;}W.%n%(E.Value%f%F!%k%)W.%n%("/"+F.Value%f%H!%k%)W.%n%("?">>%ef%
<nul set/P=+H.Value%f%G!%k%)W.%n%(":"+G.Value);W.%n%(';');}};%h%%i% Z in X.Split(';')%e% I=%o%.%p%(Q,Z%f%File.%]%(I)){Y(I);}else M(I+K);}if(W.%q%^<1){%l%"Empty .config + ng%c%s");%m% %r%;}V=W.To%b%();}var J=O(@"$(ngpath)")??"%c%s";var A=@"$(proxycfg)";%h%%i% B in Enum.GetValues(typeof(%s%Type)).Cast^<%s%Type^>()){try{%t%.%s%^|=B;}catch(NotSupported%z%){}}if("$(ssl3)"!="true")%t%.%s%^&=~(%s%Type)(48^|192^|768);Func^<%b%,WebProxy^>C=Z=^>{var d=Z.Split('@'%f%d.%q%^<=1)%m% new WebProxy(d[0],%r%%a% e=d[0].Split(':');%m% new WebProxy(d[1],%r%){%u%s=new Network%u%(e[0],e.%q%^>1?e[1]:null)};};Func^<%b%,%b%^>f=g=^>{var h=%o%.Get%v%Name(g%f%!%v%.%]%(h))%v%%w%%v%(h);%m% g;};Func^<%b%,%b%,%b%,%b%,bool^>i=(j,k,g,H)=^>{var l=%o%.GetFull%o%(%o%.%p%(Q,g??k??"")%a% m=%:%[0]=='t';if(m^&^&%v%.%]%(l))%v%.Delete(l,true%f%%v%.%]%(l)^|^|File.%]%(l)){if(U)%_%k+" use "+l);%m% true;}if(U)%x%j+" ... "%a% n=%:%=="grab";var o=n?f(l):%o%.%p%(%o%.GetTemp%o%(),Guid.NewGuid().To%b%());%y%%i% p=new WebClient()){try{if(!%b%%g%Empty(A)){p.Proxy=C(A);}p.Headers.Add("User-Agent","%-%/$(%-%)");p.UseDefault%u%s=true;if(p.Proxy!%k%^&^&p.Proxy.%u%s=%k%){p.Proxy.%u%s=%u%Cache.Default%u%s;}if(%d%.Get%d%Variable("ngserver")!%k%^|^|%d%.Get%d%Variable("proxycfg")!%k%)throw new %z%("denied");p.DownloadFile((O(@"$(ngserver)")??"https://www.nuget.org/api/v2/%c%/")+j,o);}catch(%z% q){%l%q.Message);%m% %r%;}}if(U)%_%l%f%H!%k%){%x%H+" ... ");%y%%i% r=%+%.Security.Cryptography.SHA1%w%()){W.Clear();%y%%i% s=new FileStream(o,(%$%)3,(%#%)1))%h%%i% t in r.ComputeHash(s))W.%n%(t.To%b%("x2"));%x%W.To%b%()%f%!W.To%b%().Equals(H,(%b%%@%)5)){%_%"[x]");%m% %r%;}%_%);}}if(n)%m% true;%y%%i% D=ZipPackage.Open(o,(%$%)3,(%#%)1)){%h%%i% u in D.GetParts()%e% v=Uri.UnescapeData%b%(u.Uri.Original%b%%f%L.Any(w=^>v.%`%(w,(%b%%@%)4))%?% x=%o%.%p%(l,v.TrimStart('/'));M("- "+v);%y%%i% y=u.GetStream((%$%)3,(%#%)1))%y%%i% z=File.OpenWrite(f(x))){try{y.CopyTo(z);}catch(FileFormat%z%){M("[x]?crc: ">>%ef%
<nul set/P=+x);}}}}File.Delete(o%f%%:%!="get"%e% a=l+"/.pkg.install."+(%o%.VolumeSeparatorChar==':'?"bat":"sh"%f%File.%]%(a)){M(%:%+" "+a%a% b=Process.Start(new ProcessStartInfo(a,"1 "+%:%+" \""+Q+"\" \""+l+"\""){UseShellExecute=%r%});b.WaitForExit();b.Dispose();}if(m){%v%.Delete(l,true%f%%v%.GetDirectories(l+"/../").%q%^<1)%v%.Delete(l+"/../");}}%m% true;};%h%%i% D in V.Split(';')){if(D==""%?% c=D.Split(new[]{':'},2%a% j=c[0].Split(new[]{'?'},2%a% g=c.%q%^>1?c[1]:null;var k=j[0].Replace('/','.').Trim();if(!%b%%g%Empty(J)){g=%o%.%p%(J,g??k);}if(!i(j[0],k,g,j.%q%^>1?j[1]:null)^&^&"$(break)"!="no")%m% %r%;}}else if(%:%=="pack"%e% _=".nuspec";var h=%o%.%p%(Q,@"$(ngin)"%f%!%v%.%]%(h)){%l%h+K);%m% %r%;}var KK=%v%.GetFiles(h,"*"+_).FirstOrDefault(%f%KK=%k%){%l%_+K+h);%m% %r%;}%_%_+" use "+KK%a% KL=XDocument.Load(KK).Root.Elements().FirstOrDefault(w=^>w.Name.LocalName=="metadata"%f%KL=%k%){%l%"metadata"+K);%m% %r%;}var KM=new %+%.Collections.Generic.Dictionary^<%b%,%b%^>();Func^<%b%,%b%^>KN=KO=^>KM.ContainsKey(KO)?KM[KO]:"";%h%%i% KP in KL.Elements())KM[KP.Name.LocalName.ToLower()]=KP.Value;if(!%+%.Text.RegularExpressions.Regex.IsMatch(KN(S),@"^\w+(?:[_.-]\w+)*$")){%l%"Invalid id");%m% %r%;}var KQ=KN(S)+"."+KN(T)+".nupkg";var KR=%o%.%p%(Q,@"$(ngout)"%f%!%b%%g%WhiteSpace(KR)){if(!%v%.%]%(KR)){%v%%w%%v%(KR);}KQ=%o%.%p%(KR,KQ);}%_%"Pack ... "+KQ);%y%%i% D=Package.Open(KQ,(%$%)2)%e% KS=new Uri("/"+KN(S)+_,(UriKind)2);D%w%Relationship(KS,0,"%!%packaging/2010/07/manifest");%h%%i% KT in %v%.GetFiles(h,"*.*",(SearchOption)1)){if(L.Any(w=^>KT.%`%(%o%.%p%(h,w.Trim('/')),(%b%%@%)4))%?% KU=KT.%`%(h,(%b%%@%)5)?KT.Substring(h.%q%).TrimStart(%o%.%v%SeparatorChar):KT;M("+ "+KU%a% u=D%w%Part(PackUriHelper%w%PartUri(new Uri(%b%.Join("/",KU.Split('\\','/').Select(Uri.EscapeData%b%)),(UriKind)2)),"application/octet",(CompressionOption)1);%y%%i% KV=u.GetStream())%y%%i% KW=new FileStream(KT,(%$%)3,(%#%)1)){KW.CopyTo(KV);}}var KX=D.PackageProperti>>%ef%
<nul set/P=es;KX.Creator=KN("authors");KX.Description=KN("description");KX.Identifier=KN(S);KX.Version=KN(T);KX.Keywords=KN("tags");KX.Title=KN("title");KX.LastModifiedBy="%-%/$(%-%)";}}else %m% %r%;]]^>^</Code^>^</Task^>^</%{%Task^>^</Project^>>>%ef%
endlocal&exit/B0
:ep
set eg=%*&exit/B0
:er
call :ep !eg:~1!&if "!eg:~0,1!" NEQ "/" if "!eg:~0,1!" NEQ "-" if "!eg!" NEQ "" set eg=!eg! /t:%1&exit/B0
set eg=!ej!;logo=no !eg! /t:%1&exit/B0
:eq
if defined eg set eg=!eg:%~1=!
if "%~2" NEQ "" shift&goto eq
exit/B0