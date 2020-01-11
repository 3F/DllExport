//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
//$ Distributed under the MIT License (MIT)

using System;

namespace RGiesecke.DllExport.Parsing
{
    public abstract class DllExportNotifierWrapper: IDllExportNotifier, IDisposable
    {
        public event EventHandler<DllExportNotificationEventArgs> Notification
        {
            add {
                Notifier.Notification += value;
            }
            remove {
                Notifier.Notification -= value;
            }
        }

        protected virtual IDllExportNotifier Notifier
        {
            get;
            private set;
        }

        protected virtual bool OwnsNotifier
        {
            get {
                return false;
            }
        }

        public IDisposable CreateContextName(object context, string name)
        {
            return this.Notifier.CreateContextName(context, name);
        }

        public void Notify(DllExportNotificationEventArgs e)
        {
            this.Notifier.Notify(e);
        }

        public void Notify(int severity, string code, string message, params object[] values)
        {
            this.Notifier.Notify(severity, code, message, values);
        }

        public void Notify(int severity, string code, string fileName, SourceCodePosition? startPosition, SourceCodePosition? endPosition, string message, params object[] values)
        {
            this.Notifier.Notify(severity, code, fileName, startPosition, endPosition, message, values);
        }

        protected DllExportNotifierWrapper(IDllExportNotifier notifier)
        {
            Notifier = notifier;
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

            if(!OwnsNotifier) {
                return;
            }

            IDisposable disposable = Notifier as IDisposable;
            if(disposable == null) {
                return;
            }
            disposable.Dispose();
        }

        #endregion
    }
}
