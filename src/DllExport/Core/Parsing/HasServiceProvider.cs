/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;

namespace net.r_eg.DllExport.Parsing
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

        private bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool _)
        {
            if(!disposed)
            {
                if(_ServiceProvider is not IDisposable disposable) return;
                disposable.Dispose();

                disposed = true;
            }
        }

        #endregion
    }
}
