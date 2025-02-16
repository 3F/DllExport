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
using net.r_eg.DllExport.Extensions;

namespace net.r_eg.DllExport.Parsing.Actions
{
    public sealed class ParserStateValues
    {
        private readonly List<string> _Result = [];
        private readonly List<ExternalAssemlyDeclaration> _ExternalAssemlyDeclarations = [];
        private readonly IList<ExternalAssemlyDeclaration> _ReadonlyExternalAssemlyDeclarations;
        private readonly CpuPlatform _Cpu;
        private readonly ReadOnlyCollection<string> _InputLines;

        public readonly Stack<string> ClassNames = new();
        public readonly MethodStateValues Method = new();
        
        public bool AddLine;
        public string ClassDeclaration;
        public int MethodPos;
        public ParserState State;

        public IList<string> InputLines => _InputLines;

        public int InputPosition { get; internal set; }

        public CpuPlatform Cpu => _Cpu;

        public List<string> Result => _Result;

        public IList<ExternalAssemlyDeclaration> ExternalAssemlyDeclarations => _ReadonlyExternalAssemlyDeclarations;

        public ParserStateValues(CpuPlatform cpu, IList<string> inputLines)
        {
            _Cpu = cpu;
            _InputLines = new ReadOnlyCollection<string>(inputLines);
            _ReadonlyExternalAssemlyDeclarations = _ExternalAssemlyDeclarations.AsReadOnly();
        }

        public SourceCodeRange GetRange()
        {
            for(int inputPosition = InputPosition; inputPosition < InputLines.Count; ++inputPosition)
            {
                string str = InputLines[inputPosition];
                string line;
                if(str != null && (line = str.Trim()).StartsWith(".line", StringComparison.Ordinal))
                {
                    return SourceCodeRange.FromMsIlLine(line);
                }
            }
            return null;
        }

        public ExternalAssemlyDeclaration RegisterExternalAssemlyAlias(string assemblyName, string alias)
        {
            ExternalAssemlyDeclaration assemlyDeclaration = new(Result.Count, assemblyName, alias);
            _ExternalAssemlyDeclarations.Add(assemlyDeclaration);
            return assemlyDeclaration;
        }

        public sealed class MethodStateValues
        {
            public string Declaration { get; set; }

            public string ResultAttributes { get; set; }

            public string Name { get; set; }

            public string Attributes { get; set; }

            /// <summary>
            /// May contain a related signature ~ pinvokeimpl(".dll" winapi) or null value.
            /// </summary>
            public string Pinvokeimpl { get; set; }

            public string Result { get; set; }

            public string After { get; set; }

            public MethodStateValues() => Reset();

            public override string ToString() => string.Format
            (
                "{0}; {1}",
                Name.NullIfEmpty() ?? Resources.no_name___,
                Declaration.NullIfEmpty() ?? Resources.no_declaration___
            );

            public void Reset()
            {
                Declaration         = string.Empty;
                Name                = string.Empty;
                Attributes          = string.Empty;
                Result              = string.Empty;
                ResultAttributes    = string.Empty;
                After               = string.Empty;

                Pinvokeimpl = null;
            }
        }
    }
}
