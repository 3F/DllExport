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
    public class DllExportNotificationEventArgs: EventArgs
    {
        public NotificationContext Context
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public int Severity
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        }

        public string FileName
        {
            get;
            set;
        }

        public SourceCodePosition? StartPosition
        {
            get;
            set;
        }

        public SourceCodePosition? EndPosition
        {
            get;
            set;
        }
    }
}
