// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.1.28776, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;
using System.Globalization;

namespace RGiesecke.DllExport
{
    public sealed class DllExportNotifier: IDllExportNotifier, IDisposable
    {
        public object Context
        {
            get;
            set;
        }

        public event EventHandler<DllExportNotificationEventArgs> Notification;

        public void Dispose()
        {
            this.Notification = (EventHandler<DllExportNotificationEventArgs>)null;
        }

        public void Notify(DllExportNotificationEventArgs e)
        {
            this.OnNotification(this.Context ?? (object)this, e);
        }

        private void OnNotification(object sender, DllExportNotificationEventArgs e)
        {
            EventHandler<DllExportNotificationEventArgs> eventHandler = this.Notification;
            if(eventHandler == null)
            {
                return;
            }
            eventHandler(sender, e);
        }

        public void Notify(int severity, string code, string message, params object[] values)
        {
            this.Notify(severity, code, (string)null, new SourceCodePosition?(), new SourceCodePosition?(), message, values);
        }

        public void Notify(int severity, string code, string fileName, SourceCodePosition? startPosition, SourceCodePosition? endPosition, string message, params object[] values)
        {
            DllExportNotificationEventArgs e = new DllExportNotificationEventArgs() {
                Severity = severity,
                Code = code,
                Context = this.Context,
                FileName = fileName,
                StartPosition = startPosition,
                EndPosition = endPosition
            };
            e.Message = values.NullSafeCall<object[], int>((Func<int>)(() => values.Length)) == 0 ? message : string.Format((IFormatProvider)CultureInfo.InvariantCulture, message, values);
            if(string.IsNullOrEmpty(e.Message))
            {
                return;
            }
            this.Notify(e);
        }
    }
}
