using System;

namespace ProgrammingAdvent2018.Program
{
    public static class Extensions
    {
        /// <summary>
        /// Splits a string into substrings based on line breaks.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method splits a string into lines and returns a string array 
        /// containing all of the lines. A line is defined as a sequence of zero or more characters 
        /// followed by a carriage return ('\r'), a line feed ('\n'), or a carriage return 
        /// immediately followed by a line feed ("\r\n"). The strings in the array 
        /// do not contain the terminating carriage return and/or line feed.
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
            else if (text[^1] == '\n' || text[^1] == '\r')
            {
                text = text.Remove(text.Length - 1);
            }
            return text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }
    }
}
