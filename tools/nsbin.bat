@echo off

set dll=%1
set namespace=%2

powershell -NonInteractive -NoProfile -NoLogo -Command "& { . %~dp0/nsbin.ps1; defNS %dll% %namespace% }"