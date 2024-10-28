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
        [Fact]
        public void KeyListTest1()
        {
            AssertArgs($"-list -pemodule \"{Assets.PrjNetfxRuntime}\"", pe => pe.Export.Names);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void KeyListAddrTest1(bool hex)
        {
            AssertArgs
            (
                $"-list-addr {(hex ? "-hex" : "")} -pemodule \"{Assets.PrjNetfxRuntime}\"",
                pe => pe.Export.Addresses,
                hex
            );
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void KeyListOrdTest1(bool hex)
        {
            AssertArgs
            (
                $"{(hex ? "-hex" : "")} -list-ord -pemodule \"{Assets.PrjNetfxRuntime}\"",
                pe => pe.Export.NameOrdinals,
                hex
            );
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void KeyMagicTest1(bool hex)
        {
            AssertArgs
            (
                $"-magic -pemodule \"{Assets.PrjNetfxRuntime}\" {(hex ? "-hex" : "")}",
                pe => new ushort[] { (ushort)pe.Magic },
                hex
            );
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
            Assert.Contains("Unsupported keys:", stderr);
        }

        [Fact]
        public void FailedKeysTest2()
        {
            string pemodule = "notreal.dll";
            Tools.RunPeViewer($"-pemodule {pemodule} -list", out string stderr);
            Assert.Contains($"Module '{pemodule}' is not found.", stderr);
        }

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