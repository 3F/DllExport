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

using System;

namespace net.r_eg.Conari.Log
{
    /// <summary>
    /// A simple retranslator.
    /// Use the NLog etc.
    /// </summary>
    public class LSender: ISender
    {
        /// <summary>
        /// When message has been received.
        /// </summary>
        public event EventHandler<Message> Received = delegate(object sender, Message e) { };

        /// <summary>
        /// Static alias to Received.
        /// </summary>
        public static event EventHandler<Message> SReceived
        {
            add {
                _.Received += value;
            }
            remove {
                _.Received -= value;
            }
        }

        /// <summary>
        /// Static alias to `send(object sender, Message msg)`
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msg"></param>
        public static void Send(object sender, Message msg)
        {
            _.send(sender, msg);
        }

        /// <summary>
        /// Static alias to `send(object sender, string msg)`
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msg"></param>
        public static void Send(object sender, string msg)
        {
            _.send(sender, msg);
        }

        /// <summary>
        /// Static alias to `send(object sender, string msg, Message.Level type)`
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msg"></param>
        /// <param name="type"></param>
        public static void Send(object sender, string msg, Message.Level type)
        {
            _.send(sender, msg, type);
        }

        /// <summary>
        /// To send new message with default sender as typeof(T).
        /// It useful for static methods etc.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        public static void Send<T>(Message msg) where T : class
        {
            _.send<T>(msg);
        }

        /// <summary>
        /// To send new message with default sender as typeof(T).
        /// It useful for static methods etc.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        public static void Send<T>(string msg) where T : class
        {
            _.send<T>(msg);
        }

        /// <summary>
        /// To send new message with default sender as typeof(T).
        /// It useful for static methods etc.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <param name="type"></param>
        public static void Send<T>(string msg, Message.Level type) where T : class
        {
            _.send<T>(msg, type);
        }

        /// <summary>
        /// To send new message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msg"></param>
        public void send(object sender, Message msg)
        {
            if(sender == null) {
                sender = this;
            }

            Received(sender, msg);
        }

        /// <summary>
        /// To send new message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msg"></param>
        public void send(object sender, string msg)
        {
            send(sender, msg, Message.Level.Debug);
        }

        /// <summary>
        /// To send new message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msg"></param>
        /// <param name="type"></param>
        public void send(object sender, string msg, Message.Level type)
        {
            send(sender, new Message(msg, type));
        }

        /// <summary>
        /// To send new message with default sender as typeof(T).
        /// It useful for static methods etc.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        public void send<T>(Message msg) where T : class
        {
            send(typeof(T), msg);
        }

        /// <summary>
        /// To send new message with default sender as typeof(T).
        /// It useful for static methods etc.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        public void send<T>(string msg) where T : class
        {
            send(typeof(T), msg);
        }

        /// <summary>
        /// To send new message with default sender as typeof(T).
        /// It useful for static methods etc.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <param name="type"></param>
        public void send<T>(string msg, Message.Level type) where T : class
        {
            send(typeof(T), msg, type);
        }

        /// <summary>
        /// Thread-safe getting the instance of the Sender class
        /// </summary>
        public static ISender _
        {
            get { return _lazy.Value; }
        }
        private static readonly Lazy<ISender> _lazy = new Lazy<ISender>(() => new LSender());

        protected LSender() { }
    }
}
