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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace net.r_eg.DllExport.NSBin
{
    public class Marker: BinaryData
    {
        public const int FORMAT_V = 2;

        public void write(MarkerData data)
        {
            BaseStream.Seek(0, SeekOrigin.Begin);
            BaseStream.SetLength(0);

            write(BitConverter.GetBytes(FORMAT_V));

            (new BinaryFormatter()).Serialize(BaseStream, data);
        }

        public MarkerData read(out int version)
        {
            BaseStream.Seek(0, SeekOrigin.Begin);
            version = ReadInt32(); // ... reserved

            return (MarkerData)(new BinaryFormatter()).Deserialize(BaseStream);
        }

        public Marker(string fname)
            : base(fname)
        {

        }
    }
}
