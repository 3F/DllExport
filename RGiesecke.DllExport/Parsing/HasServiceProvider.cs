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
