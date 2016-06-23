// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.7.38850, Culture=neutral, PublicKeyToken=8f52d83c1a22df51
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
