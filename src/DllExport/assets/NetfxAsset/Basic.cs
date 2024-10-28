/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections.Concurrent;
using net.r_eg.Conari.Native;
using net.r_eg.Conari.Types;

namespace NetfxAsset
{
    public static class Basic
    {
        private static readonly ConcurrentStack<IDisposable> resources = new();

        [DllExport]
        public static int add(int a, int b) => a + b;

        [DllExport]
        public static void throwException() => throw new NotImplementedException("こんにちは！");

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
    }
}
