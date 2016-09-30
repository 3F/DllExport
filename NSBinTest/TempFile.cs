using System;
using System.IO;

namespace net.r_eg.vsSBE.Test
{
    public sealed class TempFile: IDisposable
    {
        public string dir
        {
            get;
            private set;
        }

        public string file
        {
            get;
            private set;
        }

        public TempFile(bool insideDir = false, string ext = null)
        {
            string path = Path.GetTempPath();
            string name = Guid.NewGuid().ToString();

            if(ext != null) {
                name += ext;
            }

            file = Path.Combine(path, name);
            if(insideDir) {
                dir = Directory.CreateDirectory(file).FullName;
                file = Path.Combine(dir, name);
            }
            using(var f = File.Create(file)) { }
        }

        public void Dispose()
        {
            try {
                File.Delete(file);
                if(dir != null) {
                    Directory.Delete(dir);
                }
            }
            catch { /* we work in temp directory with unique name, so it's not important */ }
        }
    }
}
