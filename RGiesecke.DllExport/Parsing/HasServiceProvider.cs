// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.6.36226, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
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
                return this._ServiceProvider;
            }
        }

        protected HasServiceProvider(IServiceProvider serviceProvider)
        {
            this._ServiceProvider = serviceProvider;
        }

        public void Dispose()
        {
            IDisposable disposable = this._ServiceProvider as IDisposable;
            if(disposable == null)
            {
                return;
            }
            disposable.Dispose();
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            return this._ServiceProvider.GetService(serviceType);
        }
    }
}
