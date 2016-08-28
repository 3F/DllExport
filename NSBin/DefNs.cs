// Modification of binary assemblies. Format and specification:
//     
// https://github.com/3F/DllExport/issues/2#issuecomment-231593744
// 
//     Offset(h) 00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F
// 
//     000005B0                 00 C4 7B 01 00 00 00 2F 00 12 05       .Д{..../...
//     000005C0  00 00 02 00 00 00 00 00 00 00 00 00 00 00 26 00  ..............&.
//     000005D0  20 02 00 00 00 00 00 00 00 44 33 46 30 30 46 46   ........D3F00FF   <<<<
//     000005E0  31 37 37 30 44 45 44 39 37 38 45 43 37 37 34 42  1770DED978EC774B   <<<<...
//             
//     - - - -            
//     byte-seq via chars: 
//     + Identifier        = [32]bytes
//     + size of buffer    = [ 4]bytes (range: 0000 - FFF9; reserved: FFFA - FFFF)
//     + buffer of n size
//     - - - -
//     v1.2: 01F4 - allocated buffer size
// 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using net.r_eg.Conari.Log;

namespace NSBin
{
    public class DefNs
    {
        public const string IDNS        = "D3F00FF1770DED978EC774BA389F2DC9";
        public const string DEFAULT_NS  = "System.Runtime.InteropServices";

        public Encoding encoding = Encoding.UTF8;
        protected string dll;

        public ISender Log
        {
            get { return LSender._; }
        }

        public void setNamespace(string name)
        {
            Log.send(this, $"set new namespace: ({name}) - ({dll})");

            if(String.IsNullOrWhiteSpace(dll) || !File.Exists(dll)) {
                throw new FileNotFoundException("The DllExport assembly for modifications was not found.");
            }

            if(String.IsNullOrWhiteSpace(name)) {
                //name = DEFAULT_NS;
                throw new ArgumentException("The namespace cannot be null or empty.");
            }

            defNS(name);
        }

        /// <param name="dll">The DllExport assembly for modifications.</param>
        public DefNs(string dll)
        {
            this.dll = dll;
        }

        protected void defNS(string ns)
        {
            string origin = _postfixToOrigin(dll);

            if(!File.Exists(origin)) {
                File.Copy(dll, origin);
            }
            else {
                File.Copy(origin, dll, true);
            }

            var ident = encoding.GetBytes(IDNS);

            byte[] data = File.ReadAllBytes(dll);
            if(data.Length < ident.Length) {
                throw new FileNotFoundException("Incorrect size of library.");
            }

            int lpos = -1;
            for(int i = 0; i < data.Length; ++i)
            {
                lpos = i;
                for(int j = 0; j < ident.Length; ++j) {
                    if(data[i + j] != ident[j]) {
                        lpos = -1;
                        break;
                    }
                }
                if(lpos != -1) {
                    break;
                }
            }

            if(lpos == -1) {
                throw new FileNotFoundException("Incorrect library.");
            }

            // ~
            binmod(lpos, ident.Length, nsrule(ns));
        }

        protected void binmod(int lpos, int ident, string ns)
        {
            using(FileStream stream = new FileStream(dll, FileMode.Open, FileAccess.ReadWrite))
            {
                stream.Seek(lpos + ident, SeekOrigin.Begin);

                byte[] bsize = new byte[4];
                stream.Read(bsize, 0, 4);

                var buffer = sysrange(
                    Convert.ToUInt16(encoding.GetString(bsize), 16)
                );

                byte[] nsBytes = encoding.GetBytes(ns);
                int fullseq = ident + bsize.Length + buffer;

                // beginning of miracles

                var nsb = new List<byte>();
                nsb.AddRange(nsBytes);
                nsb.AddRange(Enumerable.Repeat((byte)0x00, fullseq - nsBytes.Length));

                stream.Seek(lpos, SeekOrigin.Begin);
                stream.Write(nsb.ToArray(), 0, nsb.Count);

                using(var fs = File.Create(_postfixToUpdated(dll))) {
                    fs.Write(nsBytes, 0, nsBytes.Length);
                }

                Log.send(this, "The DllExport Library has been modified !");
                Log.send(this, $"namespace: {ns}");
                Log.send(this, "Details here: https://github.com/3F/DllExport/issues/2");
            }
        }

        protected virtual UInt16 sysrange(UInt16 val)
        {
            /* reserved: FFFA - FFFF */

            if(val < 0xFFFA) {
                return val;
            }

            var reserved = 0xFFFF - val;
            //switch(reserved) {
            //    // ...
            //}

            throw new NotImplementedException("The reserved combination is not yet implemented or not supported: " + reserved);
        }

        protected virtual string nsrule(string name)
        {
            return Regex.Replace(name, @"[\.\s]{1,}", ".").Trim(new char[] { '.', ' ' });
        }

        private string _postfixToUpdated(string dll)
        {
            return $"{dll}.updated";
        }

        private string _postfixToOrigin(string dll)
        {
            return $"{dll}.origin";
        }
    }
}
