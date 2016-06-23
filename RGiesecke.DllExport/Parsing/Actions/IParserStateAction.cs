// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.7.38850, Culture=neutral, PublicKeyToken=8f52d83c1a22df51
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

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
