// ProgrammingAdvent2019 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Text.Json;

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
            return true;
        }
        catch
        {
            lines = Array.Empty<string>();
            return false;
        }
    }

    public static bool TryReadExampleFile(out JsonDocument? jsonDocument, out string errorMessage)
    {
        string path = "Examples.json";
        if (!File.Exists(path))
        {
            jsonDocument = null;
            errorMessage = $"Example file \"{path}\" does not exist.";
            return false;
        }
        try
        {
            using StreamReader? streamReader = new(path);
            jsonDocument = JsonDocument.Parse(streamReader.BaseStream);
            errorMessage = string.Empty;
            return true;
        }
        catch
        {
            jsonDocument = null;
            errorMessage = $"Unable to read and parse example file \"{path}\".";
            return false;
        }
    }
}
