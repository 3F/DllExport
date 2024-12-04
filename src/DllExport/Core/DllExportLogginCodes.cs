/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

namespace net.r_eg.DllExport
{
    public static class DllExportLogginCodes
    {
        public static readonly string MethodFound = "EXP0001";
        public static readonly string OldDeclaration = "EXP0002";
        public static readonly string NewDeclaration = "EXP0003";
        public static readonly string AddingVtEntry = "EXP0004";
        public static readonly string IlAsmLogging = "EXP0005";
        public static readonly string VerboseToolLogging = "EXP0006";
        public static readonly string IlDasmLogging = "EXP0007";
        public static readonly string RemovingDllExportAttribute = "EXP0008";
        public static readonly string CreatingBinariesForEachPlatform = "EXP0009";
        public static readonly string AmbigiguousExportName = "EXP0010";
        public static readonly string ExportNamesHaveToBeBasicLatin = "EXP0011";
        public static readonly string RemovingReferenceToDllExportAttributeAssembly = "EXP0012";
        public static readonly string NoParserActionError = "EXP0013";
        public static readonly string LibToolLooging = "EXP0014";
        public static readonly string LibToolVerboseLooging = "EXP0015";
        public static readonly string MethodIsNotStatic = "EXP0016";
        public static readonly string ExportInGenericType = "EXP0017";
        public static readonly string ExportOnGenericMethod = "EXP0018";
        public static readonly string PeCheck1to1 = "EXP0019";
        public static readonly string PeCheckIl = "EXP0020";
        public static readonly string PeCheck32orPlus = "EXP0021";
    }
}
