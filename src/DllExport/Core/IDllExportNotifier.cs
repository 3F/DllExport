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

        /// <inheritdoc cref="Notify(int, string, string, SourceCodePosition?, SourceCodePosition?, string, object[])" />
        void Notify(int severity, string code, string message, params object[] values);

        #region temp migration. TODO: enum /F-272

        void NotifyError(string code, string message, params object[] values);

        void NotifyWarn(string code, string message, params object[] values);

        void NotifyHigh(string code, string message, params object[] values);

        void Notify(string code, string message, params object[] values);

        void NotifyLow(string code, string message, params object[] values);

        #endregion

        /// <param name="severity">
        ///  2 = Error <br/>
        ///  1 = Warning <br/>
        ///  0 = Message High <br/>
        /// -1 = Message Normal <br/>
        /// -2 = Message Low <br/>
        /// </param>
        /// <param name="code"></param>
        /// <param name="fileName"></param>
        /// <param name="startPosition"></param>
        /// <param name="endPosition"></param>
        /// <param name="message"></param>
        /// <param name="values"></param>
        /// <remarks>
        /// TODO: An original source code uses an integer values to encode `severity` arg; so ... either use related temp wrappers or migrate to enum asap
        /// </remarks>
        void Notify(int severity, string code, string fileName, SourceCodePosition? startPosition, SourceCodePosition? endPosition, string message, params object[] values);

        /// <param name="env">Environment variable to suppress the warning.</param>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="action">The action will be performed unconditionally after warning.</param>
        /// <param name="limit">Limit warning (not the action) if predicate returns false.</param>
        void WarnAndRun(string env, string code, string message, Action action, Func<bool> limit = null);

        void Notify(DllExportNotificationEventArgs e);

        IDisposable CreateContextName(object context, string name);
    }
}
