// ProgrammingAdvent2018 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System;

namespace ProgrammingAdvent2018.Program
{
    public static class Extensions
    {
        /// <summary>
        /// Returns a new string in which all white space characters 
        /// in the current string are deleted.
        /// </summary>
        /// <remarks>
        /// This method does not modify the value of the current string instance. Instead, 
        /// it returns a new string in which the white space characters have been removed. 
        /// White space characters are those categorized as white space by 
        /// the <seealso cref="char.IsWhiteSpace(char)"/> method.
        /// </remarks>
        /// <returns>
        /// A new string that is equivalent to the current string 
        /// except for the removed characters.
        /// </returns>
        public static string RemoveAllWhiteSpace(this string current)
        {
            char[] charArray = current.ToCharArray();
            int outputLength = 0;
            for (int i = 0; i < charArray.Length; i++)
            {
                if (!char.IsWhiteSpace(charArray[i]))
                {
                    charArray[outputLength++] = charArray[i];
                }
            }
            return new string(charArray, 0, outputLength);
        }

        /// <summary>
        /// Splits a string into substrings based on line breaks.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method splits a string into lines and returns a string array 
        /// containing all of the lines. A line is defined as a sequence of zero or more 
        /// characters followed by a carriage return ('\r'), a line feed ('\n'), or 
        /// a carriage return immediately followed by a line feed ("\r\n"). The strings 
        /// in the array do not contain the terminating carriage return and/or line feed.
        /// </para>
        /// <para>
        /// As an example, <seealso cref="System.IO.File.ReadAllText"/>.ToLines() and 
        /// <seealso cref="System.IO.File.ReadAllLines"/> should produce identical arrays.
        /// </para>
        /// </remarks>
        /// <returns>
        /// An array whose elements contain the substrings in this string 
        /// that are delimited by line breaks.
        /// </returns>
        public static string[] ToLines(this string text)
        {
            if (text.Length == 0)
            {
                return Array.Empty<string>();
            }
            if (text.Length > 1 && text[^2] == '\r' && text[^1] == '\n')
            {
                text = text.Remove(text.Length - 2);
            }
            else if (text[^1] == '\r' || text[^1] == '\n')
            {
                text = text.Remove(text.Length - 1);
            }
            return text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }
    }
}
