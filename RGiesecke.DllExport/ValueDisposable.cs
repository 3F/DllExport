//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2019  Denis Kuzmin < entry.reg@gmail.com > GitHub/3F
//$ Distributed under the MIT License (MIT)

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
