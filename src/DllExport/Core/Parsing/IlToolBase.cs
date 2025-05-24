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
    public abstract class IlToolBase(IServiceProvider provider, IInputValues inputValues)
        : HasServiceProvider(provider)
    {
        protected IDllExportNotifier Notifier
            => (IDllExportNotifier)ServiceProvider.GetService(typeof(IDllExportNotifier));

        public int Timeout { get; set; }

        public IInputValues InputValues { get; private set; }
            = inputValues ?? throw new ArgumentNullException(nameof(inputValues));

        public string TempDirectory { get; set; }
    }
}
