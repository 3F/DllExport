﻿/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

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
