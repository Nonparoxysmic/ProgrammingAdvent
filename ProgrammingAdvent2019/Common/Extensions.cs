// ProgrammingAdvent2019 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

namespace ProgrammingAdvent2019.Common;

internal static class Extensions
{
    private static readonly Random _random = new();

    /// <summary>
    /// Assigns the given value of type T to each element of the specified two-dimensional array.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <param name="array">The array to be filled.</param>
    /// <param name="value">The value to assign to each array element.</param>
    public static void Fill<T>(this T?[,] array, T? value)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                array[i, j] = value;
            }
        }
    }

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
    /// <param name="current">The string from which to remove white space characters.</param>
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
    /// Randomly sorts the elements of an <seealso cref="IList{T}"/> collection.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method shuffles into a random order the elements of a list, array, 
    /// or other collection that implements <seealso cref="IList{T}"/>.
    /// </para>
    /// <para>
    /// The elements are shuffled with a Fisher-Yates algorithm, using 
    /// <seealso cref="Random.Next(int, int)"/> to generate the randomization.
    /// </para>
    /// </remarks>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <param name="collection">The collection to be shuffled.</param>
    public static void Shuffle<T>(this IList<T?> collection)
    {
        for (int i = 0; i < collection.Count - 1; i++)
        {
            // Choose random index j within i (inclusive)
            // and the number of elements in the collection (exclusive).
            int j = _random.Next(i, collection.Count);
            // Swap the elements at indices i and j.
            (collection[i], collection[j]) = (collection[j], collection[i]);
        }
    }

    /// <summary>
    /// Splits a string into substrings based on line breaks.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method splits a string into lines and returns a string array 
    /// containing all of the lines. A line is defined as a sequence of zero or more 
    /// characters followed by a line feed ('\n'), or a carriage return 
    /// immediately followed by a line feed ("\r\n"). The strings 
    /// in the array do not contain the terminating carriage return and/or line feed.
    /// </para>
    /// <para>
    /// As an example, <seealso cref="File.ReadAllText"/>.ToLines() and 
    /// <seealso cref="File.ReadAllLines"/> should produce identical arrays.
    /// </para>
    /// </remarks>
    /// <returns>
    /// An array whose elements contain the substrings in this string 
    /// that are delimited by line breaks.
    /// </returns>
    /// <param name="text">The string to be split.</param>
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
        else if (text[^1] == '\n')
        {
            text = text.Remove(text.Length - 1);
        }
        return text.Split(_newlines, StringSplitOptions.None);
    }
    private static readonly string[] _newlines = new[] { "\r\n", "\n" };
}
