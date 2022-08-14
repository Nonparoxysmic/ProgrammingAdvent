// ProgrammingAdvent2018 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System;
using System.Diagnostics;
using System.IO;

namespace ProgrammingAdvent2018.Program
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
                string filename = Path.GetFileName(path);
                result = $"Puzzle input \"{filename}\" not found in InputFiles folder."
                    + NL + NL + e.Message;
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
    }
}
