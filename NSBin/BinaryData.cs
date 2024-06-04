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

        public virtual byte[] getBytesFrom(string data)
        {
            return Enc.GetBytes(data);
        }

        public void write(string data, int offset, SeekOrigin origin = SeekOrigin.Begin)
        {
            write(getBytesFrom(data), offset, origin);
        }

        public void write(string data)
        {
            write(getBytesFrom(data));
        }

        public void write(byte[] data, int offset, SeekOrigin origin = SeekOrigin.Begin)
        {
            BaseStream.Seek(offset, origin);
            write(data);
        }

        public void write(byte[] data)
        {
            BaseStream.Write(data, 0, data.Length);
        }

        public byte[] read(int count)
        {
            byte[] buf = new byte[count];
            BaseStream.Read(buf, 0, count);
            return buf;
        }

        public byte[] read(int count, int offset, SeekOrigin origin = SeekOrigin.Begin)
        {
            BaseStream.Seek(offset, origin);
            return read(count);
        }

        public byte[] readFirst64K()
        {
            try {
                return read(UInt16.MaxValue, 0);
            }
            finally {
                BaseStream.Seek(0, SeekOrigin.Begin);
            }
        }

        public UInt16 readHexStrAsUInt16()
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

        public UInt16 readHexStrAsUInt16(int offset, SeekOrigin origin = SeekOrigin.Begin)
        {
            BaseStream.Seek(offset, origin);
            return readHexStrAsUInt16();
        }

        public int find(string what, ref byte[] where)
        {
            return find(getBytesFrom(what), ref where);
        }

        public int find(byte[] what, ref byte[] where)
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

        protected void free()
        {
            if(BaseStream != null) {
                BaseStream.Dispose();
            }
        }

        #region IDisposable

        // To detect redundant calls
        private bool disposed = false;

        //public void Dispose()
        //{
        //    Dispose(true);
        //}

        protected override void Dispose(bool disposing)
        {
            if(disposed) {
                return;
            }
            disposed = true;

            //...
            free();
        }

        #endregion
    }
}
