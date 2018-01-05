//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2018  Denis Kuzmin < entry.reg@gmail.com > :: github.com/3F
//$ Distributed under the MIT License (MIT)

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
