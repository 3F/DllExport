using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.r_eg.DllExport.NSBin;

namespace net.r_eg.DllExport.NSBinTest
{
    [TestClass]
    public class DDNSTest
    {
        [TestMethod]
        public void nsTest1()
        {
            Assert.AreEqual(true, DDNS.IsValidNS("net.r_eg.DllExport"));
            Assert.AreEqual(false, DDNS.IsValidNS("net.r_eg..DllExport"));
            Assert.AreEqual(false, DDNS.IsValidNS("net.r-eg.DllExport"));
            Assert.AreEqual(false, DDNS.IsValidNS("net.  r_eg  .   DllExport"));
            Assert.AreEqual(false, DDNS.IsValidNS("net.r_eg.DllExport."));
            Assert.AreEqual(false, DDNS.IsValidNS("0net.r_eg.DllExport"));
            Assert.AreEqual(false, DDNS.IsValidNS(".net.r_eg.DllExport"));
            Assert.AreEqual(false, DDNS.IsValidNS("  "));
            Assert.AreEqual(false, DDNS.IsValidNS(null));
        }


    }
}
