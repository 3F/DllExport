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
                if(!OwnsNotifier) return;

                if(Notifier is not IDisposable disposable) return;
                disposable.Dispose();

                disposed = true;
            }
        }

        #endregion
    }
}
