using net.r_eg.DllExport.NSBin;
using Xunit;

namespace net.r_eg.DllExport.NSBinTest
{
    public class DDNSTest
    {
        [Fact]
        public void IsValidNSTest1()
        {
            Assert.Equal(true, DDNS.IsValidNS("net.r_eg.DllExport"));
            Assert.Equal(true, DDNS.IsValidNS("  "));
            Assert.Equal(true, DDNS.IsValidNS(" "));
            Assert.Equal(true, DDNS.IsValidNS(null));
            Assert.Equal(true, DDNS.IsValidNS(string.Empty));
        }

        [Fact]
        public void IsValidNSTest2()
        {
            Assert.Equal(false, DDNS.IsValidNS("net.r-eg.DllExport"));
            Assert.Equal(false, DDNS.IsValidNS("net.  r_eg  .   DllExport"));
            Assert.Equal(false, DDNS.IsValidNS("net.r_eg.DllExport."));
            Assert.Equal(false, DDNS.IsValidNS("1net.r_eg.DllExport"));
            
            Assert.Equal(false, DDNS.IsValidNS("net.r_eg..DllExport"));
            Assert.Equal(false, DDNS.IsValidNS(".net.r_eg.DllExport"));

            Assert.Equal(true, DDNS.IsValidNS("_net.r_eg.DllExport"));
        }

        [Fact]
        public void IsValidNSTest3()
        {
            // https://github.com/3F/DllExport/issues/61#issuecomment-352804273

            Assert.Equal(true, DDNS.IsValidNS("あいうえおかきくけこ"));
            Assert.Equal(true, DDNS.IsValidNS("中文解决方案名称"));
            Assert.Equal(true, DDNS.IsValidNS("あいうえおかきくけこ.DllExport"));
            Assert.Equal(true, DDNS.IsValidNS("中文解决方案名称.DllExport"));

            Assert.Equal(true, DDNS.IsValidNS("Проверка"));
            Assert.Equal(true, DDNS.IsValidNS("Проверка.DllExport"));

            Assert.Equal(false, DDNS.IsValidNS("0あいうえおかきくけこ"));
            Assert.Equal(false, DDNS.IsValidNS("0中文解决方案名称"));
            Assert.Equal(false, DDNS.IsValidNS("0あいうえおかきくけこ.DllExport"));
            Assert.Equal(false, DDNS.IsValidNS("0中文解决方案名称.DllExport"));

            Assert.Equal(false, DDNS.IsValidNS("0Проверка"));
            Assert.Equal(false, DDNS.IsValidNS("0Проверка.DllExport"));
        }

    }
}
