/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

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

            /// <summary>
            /// May contain related signature ~ pinvokeimpl(".dll" winapi) or null value.
            /// </summary>
            public string Pinvokeimpl
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
                Reset();
            }

            public override string ToString()
            {
                return String.Format(
                    "{0}; {1}",
                    Name.IfEmpty(Resources.no_name___),
                    Declaration.IfEmpty(Resources.no_declaration___)
                );
            }

            public void Reset()
            {
                Declaration         = String.Empty;
                Name                = String.Empty;
                Attributes          = String.Empty;
                Result              = String.Empty;
                ResultAttributes    = String.Empty;
                After               = String.Empty;

                Pinvokeimpl = null;
            }
        }
    }
}
