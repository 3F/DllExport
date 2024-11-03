/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using net.r_eg.Conari.Native;
using net.r_eg.Conari.Types;

namespace NetfxAsset
{
    using static net.r_eg.Conari.Static.Members;

    public static class Basic
    {
        private static readonly ConcurrentStack<IDisposable> resources = new();

        [DllExport]
        public static int add(int a, int b) => a + b;

        [DllExport]
        public static void throwException() => throw new NotImplementedException("こんにちは！");

        [DllExport]
        public static void passBuffered(CharPtr cstr)
        {
            new NativeString<CharPtr>(cstr.AddressPtr).update("new value");
        }

        [DllExport]
        public static IntPtr callme(TCharPtr str, IntPtr structure)
        {
            if(TCharPtr.Unicode && $"{str}".EndsWith(" and You 👋"))
            {
                return R(new NativeString<TCharPtr>("Рад знакомству 🤝 どうぞよろしく"));
            }

            if(str != "Hello world!") return IntPtr.Zero;

            structure.Native().f<int>("x", "y").build(out dynamic v);
            if(v.x > v.y)
            {
                structure.Access().write<int>(8);
            }
            return R(new NativeArray<int>(-1, v.x, 1, v.y));
        }

        [DllExport]
        public static IntPtr getStructExarg() => R
        (
            new NativeStruct<Exarg>(new Exarg
            (
                0x3210,
                R(new NativeString<CharPtr>(".NET DllExport + Conari"))
            ))
        );

        [DllExport]
        public static IntPtr getUnalignedStruct() => NativeStruct.Make
            .f<int>("x") // NOTE this field is not aligned (x64) as in getStructExarg() ^v
            .f<CharPtr>("str")
            .Struct.AddressPtr.Access()
                .write(4096)
                .write(R(new NativeString<CharPtr>("unaligned struct via Conari")))
                .InitialPtr;

        [DllExport]
        public static bool pass(int a, CharPtr cstr, [MarshalAs(UnmanagedType.LPWStr)] string str, Exarg data)
        {
            bool lef = (a == 0x3F_0000) && (str == "hello") && (cstr == "test123");
            return !Is64bit ? lef : lef && (data.x == a) && ((CharPtr)data.str == cstr);
        }

        [DllExport]
        public static void free()
        {
            while(resources.Count > 0)
            {
                resources.TryPop(out IDisposable r);
                r?.Dispose();
            }
        }

        private static IntPtr R<T>(T input) where T: IDisposable, IMarshalableGeneric 
        {
            resources.Push(input);
            return input.AddressPtr;
        }

        #region optional structures for the convenience

        public struct Exarg(int x, IntPtr str)
        {
            public int x = x;
            public IntPtr str = str;
        }

        #endregion
    }
}
