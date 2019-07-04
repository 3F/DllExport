@echo off

:: Tests for DllExport Manager (batch version)
:: https://github.com/3F/DllExport

set "mgrFile=%1"
set "flagName=testCase"

if not defined mgrFile (
    set "mgrFile=DllExport.bat"
)

if not exist %mgrFile% (
    echo DllExport Manager was not found for tests: %mgrFile%
    echo Usage: %~nx0 [file]
    exit /B 2
)

set "tmapFile=%mgrFile%.map.targets"

setlocal
set __p_call=1
set app=%mgrFile% -tests "tests.targets"

:: NOTE: $version$ managed via vssbe scripts
set appl=%app% -pkg-link "..\..\DllExport.$version$.nupkg"

    setlocal 
        echo Test case 1: -action flag & set "%flagName%=1"

        call %appl% -action Configure

    endlocal

    setlocal 
        echo Test case 2: -action flag & set "%flagName%=2"

        call %appl% -action Update

    endlocal

    setlocal 
        echo Test case 3: -action flag  & set "%flagName%=3"

        call %appl% -action Restore

    endlocal

    setlocal 
        echo Test case 4: -action flag & set "%flagName%=4"

        call %appl% -action Export

    endlocal

    setlocal 
        echo Test case 5: -action flag & set "%flagName%=5"

        call %appl% -action Recover

    endlocal

    setlocal 
        echo Test case 6: -action flag & set "%flagName%=6"

        call %appl% -action Unset

    endlocal

    setlocal 
        echo Test case 7: -action flag & set "%flagName%=7"

        call %appl% -action Upgrade

    endlocal

    setlocal 
        echo Test case 8: before/after -action & set "%flagName%=1"

        call %appl% -force -action Configure -eng

    endlocal

    setlocal 
        echo Test case 9: -sln-dir {path} without double quotes & set "%flagName%=9"

        call %appl% -action Configure -sln-dir directory123

    endlocal

    setlocal 
        echo Test case 10: -sln-dir {path} with double quotes & set "%flagName%=10"

        call %appl% -action Configure -sln-dir "directory 123"

    endlocal

    setlocal 
        echo Test case 11: -sln-dir {path} with special symbols & set "%flagName%=11"

        rem possible: ~`!@#$%^&()_+=-;'[]{}.,
        rem not allowed: *?"<>|

        rem passed via real build for msbuild: crazy' dir&name!~`@$#^(+);_=-[.]%{,}

        call %appl% -action Configure -sln-dir "crazy' dir&name!356~`@#$^(+)_=-;[.]{,}" -eng

    endlocal

    setlocal 
        echo Test case 12: -sln-dir {path} for %% symbol & set "%flagName%=12"

        rem because of calling type %% + %% is needed.

        :: [I] from scripts:
        :: # call DllExport  %%%% - %
        :: # call DllExport  %%   - empty
        :: # call DllExport  %    - empty
        :: # DllExport       %%   - %
        :: # DllExport       %    - empty
                    
        :: [II] from command-line:
        :: # call DllExport   %  -  %
        :: # DllExport        %  -  %
        
        call %appl% -action Configure -sln-dir "any %%%% data %%%% 123" -eng

    endlocal

    setlocal 
        echo Test case 13: special symbols for -sln-file, -metalib, -dxp-target & set "%flagName%=13"

        set _arg="crazy' dir&name!356~`@#$^(+)_=-;[.]{,}"
        
        call %appl% -action Configure -sln-file %_arg% -metalib %_arg% -dxp-target %_arg%

    endlocal

    setlocal 
        echo Test case 14: -sln-dir {path} for path \/: symbols & set "%flagName%=14"
                
        call %appl% -action Configure -sln-dir "D:\\dir1/dir2/" -eng

    endlocal

    setlocal 
        echo Test case 15: special symbols for -packages & set "%flagName%=15"

        rem 1.6.1 uses this hack https://github.com/3F/GetNuTool/issues/6
        ::   that does not allow ';' symbols in paths.
        ::   i.e. while -packages will accept correct data the gnt.core $version$ will parse this incorrectly like: Could not find a part of the path ...

        set _arg="crazy' dir&name!356~`@#$^(+)_=-;[.]{,}"
        
        call %appl% -action Configure -packages %_arg%

    endlocal

    setlocal 
        echo Test case 16: checking for -dxp-version, -server, -proxy  & set "%flagName%=16"
        
        call %mgrFile% -action Default -dxp-version 1.6.0
        call %app% -action Default -dxp-version 1.6.0 -server "https://127.0.0.1:8082/" -proxy "guest:1234@127.0.0.1:7428" 
        :: -pe-exp-list "bin\Debug\regXwild.dll"

    endlocal
    

endlocal