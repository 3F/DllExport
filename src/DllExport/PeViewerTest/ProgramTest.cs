/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections;
using net.r_eg.Conari.PE;
using net.r_eg.DllExport.UnitedTest._svc;
using Xunit;

namespace net.r_eg.DllExport.PeViewerTest
{
    public class ProgramTest
    {
        [Theory]
        [InlineData("-pemodule")]
        [InlineData("-i")]
        public void KeyListTest1(string keyFile)
        {
            AssertArgs($"-list {keyFile} \"{Assets.PrjNetfxRuntime}\"", pe => pe.Export.Names);
            AssertEmpty($"-list {keyFile} \"{Assets.PkgPeViewer}\"");
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void KeyListAddrTest1(bool hex)
        {
            AssertArgs
            (
                $"-list-addr {GetHex(hex)} -pemodule \"{Assets.PrjNetfxRuntime}\"",
                pe => pe.Export.Addresses,
                hex
            );
            AssertEmpty($"-list-addr {GetHex(hex)} -pemodule \"{Assets.PkgPeViewer}\"");
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void KeyListOrdTest1(bool hex)
        {
            AssertArgs
            (
                $"{GetHex(hex)} -list-ord -pemodule \"{Assets.PrjNetfxRuntime}\"",
                pe => pe.Export.NameOrdinals,
                hex
            );
            AssertEmpty($"{GetHex(hex)} -list-ord -pemodule \"{Assets.PkgPeViewer}\"");
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void KeyMagicTest1(bool hex)
        {
            AssertArgs
            (
                $"-magic -pemodule \"{Assets.PrjNetfxRuntime}\" {GetHex(hex)}",
                pe => new ushort[] { (ushort)pe.Magic },
                hex
            );
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void KeyNumFunctionsTest1(bool hex)
        {
            AssertArgs
            (
                $"-num-functions -i \"{Assets.PrjNetfxRuntime}\" {GetHex(hex)}",
                pe => new[] { pe.DExport.NumberOfFunctions },
                hex
            );

            AssertArgs
            (
                $"-num-functions {GetHex(hex)} -i \"{Assets.PkgPeViewer}\"",
                pe => new[] { 0 },
                hex
            );
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void KeyBaseTest1(bool hex)
        {
            AssertArgs
            (
                $"-base -i \"{Assets.PrjNetfxRuntime}\" {GetHex(hex)}",
                pe => new[] { pe.DExport.Base },
                hex
            );
            AssertEmpty($"-base -i \"{Assets.PkgPeViewer}\"");
        }

        [Fact]
        public void KeyVersionTest1()
        {
            string cmdVersion = Tools.RunPeViewer("-version");
            Assert.Equal(cmdVersion, Tools.RunPeViewer(string.Empty));

            Assert.StartsWith(DllExportVersion.S_NUM, cmdVersion);

#if DEBUG
            Assert.Contains(" Debug", cmdVersion);
#endif
        }

        [Fact]
        public void KeyHelpTest1()
        {
            string cmdHelp = Tools.RunPeViewer("-help");
            Assert.Equal(cmdHelp, Tools.RunPeViewer("-h"));

            foreach(string key in PeViewer.Program.Switches.Keys)
            {
                Assert.Contains(key, cmdHelp);
            }
        }

        [Fact]
        public void FailedKeysTest1()
        {
            Tools.RunPeViewer("-pemodule -not-real", out string stderr);
            Assert.Contains("Invalid or Incomplete command: -pemodule", stderr);

            Tools.RunPeViewer("-pemodule \"\" -not-real", out stderr);
            Assert.Contains("?-> -not-real", stderr);
        }

        [Fact]
        public void FailedKeysTest2()
        {
            string pemodule = "notreal.dll";
            Tools.RunPeViewer($"-pemodule {pemodule} -list", out string stderr);
            Assert.Contains($"Module '{pemodule}' is not found.", stderr);
        }

        private static string GetHex(bool hex) => hex ? "-hex" : string.Empty;

        private static void AssertEmpty(string args) => Assert.Equal(string.Empty, Tools.RunPeViewer(args));

        private void AssertArgs(string args, Func<IPE, IEnumerable> list, bool hex = false)
        {
            using PEFile pe = new(Assets.PrjNetfxRuntime);
            Assert.Empty
            (
                RemoveFrom
                (
                    Tools.RunPeViewer(args),
                    list(pe),
                    hex
                )
            );
        }

        private string RemoveFrom(string input, IEnumerable list, bool hex = false)
        {
            foreach(object data in list)
            {
                input = input.Replace((hex ? $"0x{data:x8}" : data.ToString()) + Environment.NewLine, string.Empty);
            }
            return input;
        }
    }
}