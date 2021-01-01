//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2021  Denis Kuzmin <x-3F@outlook.com> github/3F
//$ Distributed under the MIT License (MIT)

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
