/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.IO;
using System.Text;

namespace net.r_eg.DllExport.NSBin
{
    public abstract class BinaryData: BinaryReader, IDisposable
    {
        public Encoding Enc
        {
            get;
            protected set;
        }

        public virtual byte[] GetBytesFrom(string data)
        {
            return Enc.GetBytes(data);
        }

        public void Write(string data, int offset, SeekOrigin origin = SeekOrigin.Begin)
        {
            Write(GetBytesFrom(data), offset, origin);
        }

        public void Write(string data)
        {
            Write(GetBytesFrom(data));
        }

        public void Write(byte[] data, int offset, SeekOrigin origin = SeekOrigin.Begin)
        {
            BaseStream.Seek(offset, origin);
            Write(data);
        }

        public void Write(byte[] data)
        {
            BaseStream.Write(data, 0, data.Length);
        }

        public byte[] Read(int count)
        {
            byte[] buf = new byte[count];
            BaseStream.Read(buf, 0, count);
            return buf;
        }

        public byte[] Read(int count, int offset, SeekOrigin origin = SeekOrigin.Begin)
        {
            BaseStream.Seek(offset, origin);
            return Read(count);
        }

        public byte[] ReadFirst64K()
        {
            try {
                return Read(UInt16.MaxValue, 0);
            }
            finally {
                BaseStream.Seek(0, SeekOrigin.Begin);
            }
        }

        public UInt16 ReadHexStrAsUInt16()
        {
            byte[] bsize = new byte[4];
            BaseStream.Read(bsize, 0, 4);

            try {
                return Convert.ToUInt16(Enc.GetString(bsize), 16);
            }
            catch(Exception ex) {
                throw new ArgumentException("Incorrect UInt16 data from stream", ex);
            }
        }

        public UInt16 ReadHexStrAsUInt16(int offset, SeekOrigin origin = SeekOrigin.Begin)
        {
            BaseStream.Seek(offset, origin);
            return ReadHexStrAsUInt16();
        }

        public int Find(string what, ref byte[] where)
        {
            return Find(GetBytesFrom(what), ref where);
        }

        public int Find(byte[] what, ref byte[] where)
        {
            int lpos = -1;

            if(where.Length < 1 || what.Length > where.Length) {
                return lpos;
            }

            for(int i = 0; i < where.Length; ++i)
            {
                lpos = i;
                for(int j = 0; j < what.Length; ++j) {
                    if(where[i + j] != what[j]) {
                        lpos = -1;
                        break;
                    }
                }

                if(lpos != -1) {
                    break;
                }
            }

            return lpos;
        }

        public BinaryData(string fname, Encoding encoding)
            : base(File.Open(fname, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite), encoding)
        {
            Enc = encoding;
        }

        public BinaryData(string fname)
            : this(fname, Encoding.UTF8)
        {

        }

        #region IDisposable

        private bool disposed;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if(!disposed)
            {
                BaseStream?.Dispose();
                disposed = true;
            }
        }

        #endregion
    }
}
