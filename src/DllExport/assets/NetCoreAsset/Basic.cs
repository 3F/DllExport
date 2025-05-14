using System.Reflection;
using System.Text;
using io.github._3F.DllExport;

namespace NetCoreAsset
{
    public static class Basic
    {
        [DllExport]
        public static double touch()
        {
            string str = "Hello World!";
            char[] arrchars = ['H', 'E', 'L', 'L', 'O', '\0', '\0'];

            ReadOnlySpan<char> rosS = str.AsSpan();
            ReadOnlySpan<char> rosC = arrchars;

            Console.WriteLine("ReadOnlySpan<char> rosC = " + rosC.ToString());
            Console.WriteLine("ReadOnlySpan<char> rosS = " + rosS.ToString());

            nuint a = 0x10_000_000;

            int[] arr = [1, 2, 3, 4, 5, 1];
            ReadOnlySpan<int> ints = arr;

            foreach(int i in ints) Console.WriteLine("i = " + i.ToString());
            foreach(int i in ints) Console.Write($"i = {i}, ");
            Console.WriteLine();

            Memory<int> mem = arr;
            mem.Slice(2, 2);
            foreach(int i in mem.ToArray()) Console.WriteLine($"i = '{i:x8}' ({arr.Length * i}); ");

            async Task<string> _M1() => await Task.FromResult($"processed {nameof(_M1)}");

            async Task<byte[]> _M2(string src)
            {
                using FileStream f = new(src, FileMode.Open, FileAccess.Read);
                byte[] buf = new byte[4];
                _ = await f.ReadAsync(buf, 0, buf.Length);
                return buf;
            }

            Task.Factory.StartNew(async () =>
            {
                Console.WriteLine($"{await _M1()}");

                byte[] data = await _M2(typeof(Basic).Assembly.Location);
                Console.WriteLine($"x x: {data[0]:X2} {data[1]:X2}");
            })
            .Wait();

            _ = ints.ToArray();
            int[] d3 = new int[ints.Length + 1];
            ints.CopyTo(d3);

            DateTime date = DateTime.Now;
            Console.WriteLine($")( {date:yyyy.MM.dd HH:mm:ss} ({date.Month})".ToString());

            int x = 2, y = 3;
            Console.WriteLine($""" "{x}, {y}" = {Math.Sqrt(x * x + y * y):F3}; {(x > 1 ? "$" : "@")} """);
            Console.WriteLine($$"""{ {{{x}}, {{y}}} = {{Math.Sqrt(x * x + y * y):F3}}; }""");

            Console.WriteLine($"{"Left",-12}|{"Right",5}");

            const int _A = 12;
            Console.WriteLine($"{Math.PI,_A:F3}");

            System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("ja-JP");
            Console.WriteLine($"{12345.678:N3}");

            int b = (int)Math.Pow(a, 2);
            StringBuilder sb = new();
            sb.Append("b = ");
            sb.Append(b);
            sb.AppendLine();

            Console.WriteLine($"{sb}");

            return Math.PI;
        }

#if DLLEXPORT_REF_NOMERGE
        static Basic()
        {
            string dir = Path.GetDirectoryName(typeof(Basic).Assembly.Location)!;
            Assembly? _Get(string input, string asm)
                => input.StartsWith(asm + ",") ? Assembly.LoadFrom(Path.Combine(dir, asm + ".dll")) : null;

            AppDomain.CurrentDomain.AssemblyResolve += (s, a) => _Get(a.Name, "System.Memory");
            AppDomain.CurrentDomain.AssemblyResolve += (s, a) => _Get(a.Name, "System.Runtime.CompilerServices.Unsafe");
        }
#endif
    }
}
