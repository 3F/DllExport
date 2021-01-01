/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016-2021  Denis Kuzmin <x-3F@outlook.com> github/3F
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/

using System.Collections.Generic;
using System.Linq;

namespace net.r_eg.DllExport.Wizard
{
    /// <summary>
    /// https://github.com/3F/DllExport/pull/148
    /// </summary>
    public sealed class PostProc
    {
        private const string ENV_SLN    = "$(" + MSBuildProperties.SLN_PATH + ")";
        private const string ENV_PRJ    = "$(" + MSBuildProperties.MSB_THIS_FULL_FPATH + ")";

        [System.Flags]
        public enum CmdType: long
        {
            None,

            Custom                      = 0x1,

            Predefined                  = 0x2,

            DependentX86X64             = 0x4,

            DependentIntermediateFiles  = 0x8,

            SeqDependentForSys          = 0x10,
        }

        /// <summary>
        /// Never null environment.
        /// </summary>
        public IList<string> ProcEnv
        {
            get;
            private set;
        }

        /// <summary>
        /// Never null command.
        /// </summary>
        public string Cmd
        {
            get;
            private set;
        }

        public CmdType Type
        {
            get;
            private set;
        }

        public PostProc Configure(CmdType type = CmdType.None, string env = null, string cmd = null)
        {
            Configure(type, cmd);

            ProcEnv = InitProcEnv
            (
                type,
                GetValidValue(type, env).Split(';')
                    .Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p))
            );

            return this;
        }

        public PostProc Configure(IEnumerable<string> env, CmdType type = CmdType.None, string cmd = null)
        {
            ProcEnv = InitProcEnv(type, Configure(type, cmd).GetValidValue(type, env));
            return this;
        }

        public string GetProcEnvAsString() => string.Join(";", ProcEnv);

        private PostProc Configure(CmdType type, string cmd)
        {
            Type = type;
            Cmd = GetValidValue(type, cmd);
            return this;
        }

        private IList<string> InitProcEnv(CmdType type, IEnumerable<string> env)
        {
            var ret = new List<string>(env.Count() + 3);

            if(!env.Contains(ENV_SLN)) {
                ret.Add(ENV_SLN);
            }

            if(!env.Contains(ENV_PRJ)) {
                ret.Add(ENV_PRJ);
            }

            if((type & (CmdType.DependentX86X64 | CmdType.DependentIntermediateFiles)) != 0)
            {
                if(!env.Contains(MSBuildProperties.PRJ_TARGET_DIR)) {
                    ret.Add(MSBuildProperties.PRJ_TARGET_DIR);
                }
            }

            ret.AddRange(env);
            return ret;
        }

        private string GetValidValue(CmdType type, string val) 
            => (type == CmdType.None || val == null) ? string.Empty : val;

        private IEnumerable<string> GetValidValue(CmdType type, IEnumerable<string> val) 
            => (type == CmdType.None || val == null) ? Enumerable.Empty<string>() : val;
    }
}
