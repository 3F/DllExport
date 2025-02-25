::: .NET DllExport [Offline version]
::!  https://github.com/3F/DllExport



@set defaultCommand=-action Configure





::....................................
@echo off
set _args=%*
set "_launcher=DllExport"
if not defined _args set _args=%defaultCommand%

set __dxp_pv=/Offline
"%~dp0\%_launcher%\%_launcher%" -packages "%~dp0" -sln-dir "%~dp0" -dxp-version actual %_args%
