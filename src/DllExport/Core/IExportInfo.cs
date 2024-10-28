/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System.Runtime.InteropServices;

namespace net.r_eg.DllExport
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
