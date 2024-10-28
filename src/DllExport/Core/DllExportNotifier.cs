/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections.Generic;
using System.Globalization;

namespace net.r_eg.DllExport
{
    public class DllExportNotifier: IDllExportNotifier, IDisposable
    {
        private readonly Stack<NotificationContext> _ContextScopes = new Stack<NotificationContext>();

        public string ContextName
        {
            get {
                NotificationContext context = this.Context;
                if(!(context != (NotificationContext)null))
                {
                    return (string)null;
                }
                return context.Name;
            }
        }

        public object ContextObject
        {
            get {
                NotificationContext context = this.Context;
                if(!(context != (NotificationContext)null))
                {
                    return (object)null;
                }
                return context.Context;
            }
        }

        public NotificationContext Context
        {
            get {
                return _ContextScopes.Peek();
            }
        }

        public event EventHandler<DllExportNotificationEventArgs> Notification;

        public IDisposable CreateContextName(object context, string name)
        {
            return (IDisposable)new DllExportNotifier.ContextScope(this, new NotificationContext(name, context));
        }

        public void Notify(DllExportNotificationEventArgs e)
        {
            this.OnNotification((object)(this.Context ?? new NotificationContext((string)null, (object)this)), e);
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
            Notify(severity, code, fileName: null, startPosition: new(), endPosition: new(), message, values);
        }

        public void Notify(int severity, string code, string fileName, SourceCodePosition? startPosition, SourceCodePosition? endPosition, string message, params object[] values)
        {
            string msg = values?.Length > 0 ? string.Format(CultureInfo.InvariantCulture, message, values) : message;
            if(string.IsNullOrEmpty(msg)) return;

            Notify(new()
            {
                Severity = severity,
                Code = code,
                Context = Context,
                FileName = fileName,
                StartPosition = startPosition,
                EndPosition = endPosition,
                Message = msg
            });
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
                Notification = null;
                disposed = true;
            }
        }

        #endregion

        private sealed class ContextScope: IDisposable
        {
            private readonly DllExportNotifier _Notifier;

            public NotificationContext Context
            {
                get;
                private set;
            }

            public ContextScope(DllExportNotifier notifier, NotificationContext context)
            {
                this.Context = context;
                this._Notifier = notifier;
                Stack<NotificationContext> notificationContextStack = this._Notifier._ContextScopes;
                lock(notificationContextStack)
                    notificationContextStack.Push(context);
            }

            public void Dispose()
            {
                Stack<NotificationContext> notificationContextStack = this._Notifier._ContextScopes;
                lock(notificationContextStack)
                {
                    if(notificationContextStack.Peek() != this.Context)
                    {
                        throw new InvalidOperationException(string.Format(Resources.Current_Notifier_Context_is___0____it_should_have_been___1___, (object)notificationContextStack.Peek(), (object)this.Context.Name));
                    }
                    notificationContextStack.Pop();
                }
            }
        }
    }
}
