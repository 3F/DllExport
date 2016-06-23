// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.7.38850, Culture=neutral, PublicKeyToken=8f52d83c1a22df51
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;

namespace RGiesecke.DllExport.Parsing
{
    public abstract class DllExportNotifierWrapper: IDllExportNotifier, IDisposable
    {
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

        event EventHandler<DllExportNotificationEventArgs> IDllExportNotifier.Notification
        {
            add {
                this.Notifier.Notification += value;
            }
            remove {
                this.Notifier.Notification -= value;
            }
        }

        protected DllExportNotifierWrapper(IDllExportNotifier notifier)
        {
            this.Notifier = notifier;
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

        public void Dispose()
        {
            if(!this.OwnsNotifier)
            {
                return;
            }
            IDisposable disposable = this.Notifier as IDisposable;
            if(disposable == null)
            {
                return;
            }
            disposable.Dispose();
        }
    }
}
