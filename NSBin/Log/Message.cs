/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016-2019  Denis Kuzmin < entry.reg@gmail.com > GitHub/3F
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

using System;

namespace net.r_eg.Conari.Log
{
    [Serializable]
    public class Message: EventArgs
    {
        public DateTime stamp;

        public string content;

        public Exception exception;

        public object data;

        public Level type;

        public enum Level
        {
            Trace,
            Debug,
            Info,
            Warn,
            Error,
            Fatal
        }

        public Message(string msg, Level type = Level.Debug)
        {
            content     = msg;
            this.type   = type;
            stamp       = DateTime.Now;
        }

        public Message(string msg, Exception ex, Level type = Level.Error)
            : this(msg, type)
        {
            exception = ex;
        }

        public Message(string msg, object data, Level type = Level.Debug)
            : this(msg, type)
        {
            this.data = data;
        }
    }
}
