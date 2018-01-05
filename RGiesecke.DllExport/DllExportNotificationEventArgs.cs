//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2018  Denis Kuzmin < entry.reg@gmail.com > :: github.com/3F
//$ Distributed under the MIT License (MIT)

using System;

namespace RGiesecke.DllExport
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
