﻿// ProgrammingAdvent2019 by Nonparoxysmic
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
    /// Retrieves a substring starting at the left end of a string.
    /// </summary>
    /// <remarks>
    /// This method returns:
    /// <list type="bullet">
    /// <item>
    /// The empty string, if the instance string has no characters or 
    /// the specified maximum length is less than one.
    /// </item>
    /// <item>
    /// Otherwise, the instance string unchanged, if the instance string is shorter 
    /// than the specified maximum length.
    /// </item>
    /// <item>
    /// Otherwise, a new string consisting of the leftmost characters 
    /// of the instance string, up to the specified maximum length. <br/>
    /// If <paramref name="allowEllipsis"/> is <b>true</b>, up to three of the 
    /// trailing characters of the new string will be replaced with periods.
    /// </item>
    /// </list>
    /// </remarks>
    /// <returns>
    /// A string containing the leftmost characters of the instance string.
    /// </returns>
    /// <param name="instance">The string from which to take the leftmost characters.</param>
    /// <param name="maxLength">The maximum length of the returned string.</param>
    /// <param name="allowEllipsis">Whether to end a truncated string with an ellipsis.</param>
    public static string Left(this string instance, int maxLength, bool allowEllipsis = false)
    {
        if (string.IsNullOrEmpty(instance) || maxLength < 1)
        {
            return string.Empty;
        }
        if (instance.Length <= maxLength)
        {
            return instance;
        }
        if (allowEllipsis)
        {
            char[] charArray = instance.ToCharArray();
            for (int i = maxLength - 1; i >= maxLength - 3 && i >= 0; i--)
            {
                charArray[i] = '.';
            }
            return new string(charArray, 0, maxLength);
        }
        else
        {
            return instance[..maxLength];
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
    /// Replaces subsequences of strings in a collection of strings.
    /// </summary>
    /// <param name="collection">The collection to be modified.</param>
    /// <param name="pattern">The pattern to replace.</param>
    /// <param name="replacement">The value with which to replace the pattern.</param>
    public static void ReplaceSequence(this IList<string> collection, IList<string> pattern, string replacement)
    {
        if (collection.Count == 0 || pattern.Count == 0 || pattern.Count > collection.Count)
        {
            return;
        }
        for (int i = 0; i < collection.Count - pattern.Count + 1; i++)
        {
            bool patternMatch = true;
            for (int j = 0; j < pattern.Count; j++)
            {
                patternMatch &= (collection[i + j] == pattern[j]);
            }
            if (patternMatch)
            {
                for (int j = 0; j < pattern.Count; j++)
                {
                    collection[i + j] = replacement;
                }
            }
        }
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
    /// Swaps two elements in an array or an <seealso cref="IList{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <param name="collection">The collection with the two elements to be swapped.</param>
    /// <param name="indexA">The index of one of the elements to swap.</param>
    /// <param name="indexB">The index of the other element to swap.</param>
    public static void Swap<T>(this IList<T?> collection, int indexA, int indexB)
    {
        (collection[indexA], collection[indexB]) = (collection[indexB], collection[indexA]);
    }

    /// <summary>
    /// Converts a one-dimensional string array into a two-dimensional character array.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If the <paramref name="width"/> parameter is less than zero, the width of the
    /// output will default to the length of the first string in the string array.
    /// </para>
    /// <para>
    /// If a line of the string array is longer than the width of the output array, 
    /// the extra characters will be ignored. If it is shorter, the corresponding line 
    /// in the output array will be padded with the <paramref name="defaultChar"/> character.
    /// </para>
    /// </remarks>
    /// <returns>
    /// A two-dimensional <seealso cref="char"/> array containing the characters from 
    /// the original one-dimensional string array.
    /// </returns>
    /// <param name="strings">The array of strings to convert.</param>
    /// <param name="width">The width of the output array.</param>
    /// <param name="defaultChar">The default character of the output array.</param>
    public static char[,] ToCharArray2D(this string[] strings, int width = -1, char defaultChar = '\0')
    {
        width = width < 0 ? strings[0].Length : width;
        char[,] output = new char[width, strings.Length];
        for (int y = 0; y < strings.Length; y++)
        {
            for (int x = 0; x < Math.Min(strings[y].Length, width); x++)
            {
                output[x, y] = strings[y][x];
            }
            for (int x = strings[y].Length; x < width; x++)
            {
                output[x, y] = defaultChar;
            }
        }
        return output;
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

    /// <summary>
    /// Converts a character to lowercase.
    /// </summary>
    /// <remarks>
    /// If the character is an uppercase letter in the <b>Basic Latin</b> or 
    /// <b>Latin-1 Supplement</b> Unicode blocks with a lowercase equivalent in the same block, 
    /// this method returns the lowercase form. Otherwise, the original character is returned.
    /// </remarks>
    /// <returns>
    /// The same character, converted to lowercase if possible.
    /// </returns>
    /// <param name="character">The character to be converted to lowercase.</param>
    public static char ToLower(this char character)
    {
        if ('A' <= character && character <= 'Z')
        {
            return (char)(character + 32);
        }
        if ('\u00C0' <= character && character <= '\u00D6')
        {
            return (char)(character + 32);
        }
        if ('\u00D8' <= character && character <= '\u00DE')
        {
            return (char)(character + 32);
        }
        return character;
    }

    /// <summary>
    /// Converts a character to uppercase.
    /// </summary>
    /// <remarks>
    /// If the character is a lowercase letter in the <b>Basic Latin</b> or 
    /// <b>Latin-1 Supplement</b> Unicode blocks with an uppercase equivalent in the same block, 
    /// this method returns the uppercase form. Otherwise, the original character is returned.
    /// </remarks>
    /// <returns>
    /// The same character, converted to uppercase if possible.
    /// </returns>
    /// <param name="character">The character to be converted to uppercase.</param>
    public static char ToUpper(this char character)
    {
        if ('a' <= character && character <= 'z')
        {
            return (char)(character - 32);
        }
        if ('\u00E0' <= character && character <= '\u00F6')
        {
            return (char)(character - 32);
        }
        if ('\u00F8' <= character && character <= '\u00FE')
        {
            return (char)(character - 32);
        }
        return character;
    }
}
