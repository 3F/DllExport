//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2018  Denis Kuzmin < entry.reg@gmail.com > :: github.com/3F
//$ Distributed under the MIT License (MIT)

using System;

namespace RGiesecke.DllExport
{
    [CLSCompliant(true)]
    public enum CpuPlatform
    {
        None,
        X86,
        X64,
        Itanium,
        AnyCpu,
    }
}
