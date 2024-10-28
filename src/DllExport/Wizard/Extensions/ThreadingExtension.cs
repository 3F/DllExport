/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System.Threading;
using System.Threading.Tasks;

namespace net.r_eg.DllExport.Wizard.Extensions
{
    internal static class ThreadingExtension
    {
        internal static CancellationTokenSource CancelAndResetIfRunning(this CancellationTokenSource cts, Task task, int signalLimit)
        {
            var ret = new CancellationTokenSource();
            if(cts == null) {
                return ret;
            }

            if(cts.Token.CanBeCanceled == true && task?.Status == TaskStatus.Running)
            {
                cts.Cancel();
                task.Wait(signalLimit);
            }

            cts.Dispose();
            return ret;
        }
    }
}