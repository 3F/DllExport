// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.3.29766, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;

namespace RGiesecke.DllExport
{
    public class GenericDisposable: IDisposable
    {
        private readonly Action _Action;

        public GenericDisposable(Action action)
        {
            if(action == null)
            {
                throw new ArgumentNullException("action");
            }
            this._Action = action;
        }

        public void Dispose()
        {
            this._Action();
        }
    }
}
