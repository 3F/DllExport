using net.r_eg.DllExport.NSBin;
using Xunit;

namespace net.r_eg.DllExport.NSBinTest
{
    public class MarkerTest
    {
        [Fact]
        public void ReadWriteTest1()
        {
            using TempFile tf = new();
            MarkerData md = new()
            {
                nsName = "net.r_eg",
                nsBuffer = 0x01F4,
                nsPosition = 1024
            };

            using Marker target = new(tf.FullPath);
            target.Write(md);

            MarkerData actual = target.Read(out int actualVersion);

            Assert.Equal(Marker.FORMAT_V, actualVersion);
            Assert.Equal(md.nsBuffer, actual.nsBuffer);
            Assert.Equal(md.nsName, actual.nsName);
            Assert.Equal(md.nsPosition, actual.nsPosition);
        }


    }
}
