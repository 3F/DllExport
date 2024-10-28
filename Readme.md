# [.NET DllExport](https://github.com/3F/DllExport)

*.NET DllExport* with .NET Core support (a.k.a. ***3F**/DllExport* a.k.a. *DllExport.**bat***)

[![Build status](https://ci.appveyor.com/api/projects/status/hh2oxibqoi6wrdnc/branch/master?svg=true)](https://ci.appveyor.com/project/3Fs/dllexport-ix27o/branch/master)
[![Release](https://img.shields.io/github/release/3F/DllExport.svg)](https://github.com/3F/DllExport/releases/latest)
[![License](https://img.shields.io/badge/License-MIT-74A5C2.svg)](https://github.com/3F/DllExport/blob/master/LICENSE.txt)

[`DllExport`](https://3F.github.io/DllExport/releases/latest/manager/)` -action Configure` [[?](#how-to-get-dllexport)]

```csharp
[DllExport("Init", CallingConvention.Cdecl)]
// __cdecl is the default calling convention
[DllExport(CallingConvention.StdCall)]
[DllExport("MyFunc")]
[DllExport]
```

Based on *UnmanagedExports* that was created by Robert Giesecke. His [page](https://sites.google.com/site/robertgiesecke/Home/uploads/unmanagedexports).

***.NET DllExport*** is a different [unrelated project](https://github.com/3F/DllExport/issues/87#issuecomment-438576100) that was developed by Denis Kuzmin [ 「 ❤ 」 ](https://3F.github.io/fund)

```
Copyright (c) 2009-2015  Robert Giesecke
Copyright (c) 2016-2024  Denis Kuzmin <x-3F@outlook.com> github/3F
```

> [ ***[Quick start](https://github.com/3F/DllExport/wiki/Quick-start)*** ] [ [Examples: C++, C#, Java, ...](https://github.com/3F/DllExport/wiki/Examples) ] 
> -> { **[Wiki](https://github.com/3F/DllExport/wiki)** } { [🧪 Demo src](https://github.com/3F/Examples/tree/master/DllExport/BasicExport) }

[![](https://github.com/3F/DllExport/blob/9a4bc5143b0b0ab6aca7cfc1abe3f6da9644ecd9/Resources/img/DllExport.png?raw=true)](https://3F.github.io/DllExport/releases/latest/manager/)
[![](https://github.com/3F/DllExport/blob/36d452268c1f69b5c8dd5e22cc106c71ac76a82c/Resources/img/screencast_Complex_types.jpg?raw=true)](https://www.youtube.com/watch?v=QXMj9-8XJnY)

For Lua, consider using [LuNari](https://github.com/3F/LuNari)

```csharp
[DllExport]
public static int entrypoint(IntPtr L)
{
    using Lua<ILua53> lua = new("Lua.dll");
    ...
    lua.pushcclosure(L, onProc, 0);
    lua.setglobal(L, "onKeyDown");
    LuaNumber num = lua.tonumber<LuaNumber>(L, 7);
    ...
}
```

For working with **unmanaged** memory including native or binary data from the heap and binding between .NET and unmanaged native C/C++ etc, try [Conari](https://github.com/3F/Conari) (Wiki. [**Quick-start**](https://github.com/3F/Conari/wiki/Quick-start))

```csharp
[DllExport] // DllExportModifiedClassLibrary.dll
public static IntPtr callme(TCharPtr str, IntPtr structure)
{
    if(str != "Hello world!") return IntPtr.Zero;

    structure.Native().f<int>("x", "y").build(out dynamic v);
    if(v.x > v.y)
    {
        structure.Access().write<int>(8);
    }
    return new NativeArray<int>(-1, v.x, 1, v.y);
}
```
```csharp
... // host side via C/C++, Java, Rust, Python, ... or even same dotnet C#
using NativeString<TCharPtr> ns = new("Hello world!");
using NativeStruct<Arg> nstruct = new(new Arg() { x = 7, y = 5 });

using dynamic l = new ConariX("DllExportModifiedClassLibrary.dll");
IntPtr ptr = l.callme<IntPtr>(ns, nstruct);

using NativeArray<int> nr = new(4, ptr); // (nstruct.Data.x == 8) != (nr[1] == 7)
```

*.NET DllExport* supports both Library (**.dll**) and Executable (**.exe**) PE modules.

Find demo project here: https://github.com/3F/Examples/tree/master/DllExport/BasicExport

## How does it work

Current features has been implemented through [ILDasm](https://github.com/3F/coreclr/tree/master/src/ildasm) & [ILAsm](https://github.com/3F/coreclr/tree/master/src/ilasm) that prepares all the necessary steps via `.export` directive ([it's part of the ILAsm compiler, **not** CLR](https://github.com/3F/DllExport/issues/45#issuecomment-317802099)).

**What inside ? how does work the .export directive ?**

Read about format PE32/PE32+, start with grammar from asmparse and move to writer:

```cpp
...
//yacc
if(PASM->m_pCurMethod->m_dwExportOrdinal == 0xFFFFFFFF)
{
  PASM->m_pCurMethod->m_dwExportOrdinal = $3;
  PASM->m_pCurMethod->m_szExportAlias = $6;
  if(PASM->m_pCurMethod->m_wVTEntry == 0) PASM->m_pCurMethod->m_wVTEntry = 1;
  if(PASM->m_pCurMethod->m_wVTSlot  == 0) PASM->m_pCurMethod->m_wVTSlot = $3 + 0x8000;
}
...
EATEntry*   pEATE = new EATEntry;
pEATE->dwOrdinal = pMD->m_dwExportOrdinal;
pEATE->szAlias = pMD->m_szExportAlias ? pMD->m_szExportAlias : pMD->m_szName;
pEATE->dwStubRVA = EmitExportStub(pGlobalLabel->m_GlobalOffset+dwDelta);
m_EATList.PUSH(pEATE);
...
// logic of definition of records into EXPORT_DIRECTORY (see details from PE format)
HRESULT Assembler::CreateExportDirectory()  
{
...
    IMAGE_EXPORT_DIRECTORY  exportDirIDD;
    DWORD                   exportDirDataSize;
    BYTE                   *exportDirData;
    EATEntry               *pEATE;
    unsigned                i, L, ordBase = 0xFFFFFFFF, Ldllname;
    ...
    ~ now we're ready to miracles ~
```

Read also my brief explanations here: [AssemblyRef encoding](https://github.com/3F/DllExport/issues/125#issuecomment-561245575) / [about mscoree](https://github.com/3F/DllExport/issues/45#issuecomment-317802099) / [DllMain & the export-table](https://github.com/3F/DllExport/issues/5#issuecomment-240697109) / [DllExport.dll](https://github.com/3F/DllExport/issues/28#issuecomment-281957212) / [ordinals](https://github.com/3F/DllExport/issues/8#issuecomment-245228065) ...

## How to get DllExport

[**Put this inside solution folder, then click it there**](https://3f.github.io/DllExport/releases/latest/manager/)

Since 1.6+ does not provide official support for NuGet [[?](https://github.com/3F/DllExport/wiki/DllExport-Manager-Q-A)], something somewhere may not work or not work properly.

In any case you need [DllExport.bat](https://3f.github.io/DllExport/releases/latest/manager/) (~20 KB) inside your solution folder. Read [Wiki. Quick-start](https://github.com/3F/DllExport/wiki/Quick-start)

You can get it from any *trusted* places:
* [GitHub Releases](https://github.com/3F/DllExport/releases/latest). Alias to latest stable manager: https://3F.github.io/DllExport/releases/latest/manager/
* [![NuGet package](https://img.shields.io/nuget/v/DllExport.svg)](https://www.nuget.org/packages/DllExport/) Visual Studio Package Manager can still distribute and activate *DllExport.bat* for solution folder in most default setup cases.
* You can also use [embeddable package manager *GetNuTool*](https://github.com/3F/GetNuTool) to reach a remote *DllExport.bat* or .nupkg package.

Read [ **[Documentation](https://github.com/3F/DllExport/wiki/DllExport-Manager)** ]

## Build .NET DllExport from source

```bat
git clone https://github.com/3F/DllExport.git DllExport
cd DllExport
```

Call *build.bat* to build final binaries like `DllExport.<version>.nupkg`, Manager, tests, zip-archives, and related:

```batch
.\build Release
```

Note, this relies on [vsSolutionBuildEvent](https://github.com/3F/vsSolutionBuildEvent) scripting **if** you're using Visual Studio **IDE**.

### Modified Assembler ILAsm on CoreCLR

We're using **[3F's](https://github.com/3F) modified versions** specially for *.NET DllExport* project
* https://github.com/3F/coreclr

This helps to avoid some problems [like this](https://github.com/3F/DllExport/issues/125#issuecomment-561245575), or [this](https://github.com/3F/DllExport/issues/17), and more ...

To build minimal version (it will not include all components as for original coreclr repo):

Restore git submodule or use repo: https://github.com/3F/coreclr.git

```bash
git submodule update --init --recursive
```

Make sure that you have installed [CMake](https://cmake.org/download/), then:

```bash
build-s -all -x86 -x64 Release
```

Note, you can also get the compiled via [![NuGet package](https://img.shields.io/nuget/v/ILAsm.svg)](https://www.nuget.org/packages/ILAsm/)
