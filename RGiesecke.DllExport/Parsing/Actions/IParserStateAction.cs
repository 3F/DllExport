/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

namespace RGiesecke.DllExport.Parsing.Actions
{
    public interface IParserStateAction
    {
        long Milliseconds
        {
            get;
            set;
        }

        IlParser Parser
        {
            get;
            set;
        }

        void Execute(ParserStateValues state, string trimmedLine);
    }
}
