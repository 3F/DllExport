# DllExport

*Unmanaged Exports ( .NET DllExport )*

```
Copyright (c) 2009  Robert Giesecke
Copyright (c) 2016  Denis Kuzmin <entry.reg@gmail.com>
```

The original tool `UnmanagedExports` has been distributed without any source code, the only:

* [Official page](https://sites.google.com/site/robertgiesecke/Home/uploads/unmanagedexports) - *posted Jul 9, 2009 [ updated Dec 19, 2012 ]*
* [Official NuGet Packages](https://www.nuget.org/packages/UnmanagedExports) 
    * the only known [contact with author](https://www.nuget.org/packages/UnmanagedExports/ContactOwners) - but it seems also is outdated, I have also tried this.

However, not all so bad... the **original tool `UnmanagedExports`** was distributed **under the [MIT License](https://opensource.org/licenses/mit-license.php)**, so what we have finally ?

It says:
```
... the Software without restriction, including without limitation the rights to use, 
copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
and to permit persons to whom the Software is furnished to do so ...
```

Good! Trying to open this... 


## License

It still under the [MIT License (MIT)](https://github.com/3F/DllExport/blob/master/LICENSE) - be a ~free~ and open

## Build

No requires additional steps for you, just build as you need.

Use build.bat if you need final NuGet package as a `DllExport.<version>.nupkg` etc.
* *You do not need to do anything inside IDE, if you already have installed plugin.*

## How to debug

For example, find the DllExport.MSBuild project in solution:

* `Properties` > `Debug`:
    * `Start Action`: set as `Start External program`
        * Add full path to **msbuild.exe**, for example: C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe
    * `Start Options` > `Command line arguments` write for example:

```bash
"<path_to_SolutionFile_for_debugging>.sln" /t:Build /p:Configuration=<Configuration>
```

use additional `Diagnostic` key to msbuild if you need details from .targets
```bash
"<path_to_SolutionFile_for_debugging>.sln" /verbosity:Diagnostic /t:Rebuild /p:Configuration=<Configuration>
```

`Start Debugging`, Now you can debug this at runtime.
