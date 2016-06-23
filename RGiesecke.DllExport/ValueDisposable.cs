// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.2.23706, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;

namespace RGiesecke.DllExport
{
    public class ValueDisposable<T>: GenericDisposable
    {
        public T Value
        {
            get;
            private set;
        }

        public ValueDisposable(T value, Action<T> action)
        : base((Action)(() => action(value)))
        {
            this.Value = value;
        }
    }
}
