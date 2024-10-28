/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Linq;
using net.r_eg.Conari;
using net.r_eg.Conari.PE;
using net.r_eg.Conari.Types;
using net.r_eg.DllExport.UnitedTest._svc;
using Xunit;

namespace net.r_eg.DllExport.UnitedTest
{
    public class NetfxAssetBasicTest
    {
        [Fact]
        public void AddTest1()
        {
            using dynamic l = new ConariX(Assets.PrjNetfxRuntime);
            int x = l.add<int>(5, 7);

            Assert.Equal(12, x);

            IPE pe = ((IConari)l).PE;
            Assert.Equal(pe.Magic, Assets.MagicAtThis);

            Assert.NotEmpty(pe.ExportedProcNames);
            Assert.NotNull(pe.ExportedProcNames.FirstOrDefault(s => s == "add"));
        }

        [Fact]
        public void CallmeTest1()
        {
            TCharPtr.Unicode = true;
            using NativeString<TCharPtr> ns = new("Hello world!");
            using NativeStruct<Arg> nstruct = new(new Arg() { x = 7, y = 5 });

            using dynamic l = new ConariX(Assets.PrjNetfxRuntime);

            try
            {
                IntPtr ptr = l.callme<IntPtr>(ns, nstruct);

                if(ptr != IntPtr.Zero)
                {
                    using NativeArray<int> nr = new(4, ptr);

                    Assert.Equal(-1, nr[0]);
                    Assert.Equal(7, nr[1]);
                    Assert.Equal(1, nr[2]);
                    Assert.Equal(5, nr[3]);

                    nstruct.read(out Arg upd);
                    Assert.Equal(8, upd.x); // due to structure.Access().write<int>(8);

                    // possible alternatives ~
                    using NativeStruct<Arg> nstruct2 = new((IntPtr)nstruct);
                    Assert.Equal(upd.x, nstruct2.Data.x);
                    Assert.Equal(upd.x, nstruct.Native.f<int>("x")._.x);

                    using NativeArray<int> nr2 = new(2, nstruct2);
                    Assert.Equal(upd.x, nr2[0]);
                }

                TCharPtr r = l.callme<TCharPtr>(ns + " and You 👋", nstruct);
                Assert.Equal("Рад знакомству 🤝 どうぞよろしく", r);
            }
            finally
            {
                l.free();
            }
        }

        [Fact]
        public void CallmeTest2()
        {
            using dynamic l = new ConariX(Assets.PrjNetfxRuntime);

            using NativeString<CharPtr> ns = new("Hello world");

            try
            {
                IntPtr ptr = l.callme<IntPtr>(ns);
                Assert.Equal(IntPtr.Zero, ptr);
            }
            finally
            {
                l.free();
            }
        }

        [Fact]
        public void ThrowExceptionTest1()
        {
            using ConariL l = new(Assets.PrjNetfxRuntime);

            Assert.Equal
            (
                "こんにちは！",
                Assert.Throws<NotImplementedException>(() => { l.bind<Action>("throwException")(); }).Message
            );
        }

        struct Arg { public int x, y; }
    }
}
