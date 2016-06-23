// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.7.38850, Culture=neutral, PublicKeyToken=8f52d83c1a22df51
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
