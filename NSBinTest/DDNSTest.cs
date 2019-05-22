using System;
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
            Assert.AreEqual(true, DDNS.IsValidNS("  "));
            Assert.AreEqual(true, DDNS.IsValidNS(" "));
            Assert.AreEqual(true, DDNS.IsValidNS(null));
            Assert.AreEqual(true, DDNS.IsValidNS(String.Empty));
        }

        [TestMethod]
        public void nsTest2()
        {
            Assert.AreEqual(false, DDNS.IsValidNS("net.r-eg.DllExport"));
            Assert.AreEqual(false, DDNS.IsValidNS("net.  r_eg  .   DllExport"));
            Assert.AreEqual(false, DDNS.IsValidNS("net.r_eg.DllExport."));
            Assert.AreEqual(false, DDNS.IsValidNS("1net.r_eg.DllExport"));
            
            Assert.AreEqual(false, DDNS.IsValidNS("net.r_eg..DllExport"));
            Assert.AreEqual(false, DDNS.IsValidNS(".net.r_eg.DllExport"));

            Assert.AreEqual(true, DDNS.IsValidNS("_net.r_eg.DllExport"));
        }

        [TestMethod]
        public void nsTest3()
        {
            // https://github.com/3F/DllExport/issues/61#issuecomment-352804273

            Assert.AreEqual(true, DDNS.IsValidNS("あいうえおかきくけこ"));
            Assert.AreEqual(true, DDNS.IsValidNS("中文解决方案名称"));
            Assert.AreEqual(true, DDNS.IsValidNS("あいうえおかきくけこ.DllExport"));
            Assert.AreEqual(true, DDNS.IsValidNS("中文解决方案名称.DllExport"));

            Assert.AreEqual(true, DDNS.IsValidNS("Проверка"));
            Assert.AreEqual(true, DDNS.IsValidNS("Проверка.DllExport"));

            Assert.AreEqual(false, DDNS.IsValidNS("0あいうえおかきくけこ"));
            Assert.AreEqual(false, DDNS.IsValidNS("0中文解决方案名称"));
            Assert.AreEqual(false, DDNS.IsValidNS("0あいうえおかきくけこ.DllExport"));
            Assert.AreEqual(false, DDNS.IsValidNS("0中文解决方案名称.DllExport"));

            Assert.AreEqual(false, DDNS.IsValidNS("0Проверка"));
            Assert.AreEqual(false, DDNS.IsValidNS("0Проверка.DllExport"));
        }

    }
}
