@echo off

set reltype=%~1
if not defined reltype set reltype=Release

tools\gnt & packages\vsSolutionBuildEvent\cim.cmd ~x ~c %reltype%
