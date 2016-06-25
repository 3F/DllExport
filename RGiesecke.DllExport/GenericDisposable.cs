// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.7.38850, Culture=neutral, PublicKeyToken=8f52d83c1a22df51
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

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
