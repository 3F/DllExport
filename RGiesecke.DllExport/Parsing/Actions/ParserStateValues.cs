// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.3.29766, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RGiesecke.DllExport.Parsing.Actions
{
    public sealed class ParserStateValues
    {
        public readonly Stack<string> ClassNames = new Stack<string>();
        public readonly ParserStateValues.MethodStateValues Method = new ParserStateValues.MethodStateValues();
        private readonly List<string> _Result = new List<string>();
        private readonly CpuPlatform _Cpu;
        private readonly ReadOnlyCollection<string> _InputLines;
        public bool AddLine;
        public string ClassDeclaration;
        public int MethodPos;
        public ParserState State;

        public IList<string> InputLines
        {
            get {
                return (IList<string>)this._InputLines;
            }
        }

        public int InputPosition
        {
            get;
            internal set;
        }

        public CpuPlatform Cpu
        {
            get {
                return this._Cpu;
            }
        }

        public List<string> Result
        {
            get {
                return this._Result;
            }
        }

        public ParserStateValues(CpuPlatform cpu, IList<string> inputLines)
        {
            this._Cpu = cpu;
            this._InputLines = new ReadOnlyCollection<string>(inputLines);
        }

        public SourceCodeRange GetRange()
        {
            for(int inputPosition = this.InputPosition; inputPosition < this.InputLines.Count; ++inputPosition)
            {
                string str = this.InputLines[inputPosition];
                string line;
                if(str != null && (line = str.Trim()).StartsWith(".line", StringComparison.Ordinal))
                {
                    return SourceCodeRange.FromMsIlLine(line);
                }
            }
            return (SourceCodeRange)null;
        }

        public sealed class MethodStateValues
        {
            public string Declaration
            {
                get;
                set;
            }

            public string ResultAttributes
            {
                get;
                set;
            }

            public string Name
            {
                get;
                set;
            }

            public string Attributes
            {
                get;
                set;
            }

            public string Result
            {
                get;
                set;
            }

            public string After
            {
                get;
                set;
            }

            public MethodStateValues()
            {
                this.Reset();
            }

            public override string ToString()
            {
                return this.Name.IfEmpty("no name...") + "; " + this.Declaration.IfEmpty("no declaration...");
            }

            public void Reset()
            {
                this.Declaration = "";
                this.Name = "";
                this.Attributes = "";
                this.Result = "";
                this.ResultAttributes = "";
                this.After = "";
            }
        }
    }
}
