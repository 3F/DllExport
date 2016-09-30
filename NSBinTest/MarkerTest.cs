using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.r_eg.DllExport.NSBin;
using net.r_eg.vsSBE.Test;

namespace net.r_eg.DllExport.NSBinTest
{
    [TestClass]
    public class MarkerTest
    {
        [TestMethod]
        public void rwTest1()
        {
            using(var tf = new TempFile())
            {
                MarkerData md = new MarkerData() {
                    nsName      = "net.r_eg",
                    nsBuffer    = 0x01F4,
                    nsPosition  = 1024
                };

                var target = new Marker(tf.file);
                target.write(md);

                int actualVersion;
                MarkerData actual = target.read(out actualVersion);

                Assert.AreEqual(Marker.FORMAT_V, actualVersion);
                Assert.AreEqual(md.nsBuffer, actual.nsBuffer);
                Assert.AreEqual(md.nsName, actual.nsName);
                Assert.AreEqual(md.nsPosition, actual.nsPosition);
            }
        }


    }
}
