// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.7.38850, Culture=neutral, PublicKeyToken=8f52d83c1a22df51
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;

namespace RGiesecke.DllExport
{
    public interface IDllExportNotifier
    {
        event EventHandler<DllExportNotificationEventArgs> Notification;

        void Notify(int severity, string code, string message, params object[] values);

        void Notify(int severity, string code, string fileName, SourceCodePosition? startPosition, SourceCodePosition? endPosition, string message, params object[] values);

        void Notify(DllExportNotificationEventArgs e);

        IDisposable CreateContextName(object context, string name);
    }
}
