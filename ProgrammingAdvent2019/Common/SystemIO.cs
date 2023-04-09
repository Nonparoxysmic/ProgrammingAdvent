// ProgrammingAdvent2019 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

namespace ProgrammingAdvent2019.Common;

internal static class SystemIO
{
    public static bool TryReadInputFile(int dayNumber, out string[] lines)
    {
        string path = $"InputFiles\\InputDay{dayNumber:00}.txt";
        if (dayNumber < 1 || dayNumber > 25 || !File.Exists(path))
        {
            lines = Array.Empty<string>();
            return false;
        }
        try
        {
            lines = File.ReadAllLines(path);
        }
        catch
        {
            lines = Array.Empty<string>();
            return false;
        }
        return true;
    }
}
