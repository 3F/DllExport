/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.IO;
using net.r_eg.Conari;
using net.r_eg.DllExport.UnitedTest._svc;
using Xunit;
using Xunit.Abstractions;

namespace net.r_eg.DllExport.UnitedTest
{
    public class Net5OrLessAssetBasicTest(ITestOutputHelper output)
    {
        private readonly ITestOutputHelper output = output;

        [Theory]
        [InlineData("net5.0")]
        [InlineData("netcoreapp3.1")]
        [InlineData("netstandard2.1")]
        [InlineData("netstandard2.0")]
        public void TouchTest1(string tfm)
        {
            string dll = Assets.GetNet5OrLessDll(tfm);
            if(File.Exists(dll)) // some TFMs can be excluded from the build
            {
                using dynamic l = new ConariX(dll);
                Assert.Equal(Math.PI, l.touch<double>());
            }
            else output.WriteLine($"{nameof(TouchTest1)}. Ignored: {tfm}");
        }
    }
}
