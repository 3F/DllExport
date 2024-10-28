using net.r_eg.DllExport.NSBin;
using Xunit;

namespace net.r_eg.DllExport.NSBinTest
{
    public class DDNSTest
    {
        [Fact]
        public void IsValidNSTest1()
        {
            Assert.True(DDNS.IsValidNS("net.r_eg.DllExport"));
            Assert.True(DDNS.IsValidNS("  "));
            Assert.True(DDNS.IsValidNS(" "));
            Assert.True(DDNS.IsValidNS(null));
            Assert.True(DDNS.IsValidNS(string.Empty));
        }

        [Fact]
        public void IsValidNSTest2()
        {
            Assert.False(DDNS.IsValidNS("net.r-eg.DllExport"));
            Assert.False(DDNS.IsValidNS("net.  r_eg  .   DllExport"));
            Assert.False(DDNS.IsValidNS("net.r_eg.DllExport."));
            Assert.False(DDNS.IsValidNS("1net.r_eg.DllExport"));
            
            Assert.False(DDNS.IsValidNS("net.r_eg..DllExport"));
            Assert.False(DDNS.IsValidNS(".net.r_eg.DllExport"));

            Assert.True(DDNS.IsValidNS("_net.r_eg.DllExport"));
        }

        [Fact]
        public void IsValidNSTest3()
        {
            // https://github.com/3F/DllExport/issues/61#issuecomment-352804273

            Assert.True(DDNS.IsValidNS("あいうえおかきくけこ"));
            Assert.True(DDNS.IsValidNS("中文解决方案名称"));
            Assert.True(DDNS.IsValidNS("あいうえおかきくけこ.DllExport"));
            Assert.True(DDNS.IsValidNS("中文解决方案名称.DllExport"));

            Assert.True(DDNS.IsValidNS("Проверка"));
            Assert.True(DDNS.IsValidNS("Проверка.DllExport"));

            Assert.False(DDNS.IsValidNS("0あいうえおかきくけこ"));
            Assert.False(DDNS.IsValidNS("0中文解决方案名称"));
            Assert.False(DDNS.IsValidNS("0あいうえおかきくけこ.DllExport"));
            Assert.False(DDNS.IsValidNS("0中文解决方案名称.DllExport"));

            Assert.False(DDNS.IsValidNS("0Проверка"));
            Assert.False(DDNS.IsValidNS("0Проверка.DllExport"));
        }

    }
}
