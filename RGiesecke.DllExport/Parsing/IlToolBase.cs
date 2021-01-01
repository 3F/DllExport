//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2021  Denis Kuzmin <x-3F@outlook.com> github/3F
//$ Distributed under the MIT License (MIT)

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
