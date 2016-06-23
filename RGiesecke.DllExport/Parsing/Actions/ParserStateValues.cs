// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.6.36226, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RGiesecke.DllExport.Properties;

namespace RGiesecke.DllExport.Parsing.Actions
{
    public sealed class ParserStateValues
    {
        public readonly Stack<string> ClassNames = new Stack<string>();
        public readonly ParserStateValues.MethodStateValues Method = new ParserStateValues.MethodStateValues();
        private readonly List<string> _Result = new List<string>();
        private readonly List<ExternalAssemlyDeclaration> _ExternalAssemlyDeclarations = new List<ExternalAssemlyDeclaration>();
        private readonly CpuPlatform _Cpu;
        private readonly ReadOnlyCollection<string> _InputLines;
        public bool AddLine;
        public string ClassDeclaration;
        public int MethodPos;
        public ParserState State;
        private readonly IList<ExternalAssemlyDeclaration> _ReadonlyExternalAssemlyDeclarations;

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

        public IList<ExternalAssemlyDeclaration> ExternalAssemlyDeclarations
        {
            get {
                return this._ReadonlyExternalAssemlyDeclarations;
            }
        }

        public ParserStateValues(CpuPlatform cpu, IList<string> inputLines)
        {
            this._Cpu = cpu;
            this._InputLines = new ReadOnlyCollection<string>(inputLines);
            this._ReadonlyExternalAssemlyDeclarations = (IList<ExternalAssemlyDeclaration>)this._ExternalAssemlyDeclarations.AsReadOnly();
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

        public ExternalAssemlyDeclaration RegisterMsCorelibAlias(string assemblyName, string alias)
        {
            ExternalAssemlyDeclaration assemlyDeclaration = new ExternalAssemlyDeclaration(this.Result.Count, assemblyName, alias);
            this._ExternalAssemlyDeclarations.Add(assemlyDeclaration);
            return assemlyDeclaration;
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
                return this.Name.IfEmpty(Resources.no_name___) + "; " + this.Declaration.IfEmpty(Resources.no_declaration___);
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
