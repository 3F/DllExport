using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.r_eg.DllExport.NSBin;
using net.r_eg.vsSBE.Test;

namespace net.r_eg.DllExport.NSBinTest
{
    [TestClass]
    public class RipperTest
    {
        [TestMethod]
        public void getBytesFromTest1()
        {
            using(var tf = new TempFile())
            {
                var target = new Ripper(tf.file, Encoding.UTF8);
                byte[] actual = target.getBytesFrom("r_1. g");

                Assert.AreEqual(6, actual.Length);
                Assert.AreEqual((byte)'r', actual[0]);
                Assert.AreEqual((byte)'_', actual[1]);
                Assert.AreEqual((byte)'1', actual[2]);
                Assert.AreEqual((byte)'.', actual[3]);
                Assert.AreEqual((byte)' ', actual[4]);
                Assert.AreEqual((byte)'g', actual[5]);
            }
        }

        [TestMethod]
        public void rwTest1()
        {
            using(var tf = new TempFile())
            {
                Encoding enc = Encoding.UTF8;

                var target = new Ripper(tf.file, enc);

                target.write("r_1. g");
                target.write(new byte[] { (byte)'0', (byte)'1', (byte)'F', (byte)'4' });

                target.Dispose();

                var target2 = new Ripper(tf.file, enc);

                byte[] readed = target2.read(3, 1);
                Assert.AreEqual(3, readed.Length);
                Assert.AreEqual("_1.", enc.GetString(readed));

                Assert.AreEqual((ushort)0x01F4, target2.readHexStrAsUInt16(2, SeekOrigin.Current));
            }
        }

        [TestMethod]
        public void findTest1()
        {
            using(var tf = new TempFile())
            {
                Encoding enc = Encoding.UTF8;

                var target = new Ripper(tf.file, enc);

                byte[] where = enc.GetBytes("left data 751349 and right data");
                Assert.AreEqual(5, target.find(enc.GetBytes("data"), ref where));
                Assert.AreEqual(10, target.find("751349", ref where));
                Assert.AreEqual(-1, target.find("777749", ref where));
                Assert.AreEqual(-1, target.find("Data", ref where));
            }
        }

        [TestMethod]
        public void findTest2()
        {
            using(var tf = new TempFile())
            {
                Encoding enc = Encoding.UTF8;

                var target = new Ripper(tf.file, enc);

                byte[] where = enc.GetBytes("system");
                Assert.AreEqual(-1, target.find("systems", ref where));

                byte[] whereZero = new byte[0];
                Assert.AreEqual(-1, target.find("systems", ref whereZero));
            }
        }

        [TestMethod]
        public void findTest3()
        {
            using(var tf = new TempFile())
            {
                Encoding enc = Encoding.UTF8;

                var target = new Ripper(tf.file, enc);

                byte[] where = enc.GetBytes("system");
                Assert.AreEqual(0, target.find("", ref where));
                Assert.AreEqual(0, target.find(new byte[0], ref where));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ctorTest1()
        {
            new Ripper(null, Encoding.UTF8);
        }

    }
}
