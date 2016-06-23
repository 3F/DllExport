// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.6.36226, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;

namespace RGiesecke.DllExport.Parsing.Actions
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal sealed class ParserStateActionAttribute: Attribute
    {
        public readonly ParserState ParserState;

        public ParserStateActionAttribute(ParserState parserState)
        {
            this.ParserState = parserState;
        }
    }
}
