// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.6.36226, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System.Text;

namespace RGiesecke.DllExport
{
    public sealed class SourceCodeRange
    {
        public string FileName
        {
            get;
            private set;
        }

        public SourceCodePosition StartPosition
        {
            get;
            private set;
        }

        public SourceCodePosition EndPosition
        {
            get;
            private set;
        }

        public SourceCodeRange(string fileName, SourceCodePosition startPosition, SourceCodePosition endPosition)
        {
            this.FileName = fileName;
            this.StartPosition = startPosition;
            this.EndPosition = endPosition;
        }

        public static SourceCodeRange FromMsIlLine(string line)
        {
            SourceCodePosition start;
            SourceCodePosition end;
            string fileName;
            if(!SourceCodeRange.ExtractLineParts(line, out start, out end, out fileName))
            {
                return (SourceCodeRange)null;
            }
            return new SourceCodeRange(fileName, start, end);
        }

        private static bool ExtractLineParts(string line, out SourceCodePosition start, out SourceCodePosition end, out string fileName)
        {
            start = new SourceCodePosition();
            end = new SourceCodePosition();
            fileName = (string)null;
            line = line.TrimStart();
            if(!line.StartsWith(".line"))
            {
                return false;
            }
            line = line.Substring(5).Trim();
            if(string.IsNullOrEmpty(line))
            {
                return false;
            }
            int startIndex = 0;
            string lineText1 = (string)null;
            string lineText2 = (string)null;
            string columnText1 = (string)null;
            string columnText2 = (string)null;
            StringBuilder stringBuilder = new StringBuilder(line.Length);
            bool flag = false;
            for(int index = 0; index < line.Length; ++index)
            {
                char ch = line[index];
                if((int)ch == 39)
                {
                    if(!flag)
                    {
                        string str = line.Substring(startIndex, index - startIndex).Trim();
                        if(columnText2 == null)
                        {
                            columnText2 = str;
                        }
                    }
                    flag = !flag;
                }
                else if(flag)
                {
                    stringBuilder.Append(ch);
                }
                else if((int)ch == 44 || (int)ch == 58)
                {
                    string str = line.Substring(startIndex, index - startIndex).Trim();
                    startIndex = index + 1;
                    switch(ch)
                    {
                        case ',':
                        if(lineText1 == null)
                        {
                            lineText1 = str;
                            continue;
                        }
                        columnText1 = str;
                        continue;
                        case ':':
                        if(lineText2 == null)
                        {
                            lineText2 = str;
                            continue;
                        }
                        columnText2 = str;
                        continue;
                        default:
                        continue;
                    }
                }
            }
            start = SourceCodePosition.FromText(lineText1, columnText1) ?? start;
            end = SourceCodePosition.FromText(lineText2, columnText2) ?? end;
            fileName = stringBuilder.Length > 0 ? stringBuilder.ToString() : (string)null;
            return fileName != null;
        }

        private bool Equals(SourceCodeRange other)
        {
            if(string.Equals(this.FileName, other.FileName) && this.StartPosition.Equals(other.StartPosition))
            {
                return this.EndPosition.Equals(other.EndPosition);
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            if(object.ReferenceEquals((object)null, obj))
            {
                return false;
            }
            if(object.ReferenceEquals((object)this, obj))
            {
                return true;
            }
            if(obj is SourceCodeRange)
            {
                return this.Equals((SourceCodeRange)obj);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ((this.FileName != null ? this.FileName.GetHashCode() : 0) * 397 ^ this.StartPosition.GetHashCode()) * 397 ^ this.EndPosition.GetHashCode();
        }
    }
}
