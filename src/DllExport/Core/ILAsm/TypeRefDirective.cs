/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System.Collections.Generic;
using System.Text;

namespace net.r_eg.DllExport.ILAsm
{
    public sealed class TypeRefDirective: AsmDirectiveAbstract
    {
        /// <summary>
        /// .typeref 'Type' at 'ResolutionScope'
        /// </summary>
        public string ResolutionScope { get; set; }

        /// <summary>
        /// .typeref 'Type' [any] at 'ResolutionScope' <br />
        /// .typeref 'Type' constraint [any] deny <br />
        /// .typeref 'Type' [any] assert
        /// </summary>
        public bool Any { get; set; }

        /// <summary>
        /// .typeref 'Type' constraint deny
        /// </summary>
        public bool Deny { get; set; }

        /// <summary>
        /// .typeref 'Type' assert
        /// </summary>
        public bool Assert { get; set; }

        public override string Serialize() => Pack(Name, ResolutionScope, Any, Deny, Assert);

        public override AsmDirectiveAbstract Deserialize(string serialized)
        {
            object[] v = Unpack(serialized);

            Name = ToS(v[0]);
            ResolutionScope = ToS(v[1]);
            Any = ToB(v[2]);
            Deny = ToB(v[3]);
            Assert = ToB(v[4]);

            return ReturnObjects(v, max: 5);
        }

        public override string ToString()
        {
            StringBuilder sb = new(80);
            sb.Append($".typeref '{Name}' ");

            if(Assert)
            {
                if(Any) sb.Append("any ");
                sb.Append("assert");
                return sb.ToString();
            }

            if(Deny)
            {
                sb.Append("constraint ");
                if(Any) sb.Append("any ");
                sb.Append("deny");
                return sb.ToString();
            }

            if(Any) sb.Append("any ");
            sb.Append($"at '{ResolutionScope}'");
            return sb.ToString();
        }

        public override IEnumerable<string> Format()
        {
            StringBuilder sb = new(80);
            sb.Append($"  .typeref '{Name}' ");

            if(Assert)
            {
                if(Any) sb.Append("any ");
                sb.Append("assert");
                yield return sb.ToString();
            }
            else if(Deny)
            {
                sb.Append("constraint ");
                if(Any) sb.Append("any ");
                sb.Append("deny");
                yield return sb.ToString();
            }
            else
            {
                if(Any) sb.Append("any ");
                sb.Append($"at '{ResolutionScope}'");
                yield return sb.ToString();
            }
        }
    }
}
