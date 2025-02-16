// Part of https://github.com/3F/DllExport
namespace System.Runtime.CompilerServices
{
    // Do NOT use any modern syntax or features here like 'primary constructors' (LangVersion=12+) etc;
    // ! remember LangVersion=latest is on the user side

    using System;
    using System.Text;

#pragma warning disable // LangVersion can be different and everything is on the user side

    // .NET 6+ ; note .NET 5 generates using string.Format() directly
    public class DefaultInterpolatedStringHandler
    {
        private readonly StringBuilder data;

        public void AppendLiteral(string value)
        {
            data.Append(value);
        }

        public void AppendFormatted<T>(T value)
        {
            data.Append(value);
        }

        public void AppendFormatted<T>(T value, string format)
        {
            data.Append(string.Format("{0:" + format + "}", value));
        }

        public void AppendFormatted<T>(T value, int alignment)
        {
            data.Append(string.Format("{0," + alignment + "}", value));
        }

        public void AppendFormatted<T>(T value, int alignment, string format)
        {
            data.Append(string.Format("{0," + alignment + ":" + format + "}", value));
        }

        public void AppendFormatted(ReadOnlySpan<char> value)
        {
            data.Append(value.ToString());
        }

        public void AppendFormatted(ReadOnlySpan<char> value, int alignment = 0, string format = null)
        {
            AppendFormatted(value.ToString(), alignment, format);
        }

        public void AppendFormatted(string value)
        {
            data.Append(value);
        }

        public void AppendFormatted(string value, int alignment = 0, string format = null)
        {
            AppendFormatted<string>(value, alignment, format);
        }

        public void AppendFormatted(object value, int alignment = 0, string format = null)
        {
            AppendFormatted<object>(value, alignment, format);
        }

        public string ToStringAndClear()
        {
            return data.ToString();
        }

        public override string ToString()
        {
            return data.ToString();
        }

        public DefaultInterpolatedStringHandler(int literalLength, int formattedCount)
        {
            data = new StringBuilder(literalLength * Math.Max(1, formattedCount) + 30);
        }

        public DefaultInterpolatedStringHandler(int literalLength, int formattedCount, IFormatProvider provider)
            : this(literalLength, formattedCount)
        {

        }

        public DefaultInterpolatedStringHandler(int literalLength, int formattedCount, IFormatProvider provider, Span<char> initialBuffer)
            : this(literalLength, formattedCount)
        {

        }
    }
#pragma warning restore // LangVersion can be different and everything is on the user side
}
