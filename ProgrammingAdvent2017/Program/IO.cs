// ProgrammingAdvent2017 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System;
using System.Diagnostics;
using System.IO;

namespace ProgrammingAdvent2017.Program
{
    internal static class IO
    {
        private static readonly string NL = Environment.NewLine;

        private static string InputFilePath(int dayNumber)
        {
            return @"InputFiles\InputDay" + dayNumber.ToString("D2") + ".txt";
        }

        internal static bool TryReadInputFile(int dayNumber, out string result)
        {
            string path = InputFilePath(dayNumber);
            try
            {
                result = File.ReadAllText(path);
                return true;
            }
            catch (FileNotFoundException e)
            {
                result = $"\"{path}\" not found." + NL + NL
                    + e.GetType().FullName + ": " + e.Message;
                return false;
            }
            catch (Exception e)
            {
                result = e.GetType().FullName + ": " + e.Message;
                return false;
            }
        }

        internal static string WriteInputFile(int dayNumber, string inputText)
        {
            if (!Directory.Exists("InputFiles"))
            {
                _ = Directory.CreateDirectory("InputFiles");
            }
            string path = InputFilePath(dayNumber);
            try
            {
                File.WriteAllText(path, inputText);
                return $"Input text for Day {dayNumber} saved to:{NL}{NL}\"{path}\"";
            }
            catch (Exception e)
            {
                return e.GetType().FullName + ": " + e.Message;
            }
        }

        internal static bool OpenInputFolder(out string exceptionMessage)
        {
            if (!Directory.Exists("InputFiles"))
            {
                _ = Directory.CreateDirectory("InputFiles");
            }
            string folderPath = Directory.GetCurrentDirectory() + @"\InputFiles";
            try
            {
                _ = Process.Start("explorer.exe", folderPath);
                exceptionMessage = null;
                return true;
            }
            catch (Exception e)
            {
                exceptionMessage = "Unable to open input file folder in File Explorer."
                    + NL + NL + e.GetType().FullName + ": " + e.Message;
                return false;
            }
        }

        /// <summary>
        /// Splits a string into substrings that are based on line breaks.
        /// </summary>
        internal static string[] ToLines(this string input)
        {
            return input
                .TrimEnd(new[] { '\r', '\n' })
                .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }
    }
}
