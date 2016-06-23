// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.2.23706, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
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
