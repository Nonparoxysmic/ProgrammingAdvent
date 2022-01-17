using System;
using System.IO;

namespace ProgrammingAdvent2017.Program
{
    internal static class IO
    {
        private static readonly string NL = Environment.NewLine;

        private static string InputFilePath(int dayNumber)
        {
            return "InputFiles\\InputDay" + dayNumber.ToString("D2") + ".txt";
        }

        public static bool TryReadInputFile(int dayNumber, out string result)
        {
            string path = InputFilePath(dayNumber);
            try
            {
                result = File.ReadAllText(path);
                return true;
            }
            catch (FileNotFoundException e)
            {
                result = "\"" + path + "\" not found." + NL + NL
                    + e.GetType().FullName + ": " + e.Message;
                return false;
            }
            catch (Exception e)
            {
                result = e.GetType().FullName + ": " + e.Message;
                return false;
            }
        }
    }
}
