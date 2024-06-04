/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
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
