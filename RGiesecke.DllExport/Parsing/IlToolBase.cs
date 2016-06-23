// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.7.38850, Culture=neutral, PublicKeyToken=8f52d83c1a22df51
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
