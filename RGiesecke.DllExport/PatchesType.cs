
/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

namespace RGiesecke.DllExport
{
    public enum PatchesType: long
    {
        None,

        /// <summary>
        /// Affects ldc.r8; ldc.r4; .field;
        /// 
        /// inf/-inf to 0x7F800000/0xFF800000 
        ///             0x7FF0000000000000/0xFFF0000000000000
        /// 
        /// https://github.com/3F/DllExport/issues/128
        /// </summary>
        InfToken = 0x01,

        /// <summary>
        /// Affects ldc.r8; ldc.r4; .field;
        /// 
        /// -nan(ind) to 00 00 C0 FF
        ///              00 00 00 00 00 00 F8 FF
        ///              
        /// https://github.com/3F/DllExport/issues/158
        /// </summary>
        NaNToken = 0x02,
    }
}
