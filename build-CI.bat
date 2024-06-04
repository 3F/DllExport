@echo off

setlocal
    cd tools
    call netfx4sdk -mode sys || call netfx4sdk -mode pkg
endlocal

build %*