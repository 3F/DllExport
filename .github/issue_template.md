## How to reproduce

... Try to isolate problem via very simple code.
    
## What version and selected configuration ?

... 

## Can you provide detailed log of your build ?

...

To set `detailed` or `diagnostics` level:

* For VS IDE: `Tools` - `Options` - `Project and Solutions` - `Build and Run` - MSBuild project build verbosity 
* For msbuild.exe: `/v:diag`:

```
msbuild <sln> /t:Rebuild /m:4 /v:diag > build.log
```

-----------
Please note

You can attach any data to help understand your problem, However, **do not forget about copyrights, license etc.** of your (or company) program, because this code will be available for 3rd party side, thus please check that you have required rights !
