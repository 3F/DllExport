/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;

namespace net.r_eg.DllExport
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
