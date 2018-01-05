//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2018  Denis Kuzmin < entry.reg@gmail.com > :: github.com/3F
//$ Distributed under the MIT License (MIT)

using System;

namespace RGiesecke.DllExport
{
    public class GenericDisposable: IDisposable
    {
        private readonly Action _Action;

        public GenericDisposable(Action action)
        {
            if(action == null)
            {
                throw new ArgumentNullException("action");
            }
            this._Action = action;
        }

        #region IDisposable

        // To detect redundant calls
        private bool disposed = false;

        // To correctly implement the disposable pattern. /CA1063
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposed) {
                return;
            }
            disposed = true;

            //...
            _Action();
        }

        #endregion
    }
}
