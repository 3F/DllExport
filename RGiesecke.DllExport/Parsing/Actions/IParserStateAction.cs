//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
//$ Distributed under the MIT License (MIT)

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
