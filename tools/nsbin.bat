@echo off

set dll=%1
set namespace=%2

powershell -NonInteractive -NoProfile -NoLogo -Command "& { Import-Module %~dp0/NSBin.dll; Set-DDNS -Dll %dll% -Namespace %namespace% }"