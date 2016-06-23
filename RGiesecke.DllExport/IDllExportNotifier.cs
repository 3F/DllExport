// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.3.29766, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
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
    }
}
