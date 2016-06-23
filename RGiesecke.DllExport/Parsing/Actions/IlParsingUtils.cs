// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.3.29766, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;

namespace RGiesecke.DllExport.Parsing.Actions
{
    internal static class IlParsingUtils
    {
        public static void ParseIlSnippet(string inputText, ParsingDirection direction, Func<IlParsingUtils.IlSnippetLocation, bool> predicate, Action<IlParsingUtils.IlSnippetFinalizaton> finalization = null)
        {
            bool withinString = false;
            bool atOuterBracket = false;
            int nestedBrackets = 0;
            int endIndex = inputText.Length - 1;
            bool wasInterupted = false;
            int num1 = direction == ParsingDirection.Forward ? 1 : -1;
            int num2 = direction == ParsingDirection.Forward ? 0 : endIndex;
            Func<int, bool> func = direction == ParsingDirection.Forward ? (Func<int, bool>)(i => i <= endIndex) : (Func<int, bool>)(i => i > -1);
            int lastPosition = -1;
            int index = num2;
            while(func(index))
            {
                char currentChar = inputText[index];
                atOuterBracket = false;
                if((int)currentChar == 39)
                {
                    withinString = !withinString;
                }
                else if(!withinString)
                {
                    if((int)currentChar == 41)
                    {
                        atOuterBracket = nestedBrackets == 0;
                        ++nestedBrackets;
                    }
                    else if((int)currentChar == 40)
                    {
                        --nestedBrackets;
                        atOuterBracket = nestedBrackets == 0;
                    }
                }
                if(!predicate(new IlParsingUtils.IlSnippetLocation(inputText, index, currentChar, withinString, nestedBrackets, atOuterBracket)))
                {
                    wasInterupted = true;
                    break;
                }
                lastPosition = index;
                index += num1;
            }
            if(finalization == null)
            {
                return;
            }
            finalization(new IlParsingUtils.IlSnippetFinalizaton(inputText, lastPosition, wasInterupted, withinString, nestedBrackets, atOuterBracket));
        }

        public class IlSnippetLocationBase
        {
            public string InputText
            {
                get;
                private set;
            }

            public bool WithinString
            {
                get;
                private set;
            }

            public int NestedBrackets
            {
                get;
                private set;
            }

            public bool AtOuterBracket
            {
                get;
                private set;
            }

            protected IlSnippetLocationBase(string inputText, bool withinString, int nestedBrackets, bool atOuterBracket)
            {
                if(inputText == null)
                {
                    throw new ArgumentNullException("inputText");
                }
                this.InputText = inputText;
                this.WithinString = withinString;
                this.NestedBrackets = nestedBrackets;
                this.AtOuterBracket = atOuterBracket;
            }
        }

        public sealed class IlSnippetFinalizaton: IlParsingUtils.IlSnippetLocationBase
        {
            public int LastPosition
            {
                get;
                private set;
            }

            public bool WasInterupted
            {
                get;
                private set;
            }

            public IlSnippetFinalizaton(string inputText, int lastPosition, bool wasInterupted, bool withinString, int nestedBrackets, bool atOuterBracket)
            : base(inputText, withinString, nestedBrackets, atOuterBracket)
            {
                this.LastPosition = lastPosition;
                this.WasInterupted = wasInterupted;
            }
        }

        public class IlSnippetLocation: IlParsingUtils.IlSnippetLocationBase
        {
            public int Index
            {
                get;
                private set;
            }

            public char CurrentChar
            {
                get;
                private set;
            }

            public IlSnippetLocation(string inputText, int index, char currentChar, bool withinString, int nestedBrackets, bool atOuterBracket)
            : base(inputText, withinString, nestedBrackets, atOuterBracket)
            {
                this.Index = index;
                this.CurrentChar = currentChar;
            }
        }
    }
}
