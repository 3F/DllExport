/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016  Denis Kuzmin <entry.reg@gmail.com>
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/

using System.Management.Automation;
using System.Text;

namespace net.r_eg.DllExport.NSBin
{
    [Cmdlet(VerbsCommon.Set, "DDNS")]
    public class SetDDNSCmdlet: Cmdlet
    {
        private object synch = new object();

        /// <summary>
        /// Full path to prepared library.
        /// </summary>
        [Parameter(Mandatory = true)]
        public string Dll
        {
            get;
            set;
        }

        /// <summary>
        /// New namespace.
        /// </summary>
        [Parameter(Mandatory = true)]
        public string Namespace
        {
            get;
            set;
        }

        protected override void ProcessRecord()
        {
            IDDNS ddns = new DDNS(Encoding.UTF8);

            lock(synch) {
                ddns.Log.Received -= onMsg;
                ddns.Log.Received += onMsg;

                ddns.setNamespace(Dll, Namespace);

                ddns.Log.Received -= onMsg;
            }
        }

        private void onMsg(object sender, net.r_eg.Conari.Log.Message e)
        {
            WriteObject(e.content); //TODO:
        }
    }
}
