@REM TODO path
set msbuild=C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe

%msbuild% gnt.core /p:ngconfig="packages.config|RGiesecke.DllExport/packages.config|RGiesecke.DllExport.MSBuild/packages.config" /nologo /verbosity:q
%msbuild% "DllExport.sln" /verbosity:normal /l:"packages\vsSBE.CI.MSBuild\bin\CI.MSBuild.dll" /m:8 /t:Build /p:Configuration=Release