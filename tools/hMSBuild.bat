@echo off
:: hMSBuild [Minified version] - v1.2.2.62992 [ 3ee58c3 ]
:: Copyright (c) 2017  Denis Kuzmin [ entry.reg@gmail.com ]
:: https://github.com/3F/hMSBuild
setlocal enableDelayedExpansion
set aa=1.0.62
set ab=%temp%\hMSBuild_vswhere
set /a ac=0
set /a ad=0
set /a ae=0
set /a af=0
set /a ag=0
set /a ah=0
set "ai="
set "aj="
set ak=0
set al=2
set am=3
set "an=%* "
set ao=%an:"=%
set ap=%ao%
set ap=%ap:-help =%
set ap=%ap:-h =%
set ap=%ap:-? =%
if not "%ao%"=="%ap%" goto a_
goto ba
:a_
echo.
@echo :: hMSBuild [Minified version] - v1.2.2.62992 [ 3ee58c3 ]
@echo Copyright (c) 2017  Denis Kuzmin [ entry.reg@gmail.com :: github.com/3F ]
echo Distributed under the MIT license
@echo https://github.com/3F/hMSBuild 
echo.
@echo.
@echo Usage: hMSBuild [args to hMSBuild] [args to msbuild.exe or GetNuTool core]
echo ------
echo.
echo Arguments:
echo ----------
echo  -novswhere             - Do not search via vswhere.
echo  -novs                  - Disable searching from Visual Studio.
echo  -nonet                 - Disable searching from .NET Framework.
echo  -vswhere-version {num} - Specific version of vswhere. Where {num}:
echo                           * Versions: 1.0.50 ...
echo                           * Keywords: 
echo                             `latest` to get latest available version; 
echo                             `local`  to use only local versions: 
echo                                      (.bat;.exe /or from +15.2.26418.1 VS-build);
echo.
echo  -nocachevswhere        - Do not cache vswhere. Use this also for reset cache.
echo  -notamd64              - To use 32bit version of found msbuild.exe if it's possible.
echo  -eng                   - Try to use english language for all build messages.
echo  -GetNuTool {args}      - Access to GetNuTool core. https://github.com/3F/GetNuTool
echo  -only-path             - Only display fullpath to found MSBuild.
echo  -debug                 - To show additional information from hMSBuild.
echo  -version               - Display version of hMSBuild.
echo  -help                  - Display this help. Aliases: -help -h -?
echo.
echo. 
echo -------- 
echo Samples:
echo -------- 
echo hMSBuild -vswhere-version 1.0.50 -notamd64 "Conari.sln" /t:Rebuild
echo hMSBuild -vswhere-version latest "Conari.sln"
echo.
echo hMSBuild -novswhere -novs -notamd64 "Conari.sln"
echo hMSBuild -novs "DllExport.sln"
echo hMSBuild vsSolutionBuildEvent.sln
echo.
echo hMSBuild -GetNuTool -unpack
echo hMSBuild -GetNuTool /p:ngpackages="Conari;regXwild"
echo.
echo "hMSBuild -novs "DllExport.sln" || goto err"
echo.
echo ---------------------
echo Possible Error Codes: ERROR_FILE_NOT_FOUND (0x2), ERROR_PATH_NOT_FOUND (0x3), ERROR_SUCCESS (0x0)
echo ---------------------
echo.
exit /B 0
:ba
call :bb an _is
if [!_is!]==[1] goto bc
set /a aq=1 & set ar=12
:bd
if "!an:~0,11!"=="-GetNuTool " (
call :be %1 & shift
goto bf
)
if "!an:~0,11!"=="-novswhere " (
call :be %1 & shift
set af=1
)
if "!an:~0,16!"=="-nocachevswhere " (
call :be %1 & shift
set ag=1
)
if "!an:~0,6!"=="-novs " (
call :be %1 & shift
set ad=1
)
if "!an:~0,7!"=="-nonet " (
call :be %1 & shift
set ae=1
)
if "!an:~0,16!"=="-vswhereVersion " set as=1
if "!an:~0,17!"=="-vswhere-version " set as=1
if defined as (
set "as="
call :be %1 & shift
set aj=%2
echo selected new vswhere version: !aj!
call :be %2 & shift
)
if "!an:~0,10!"=="-notamd64 " (
call :be %1 & shift
set ac=1
)
if "!an:~0,5!"=="-eng " (
call :be %1 & shift
chcp 437 >nul
)
if "!an:~0,11!"=="-only-path " (
call :be %1 & shift
set ai=1
)
if "!an:~0,7!"=="-debug " (
call :be %1 & shift
set ah=1
)
if "!an:~0,9!"=="-version " (
@echo hMSBuild [Minified version] - v1.2.2.62992 [ 3ee58c3 ]
exit /B 0
)
set /a "aq+=1"
if !aq! LSS %ar% goto bd
goto bc
:be
set an=!!an:%1 ^=!!
call :bg an
set "an=!an! "
exit /B 0
:bc
if "!ag!"=="1" (
call :bh "resetting cache of vswhere"
rmdir /S/Q "%ab%" 2>nul
)
if not "!af!"=="1" if not "!ad!"=="1" (
call :bi
if "!ERRORLEVEL!"=="0" goto bj
)
if not "!ad!"=="1" (
call :bk
if "!ERRORLEVEL!"=="0" goto bj
)
if not "!ae!"=="1" (
call :bl
if "!ERRORLEVEL!"=="0" goto bj
)
echo MSBuild tools was not found. Try to use other settings. Use key `-help` for details.
exit /B %al%
:bj
call :bb a0 _is
if [!_is!]==[1] (
echo Something went wrong. Use `-debug` key for details.
exit /B %al%
)
if defined ai (
echo !a0!
exit /B 0
)
set at="!a0!"
echo hMSBuild: !at! 
call :bh "Arguments: !an!"
!at! !an!
exit /B %ERRORLEVEL%
:bi
call :bh "trying via vswhere..."
if defined aj (
if not "!aj!"=="local" (
call :bm
call :bn
exit /B !ERRORLEVEL!
)
)
call :bo
if "!ERRORLEVEL!"=="%am%" (
if "!aj!"=="local" (
exit /B %am%
)
call :bm
)
call :bn
exit /B !ERRORLEVEL!
:bo
if exist "%~dp0vswhere.bat" set au="%~dp0vswhere" & exit /B 0
if exist "%~dp0vswhere.exe" set au="%~dp0vswhere" & exit /B 0
set av=Microsoft Visual Studio\Installer
if exist "%ProgramFiles(x86)%\!av!" set au="%ProgramFiles(x86)%\!av!\vswhere" & exit /B 0
if exist "%ProgramFiles%\!av!" set au="%ProgramFiles%\!av!\vswhere" & exit /B 0
call :bh "local vswhere is not found."
exit /B %am%
:bm
if "!ag!"=="1" (
set aw=%temp%\%random%%random%vswhere
) else (
set aw=%ab%
)
call :bh "tvswhere: !aw!"
if "!aj!"=="latest" (
set ax=vswhere
) else (
set ax=vswhere/!aj!
)
call :bh "vswpkg: !ax!"
if "!ah!"=="1" (
call :bp /p:ngpackages="!ax!:vswhere" /p:ngpath="!aw!"
) else (
call :bp /p:ngpackages="!ax!:vswhere" /p:ngpath="!aw!" >nul
)
set au="!aw!\vswhere\tools\vswhere"
exit /B 0
:bn
call :bh "vswbin: "!au!""
for /f "usebackq tokens=1* delims=: " %%a in (`!au! -latest -requires Microsoft.Component.MSBuild`) do (
if /i "%%a"=="installationPath" set ay=%%b
if /i "%%a"=="installationVersion" set az=%%b
)
call :bh "vspath: !ay!"
call :bh "vsver: !az!"
if defined aw (
if "!ag!"=="1" (
call :bh "reset vswhere"
rmdir /S/Q "!aw!"
)
)
if [!az!]==[] (
call :bh "VS2017+ was not found via vswhere"
exit /B %am%
)
for /f "tokens=1,2 delims=." %%a in ("!az!") do (
set az=%%a.0
)
set a0=!ay!\MSBuild\!az!\Bin
call :bh "found path to msbuild: !a0!"
if exist "!a0!\amd64" (
call :bh "found /amd64"
set a0=!a0!\amd64
)
call :bq 
exit /B 0
:bk
call :bh "trying via MSBuild tools from Visual Studio - 2015, 2013, ..."
for %%v in (14.0, 12.0) do (
call :br %%v Y & if [!Y!]==[1] exit /B 0
)
call :bh "msbvsold: unfortunately we didn't find anything."
exit /B %al%
:bl
call :bh "trying via MSBuild tools from .NET Framework - .net 4.0, ..."
for %%v in (4.0, 3.5, 2.0) do (
call :br %%v Y & if [!Y!]==[1] exit /B 0
)
call :bh "msbnetf: unfortunately we didn't find anything."
exit /B %al%
:br
call :bh "checking of version: %1"
for /F "usebackq tokens=2* skip=2" %%a in (
`reg query "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSBuild\ToolsVersions\%1" /v MSBuildToolsPath 2^> nul`
) do if exist %%b (
call :bh "found: %%b"
set a0=%%b
call :bq
set /a %2=1
exit /B 0
)
set /a %2=0
exit /B 0
:bf
call :bh "direct access to GetNuTool..."
call :bp !an!
exit /B 0
:bq
set a0=!a0!\MSBuild.exe
if not "!ac!" == "1" (
exit /B 0
)
set a1=!a0:Framework64=Framework!
set a1=!a1:amd64=!
if exist "!a1!" (
call :bh "Return 32bit version of MSBuild.exe because you wanted this via -notamd64"
set a0=!a1!
exit /B 0
)
call :bh "We know that 32bit version of MSBuild.exe is important for you, but we found only this."
exit /B 0
:bh
if "!ah!"=="1" (
set a2=%1
set a2=!a2:~0,-1! 
set a2=!a2:~1!
echo.[%TIME% ] !a2!
)
exit /B 0
:bg
call :a4 %%%1%%
set %1=%a3%
exit /B 0
:a4
set "a3=%*"
exit /B 0
:bb
setlocal enableDelayedExpansion
set "a4=!%1!"
if not defined a4 endlocal & set /a %2=1 & exit /B 0
set a4=%a4: =%
set "a4= %a4%"
if [^%a4:~1,1%]==[] endlocal & set /a %2=1 & exit /B 0
endlocal & set /a %2=0
exit /B 0
:bp
setlocal disableDelayedExpansion 
@echo off
:: GetNuTool - Executable version
:: Copyright (c) 2015-2017  Denis Kuzmin [ entry.reg@gmail.com ]
:: https://github.com/3F/GetNuTool
set a5=gnt.core
set a6="%temp%\%random%%random%%a5%"
set "an=%* "
set a=%an:~0,30%
set a=%a:"=%
if "%a:~0,8%"=="-unpack " goto bs
if "%a:~0,9%"=="-msbuild " goto bt
for %%v in (4.0, 14.0, 12.0, 3.5, 2.0) do (
for /F "usebackq tokens=2* skip=2" %%a in (
`reg query "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSBuild\ToolsVersions\%%v" /v MSBuildToolsPath 2^> nul`
) do if exist %%b (
set a7="%%b\MSBuild.exe"
goto bu
)
)
echo MSBuild was not found, try: gnt -msbuild "fullpath" args 1>&2
exit /B 2
:bt
call :bv %1
shift
set a7=%1
call :bv %1
:bu
call :bw
%a7% %a6% /nologo /p:wpath="%~dp0/" /v:m %an%
del /Q/F %a6%
exit /B 0
:bv
call set an=%%an:%1 ^=%%
exit /B 0
:bs
set a6="%~dp0\%a5%"
echo Generate minified version in %a6% ...
:bw
<nul set /P ="">%a6%
<nul set /P =^<!-- GetNuTool - github.com/3F/GetNuTool --^>^<!-- Copyright (c) 2015-2017  Denis Kuzmin [ entry.reg@gmail.com ] --^>^<Project ToolsVersion="4.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003"^>^<PropertyGroup^>^<ngconfig Condition="'$(ngconfig)' == ''"^>packages.config^</ngconfig^>^<ngserver Condition="'$(ngserver)' == ''"^>https://www.nuget.org/api/v2/package/^</ngserver^>^<ngpackages Condition="'$(ngpackages)' == ''"^>^</ngpackages^>^<ngpath Condition="'$(ngpath)' == ''"^>packages^</ngpath^>^</PropertyGroup^>^<Target Name="get" BeforeTargets="Build" DependsOnTargets="header"^>^<PrepareList config="$(ngconfig)" plist="$(ngpackages)" wpath="$(wpath)"^>^<Output PropertyName="plist" TaskParameter="Result"/^>^</PrepareList^>^<NGDownload plist="$(plist)" url="$(ngserver)" wpath="$(wpath)" defpath="$(ngpath)" debug="$(debug)"/^>^</Target^>^<Target Name="pack" DependsOnTargets="header"^>^<NGPack dir="$(ngin)" dout="$(ngout)" wpath="$(wpath)" vtool="$(GetNuTool)" debug="$(debug)"/^>^</Target^>^<PropertyGroup^>^<TaskCoreDllPath Condition="Exists('$(MSBuildToolsPath)\Microsoft.Build.Tasks.v$(MSBuildToolsVersion).dll')"^>$(MSBuildToolsPath)\Microsoft.Build.Tasks.v$(MSBuildToolsVersion).dll^</TaskCoreDllPath^>^<TaskCoreDllPath Condition="'$(TaskCoreDllPath)' == '' and Exists('$(MSBuildToolsPath)\Microsoft.Build.Tasks.Core.dll')"^>$(MSBuildToolsPath)\Microsoft.Build.Tasks.Core.dll^</TaskCoreDllPath^>^</PropertyGroup^>^<UsingTask TaskName="PrepareList" TaskFactory="CodeTaskFactory" AssemblyFile="$(TaskCoreDllPath)"^>^<ParameterGroup^>^<config Parame>> %a6%
<nul set /P =terType="System.String" Required="true"/^>^<plist ParameterType="System.String"/^>^<wpath ParameterType="System.String"/^>^<Result ParameterType="System.String" Output="true"/^>^</ParameterGroup^>^<Task^>^<Reference Include="System.Xml"/^>^<Reference Include="System.Xml.Linq"/^>^<Using Namespace="System"/^>^<Using Namespace="System.Collections.Generic"/^>^<Using Namespace="System.IO"/^>^<Using Namespace="System.Xml.Linq"/^>^<Code Type="Fragment" Language="cs"^>^<![CDATA[if(!String.IsNullOrEmpty(plist)){Result=plist;return true;}var _err=Console.Error;Action^<string,Queue^<string^>^> h=delegate(string cfg,Queue^<string^> list){foreach(var pkg in XDocument.Load(cfg).Descendants("package")){var id=pkg.Attribute("id");var version=pkg.Attribute("version");var output=pkg.Attribute("output");if(id==null){_err.WriteLine("Some 'id' does not exist in '{0}'",cfg);return;}var link=id.Value;if(version!=null){link+="/"+version.Value;}if(output!=null){list.Enqueue(link+":"+output.Value);continue;}list.Enqueue(link);}};var ret=new Queue^<string^>();foreach(var cfg in config.Split('^|',';')){var lcfg=Path.Combine(wpath,cfg??"");if(File.Exists(lcfg)){h(lcfg,ret);}else{_err.WriteLine(".config '{0}' was not found.",lcfg);}}if(ret.Count ^< 1){_err.WriteLine("List of packages is empty. Use .config or /p:ngpackages=\"...\"\n");}else{Result=String.Join(";",ret.ToArray());}]]^>^</Code^>^</Task^>^</UsingTask^>^<UsingTask TaskName="NGDownload" TaskFactory="CodeTaskFactory" AssemblyFile="$(TaskCoreDllPath)"^>^<ParameterGroup^>^<plist ParameterType="System.String"/^>^<url Paramet>> %a6%
<nul set /P =erType="System.String" Required="true"/^>^<wpath ParameterType="System.String"/^>^<defpath ParameterType="System.String"/^>^<debug ParameterType="System.Boolean"/^>^</ParameterGroup^>^<Task^>^<Reference Include="WindowsBase"/^>^<Using Namespace="System"/^>^<Using Namespace="System.IO"/^>^<Using Namespace="System.IO.Packaging"/^>^<Using Namespace="System.Net"/^>^<Code Type="Fragment" Language="cs"^>^<![CDATA[if(plist==null){return false;}var ignore=new string[]{"/_rels/","/package/","/[Content_Types].xml"};Action^<string,object^> dbg=delegate(string s,object p){if(debug){Console.WriteLine(s,p);}};Func^<string,string^> loc=delegate(string p){return Path.Combine(wpath,p??"");};Action^<string,string,string^> get=delegate(string link,string name,string path){var to=Path.GetFullPath(loc(path??name));if(Directory.Exists(to)){Console.WriteLine("`{0}` is already exists: \"{1}\"",name,to);return;}Console.Write("Getting `{0}` ... ",link);var tmp=Path.Combine(Path.GetTempPath(),Guid.NewGuid().ToString());using(var l=new WebClient()){try{l.Headers.Add("User-Agent","GetNuTool");l.UseDefaultCredentials=true;l.DownloadFile(url+link,tmp);}catch(Exception ex){Console.Error.WriteLine(ex.Message);return;}}Console.WriteLine("Extracting into \"{0}\"",to);using(var pkg=ZipPackage.Open(tmp,FileMode.Open,FileAccess.Read)){foreach(var part in pkg.GetParts()){var uri=Uri.UnescapeDataString(part.Uri.OriginalString);if(ignore.Any(x=^> uri.StartsWith(x,StringComparison.Ordinal))){continue;}var dest=Path.Combine(to,uri.TrimStart('/'));dbg("- `{0}`",uri);var dir=Path.GetDirectoryNam>> %a6%
<nul set /P =e(dest);if(!Directory.Exists(dir)){Directory.CreateDirectory(dir);}using(var src=part.GetStream(FileMode.Open,FileAccess.Read))using(var target=File.OpenWrite(dest)){try{src.CopyTo(target);}catch(FileFormatException ex){dbg("[x]?crc: {0}",dest);}}}}File.Delete(tmp);};foreach(var pkg in plist.Split(';')){var ident=pkg.Split(':');var link=ident[0];var path=(ident.Length ^> 1)?ident[1]: null;var name=link.Replace('/','.');if(!String.IsNullOrEmpty(defpath)){path=Path.Combine(defpath,path??name);}get(link,name,path);}]]^>^</Code^>^</Task^>^</UsingTask^>^<UsingTask TaskName="NGPack" TaskFactory="CodeTaskFactory" AssemblyFile="$(TaskCoreDllPath)"^>^<ParameterGroup^>^<dir ParameterType="System.String" Required="true"/^>^<dout ParameterType="System.String"/^>^<wpath ParameterType="System.String"/^>^<vtool ParameterType="System.String" Required="true"/^>^<debug ParameterType="System.Boolean"/^>^</ParameterGroup^>^<Task^>^<Reference Include="System.Xml"/^>^<Reference Include="System.Xml.Linq"/^>^<Reference Include="WindowsBase"/^>^<Using Namespace="System"/^>^<Using Namespace="System.Collections.Generic"/^>^<Using Namespace="System.IO"/^>^<Using Namespace="System.Linq"/^>^<Using Namespace="System.IO.Packaging"/^>^<Using Namespace="System.Xml.Linq"/^>^<Using Namespace="System.Text.RegularExpressions"/^>^<Code Type="Fragment" Language="cs"^>^<![CDATA[var EXT_NUSPEC=".nuspec";var EXT_NUPKG=".nupkg";var TAG_META="metadata";var DEF_CONTENT_TYPE="application/octet";var MANIFEST_URL="http://schemas.microsoft.com/packaging/2010/07/manifest";var ID="id";var VER="version">> %a6%
<nul set /P =;Action^<string,object^> dbg=delegate(string s,object p){if(debug){Console.WriteLine(s,p);}};var _err=Console.Error;dir=Path.Combine(wpath,dir);if(!Directory.Exists(dir)){_err.WriteLine("`{0}` was not found.",dir);return false;}dout=Path.Combine(wpath,dout??"");var nuspec=Directory.GetFiles(dir,"*"+EXT_NUSPEC,SearchOption.TopDirectoryOnly).FirstOrDefault();if(nuspec==null){_err.WriteLine("{0} was not found in `{1}`",EXT_NUSPEC,dir);return false;}Console.WriteLine("Found {0}: `{1}`",EXT_NUSPEC,nuspec);var root=XDocument.Load(nuspec).Root.Elements().FirstOrDefault(x=^> x.Name.LocalName==TAG_META);if(root==null){_err.WriteLine("{0} does not contain {1}.",nuspec,TAG_META);return false;}var metadata=new Dictionary^<string,string^>();foreach(var tag in root.Elements()){metadata[tag.Name.LocalName.ToLower()]=tag.Value;}if(metadata[ID].Length ^> 100 ^|^|!Regex.IsMatch(metadata[ID],@"^\w+([_.-]\w+)*$",RegexOptions.IgnoreCase ^| RegexOptions.ExplicitCapture)){_err.WriteLine("The format of `{0}` is not correct.",ID);return false;}var ignore=new string[]{Path.Combine(dir,"_rels"),Path.Combine(dir,"package"),Path.Combine(dir,"[Content_Types].xml")};var pout=String.Format("{0}.{1}{2}",metadata[ID],metadata[VER],EXT_NUPKG);if(!String.IsNullOrWhiteSpace(dout)){if(!Directory.Exists(dout)){Directory.CreateDirectory(dout);}pout=Path.Combine(dout,pout);}Console.WriteLine("Started packing `{0}` ...",pout);using(var pkg=Package.Open(pout,FileMode.Create)){var manifestUri=new Uri(String.Format("/{0}{1}",metadata[ID],EXT_NUSPEC),UriKind.Relative);pkg.CreateRelationship(manif>> %a6%
<nul set /P =estUri,TargetMode.Internal,MANIFEST_URL);foreach(var file in Directory.GetFiles(dir,"*.*",SearchOption.AllDirectories)){if(ignore.Any(x=^> file.StartsWith(x,StringComparison.Ordinal))){continue;}string pUri;if(file.StartsWith(dir,StringComparison.OrdinalIgnoreCase)){pUri=file.Substring(dir.Length).TrimStart(Path.DirectorySeparatorChar);}else{pUri=file;}dbg("- `{0}`",pUri);var escaped=String.Join("/",pUri.Split('\\','/').Select(p=^> Uri.EscapeDataString(p)));var uri=PackUriHelper.CreatePartUri(new Uri(escaped,UriKind.Relative));var part=pkg.CreatePart(uri,DEF_CONTENT_TYPE,CompressionOption.Maximum);using(var tstream=part.GetStream())using(var fs=new FileStream(file,FileMode.Open,FileAccess.Read)){fs.CopyTo(tstream);}}Func^<string,string^> getmeta=delegate(string key){return(metadata.ContainsKey(key))?metadata[key]:"";};var _p=pkg.PackageProperties;_p.Creator=getmeta("authors");_p.Description=getmeta("description");_p.Identifier=metadata[ID];_p.Version=metadata[VER];_p.Keywords=getmeta("tags");_p.Title=getmeta("title");_p.LastModifiedBy="GetNuTool v"+vtool;}]]^>^</Code^>^</Task^>^</UsingTask^>^<Target Name="Build" DependsOnTargets="get"/^>^<PropertyGroup^>^<GetNuTool^>1.6.1.10620_bde3e50^</GetNuTool^>^<wpath Condition="'$(wpath)' == ''"^>$(MSBuildProjectDirectory)^</wpath^>^</PropertyGroup^>^<Target Name="header"^>^<Message Text="%%0D%%0AGetNuTool v$(GetNuTool) - github.com/3F%%0D%%0A=========%%0D%%0A" Importance="high"/^>^</Target^>^</Project^>>> %a6%
exit /B 0