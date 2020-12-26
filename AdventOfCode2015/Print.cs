using System;

namespace AdventOfCode2015
{
    static class Print
    {
        static readonly string wrappingIndent = "  ";
        static readonly bool indentFirstLine = false;

        public static void PrintErrorAndExit(string message = "Unknown Error")
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            PrintWrappedText("ERROR: " + message);
            PrintWrappedText("Press any key to exit.");
            Console.ReadKey(true);
            Environment.Exit(0);
        }

        public static void PrintWrappedText(string text)
        {
            PrintWrappedText(text, Console.WindowWidth - 1, wrappingIndent, indentFirstLine);
        }

        public static void PrintWrappedText(string text, int width, string indent = "", bool doIndent = false)
        {
            string trimmedText = text.Trim();

            int indentLength = 0;
            if (doIndent)
            {
                trimmedText = indent + trimmedText;
                indentLength = indent.Length;
            }

            if (trimmedText.Length <= width)
            {
                Console.WriteLine(trimmedText);
                return;
            }

            int lineBreakWidth = width;
            for (int pos = width; pos > indentLength; pos--)
            {
                if (Char.IsWhiteSpace(trimmedText[pos]))
                {
                    lineBreakWidth = pos;
                    break;
                }
            }
            string firstLine = trimmedText.Substring(0, lineBreakWidth).TrimEnd();
            string remainder = trimmedText.Substring(lineBreakWidth);

            Console.WriteLine(firstLine);
            if (indent != "") doIndent = true;
            PrintWrappedText(remainder, width, indent, doIndent);
        }
    }
}
