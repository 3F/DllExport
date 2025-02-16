/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace net.r_eg.DllExport.ILAsm
{
    public abstract class AsmDirectiveAbstract
    {
        public abstract string Serialize();

        public abstract AsmDirectiveAbstract Deserialize(string serialized);

        /// <returns>line by line formatted data without newline characters</returns>
        public abstract IEnumerable<string> Format();

        /// <summary>
        /// .typeref 'Name' ... <br />
        /// .assembly extern 'Name' ... <br />
        /// </summary>
        public string Name { get; set; }

        protected string Pack(params object[] values)
        {
            StringBuilder sb = new();
            for(int i = 0, n = values.Length; i < n;)
            {
                sb.Append(values[i]);
                if(++i < n) sb.Append(',');
            }
            return sb.Append(';').ToString();
        }

        protected object[] Unpack(string serialized)
        {
            if(string.IsNullOrWhiteSpace(serialized)) throw new NotSupportedException();

            string[] data = serialized.Trim([' ', ';']).Split(',');
            return data.Length < 1 ? throw new NotSupportedException() : data;
        }

        protected AsmDirectiveAbstract ReturnObjects(object[] v, int max)
        {
            return v.Length > max ? throw new NotSupportedException() : this;
        }

        protected string ToS(object raw) => raw.ToString();

        protected bool ToB(object raw) => bool.Parse(ToS(raw));
    }
}
