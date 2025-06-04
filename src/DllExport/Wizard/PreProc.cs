/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

namespace net.r_eg.DllExport.Wizard
{
    /// <summary>
    /// https://github.com/3F/DllExport/pull/146
    /// </summary>
    public sealed class PreProc
    {
        internal const string ID = Project.METALIB_PK_TOKEN + ":" + nameof(PreProc);

        /// <summary>
        /// Never null command.
        /// </summary>
        public string Cmd { get; private set; }

        public CmdType Type { get; private set; }

        [System.Flags]
        public enum CmdType: long
        {
            None        = 0x0,

            ILMerge     = 0x1,

            Conari      = 0x2,

            Exec        = 0x4,

            DebugInfo   = 0x8,

            IgnoreErr   = 0x10,

            Log         = 0x20,

            ILRepack    = 0x40,

            MergeRefPkg = 0x80,

            AnyMerging  = ILMerge | ILRepack,
        }

        public PreProc Configure(CmdType type = CmdType.None, string cmd = null)
        {
            Type    = type;
            Cmd     = (type == CmdType.None || cmd == null) ? string.Empty : cmd;

            return this;
        }
    }
}
