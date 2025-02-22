﻿
/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

namespace net.r_eg.DllExport.Activator
{
    public interface IInputRawValues: IInputValues
    {
        string ImageBaseRaw { set; }

        long PatchesRaw { set; }

        int PeCheckRaw { set; }

        string AssemblyExternDirectivesRaw { set; }

        string TypeRefDirectivesRaw { set; }
    }
}
