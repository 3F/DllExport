// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.4.23262, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;

namespace RGiesecke.DllExport.Parsing
{
    public abstract class IlToolBase: DllExportNotifierWrapper
    {
        protected override IDllExportNotifier Notifier
        {
            get {
                return this.InputValues.Notifier;
            }
        }

        protected override bool OwnsNotifier
        {
            get {
                return false;
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

        protected IlToolBase(IInputValues inputValues)
        : base((IDllExportNotifier)null)
        {
            if(inputValues == null)
            {
                throw new ArgumentNullException("inputValues");
            }
            this.InputValues = inputValues;
        }
    }
}
