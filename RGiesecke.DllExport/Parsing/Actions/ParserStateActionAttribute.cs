//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2019  Denis Kuzmin < entry.reg@gmail.com > GitHub/3F
//$ Distributed under the MIT License (MIT)

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
