/* 
    https://github.com/3F/DllExport/issues/2#issuecomment-231593744

    Offset(h) 00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F

    000005B0                 00 C4 7B 01 00 00 00 2F 00 12 05       .Д{..../...
    000005C0  00 00 02 00 00 00 00 00 00 00 00 00 00 00 26 00  ..............&.
    000005D0  20 02 00 00 00 00 00 00 00 44 33 46 30 30 46 46   ........D3F00FF
    000005E0  31 37 37 30 44 45 44 39 37 38 45 43 37 37 34 42  1770DED978EC774B
    000005F0  41 33 38 39 46 32 44 43 39 2E 42 30 30 30 30 30  A389F2DC9.B00000
    00000600  30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30  0000000000000000
    ...
    000007A0  30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30  0000000000000000
    000007B0  30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30  0000000000000000
    000007C0  30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30  0000000000000000
    000007D0  30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30  0000000000000000
    000007E0  30 30 30 30 30 30 30 30 30 30 30 30 30 30 00 3C  00000000000000.<

    ->

    Offset(h) 00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F

    000005B0                 00 C4 7B 01 00 00 00 2F 00 12 05       .Д{..../...
    000005C0  00 00 02 00 00 00 00 00 00 00 00 00 00 00 26 00  ..............&.
    000005D0  20 02 00 00 00 00 00 00 00 49 2E 77 61 6E 74 2E   ........I.want.
    000005E0  74 6F 2E 66 6C 79 00 00 00 00 00 00 00 00 00 00  to.fly..........
    000005F0  00 00 00 00 00 00 03 00 00 00 00 00 00 00 00 00  ................
    00000600  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
    ...
    000007A0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
    000007B0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
    000007C0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
    000007D0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
    000007E0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3C  ...............< 
 */

using System;
using System.Runtime.InteropServices;


namespace
#if NS_RG
    RGiesecke.DllExport
#elif NS_3F
    net.r_eg.DllExport
#elif NS_RI
    System.Runtime.InteropServices
#else
    // byte-seq via chars: 
    // + Identifier        = [32]bytes
    // + size of buffer    = [ 4]bytes (range: 0000 - FFF9; reserved: FFFA - FFFF)
    // + buffer of n size
    D3F00FF1770DED978EC774BA389F2DC901F4 // 01F4 - allocated buffer size
    .B00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000 // 100
    .C00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
    .D00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
    .E00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
    .F00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
    //= 536
#endif

{
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DllExportAttribute: Attribute
    {
        public CallingConvention CallingConvention
        {
            get;
            set;
        }

        public string ExportName
        {
            get;
            set;
        }

        public DllExportAttribute(string function, CallingConvention convention)
        {
            ExportName          = function;
            CallingConvention   = convention;
        }

        public DllExportAttribute(string function)
            : this(function, CallingConvention.StdCall)
        {

        }

        public DllExportAttribute(CallingConvention convention)
            : this(null, convention)
        {

        }

        public DllExportAttribute()
        {

        }
    }
}
