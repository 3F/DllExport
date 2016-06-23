// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.6.36226, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;

namespace RGiesecke.DllExport.Parsing
{
    public abstract class IlToolBase: HasServiceProvider
    {
        protected IDllExportNotifier Notifier
        {
            get {
                return this.ServiceProvider.GetService<IDllExportNotifier>();
            }
        }

        public int Timeout
        {
            get;
            set;
        }

        public IInputValues InputValues
        {
            get;
            private set;
        }

        public string TempDirectory
        {
            get;
            set;
        }

        protected IlToolBase(IServiceProvider serviceProvider, IInputValues inputValues)
        : base(serviceProvider)
        {
            if(inputValues == null)
            {
                throw new ArgumentNullException("inputValues");
            }
            this.InputValues = inputValues;
        }
    }
}
