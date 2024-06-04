/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

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
