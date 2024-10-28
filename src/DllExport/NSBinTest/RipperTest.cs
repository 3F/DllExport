using System;
using System.IO;
using System.Text;
using net.r_eg.DllExport.NSBin;
using Xunit;

namespace net.r_eg.DllExport.NSBinTest
{
    public class RipperTest
    {
        [Fact]
        public void GetBytesFromTest1()
        {
            using TempFile tf = new();
            using Ripper target = new(tf.FullPath, Encoding.UTF8);
            byte[] actual = target.GetBytesFrom("r_1. g");

            Assert.Equal(6, actual.Length);
            Assert.Equal((byte)'r', actual[0]);
            Assert.Equal((byte)'_', actual[1]);
            Assert.Equal((byte)'1', actual[2]);
            Assert.Equal((byte)'.', actual[3]);
            Assert.Equal((byte)' ', actual[4]);
            Assert.Equal((byte)'g', actual[5]);
        }

        [Fact]
        public void ReadWriteTest1()
        {
            using TempFile tf = new();
            Encoding enc = Encoding.UTF8;

            Ripper target = new(tf.FullPath, enc);

            target.Write("r_1. g");
            target.Write([(byte)'0', (byte)'1', (byte)'F', (byte)'4']);

            target.Dispose();

            using Ripper target2 = new(tf.FullPath, enc);

            byte[] readed = target2.Read(3, 1);
            Assert.Equal(3, readed.Length);
            Assert.Equal("_1.", enc.GetString(readed));

            Assert.Equal((ushort)0x01F4, target2.ReadHexStrAsUInt16(2, SeekOrigin.Current));
        }

        [Fact]
        public void FindTest1()
        {
            using TempFile tf = new();
            Encoding enc = Encoding.UTF8;

            using Ripper target = new(tf.FullPath, enc);

            byte[] where = enc.GetBytes("left data 751349 and right data");
            Assert.Equal(5, target.Find(enc.GetBytes("data"), ref where));
            Assert.Equal(10, target.Find("751349", ref where));
            Assert.Equal(-1, target.Find("777749", ref where));
            Assert.Equal(-1, target.Find("Data", ref where));
        }

        [Fact]
        public void FindTest2()
        {
            using TempFile tf = new();
            Encoding enc = Encoding.UTF8;

            using Ripper target = new(tf.FullPath, enc);

            byte[] where = enc.GetBytes("system");
            Assert.Equal(-1, target.Find("systems", ref where));

            byte[] whereZero = [];
            Assert.Equal(-1, target.Find("systems", ref whereZero));
        }

        [Fact]
        public void FindTest3()
        {
            using TempFile tf = new();
            Encoding enc = Encoding.UTF8;

            using Ripper target = new(tf.FullPath, enc);

            byte[] where = enc.GetBytes("system");
            Assert.Equal(0, target.Find("", ref where));
            Assert.Equal(0, target.Find([], ref where));
        }

        [Fact]
        public void CtorTest1()
        {
            Assert.Throws<ArgumentNullException>(() => new Ripper(null, Encoding.UTF8));
        }

    }
}
