//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2019  Denis Kuzmin < entry.reg@gmail.com > GitHub/3F
//$ Distributed under the MIT License (MIT)

namespace RGiesecke.DllExport
{
    public enum PeCheckType: int
    {
        None,

        /// <summary>
        /// PE Check 1:1.
        /// Will check count of all planned exports from final PE32/PE32+ module.
        /// </summary>
        Pe1to1 = 0x01,

        /// <summary>
        /// PE Check IL code.
        /// Will check existence of all planned exports (IL code) in actual PE32/PE32+ module.
        /// </summary>
        PeIl = 0x02,
    }
}
