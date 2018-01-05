//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2018  Denis Kuzmin < entry.reg@gmail.com > :: github.com/3F
//$ Distributed under the MIT License (MIT)

using System.Runtime.InteropServices;

namespace RGiesecke.DllExport
{
    public interface IExportInfo
    {
        CallingConvention CallingConvention { get; set; }

        string ExportName { get; set; }

        bool IsStatic { get; }

        bool IsGeneric { get; }

        void AssignFrom(IExportInfo info, IInputValues input);
    }
}
