// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.7.38850, Culture=neutral, PublicKeyToken=8f52d83c1a22df51
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;

namespace RGiesecke.DllExport.Parsing
{
    public abstract class HasServiceProvider: IDisposable, IServiceProvider
    {
        private readonly IServiceProvider _ServiceProvider;

        public IServiceProvider ServiceProvider
        {
            get {
                return _ServiceProvider;
            }
        }

        public object GetService(Type serviceType)
        {
            return _ServiceProvider.GetService(serviceType);
        }

        protected HasServiceProvider(IServiceProvider serviceProvider)
        {
            _ServiceProvider = serviceProvider;
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
            IDisposable disposable = _ServiceProvider as IDisposable;
            if(disposable == null) {
                return;
            }
            disposable.Dispose();
        }

        #endregion
    }
}
