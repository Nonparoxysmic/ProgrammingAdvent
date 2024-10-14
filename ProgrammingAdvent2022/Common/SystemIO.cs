﻿// ProgrammingAdvent2022 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace ProgrammingAdvent2022.Common;

internal static class SystemIO
{
    public static bool TryReadInputFile(int dayNumber, out string[] lines)
    {
        string path = $"InputFiles\\InputDay{dayNumber:00}.txt";
        if (dayNumber < 1 || dayNumber > 25 || !File.Exists(path))
        {
            lines = [];
            return false;
        }
        try
        {
            lines = File.ReadAllLines(path);
            return true;
        }
        catch
        {
            lines = [];
            return false;
        }
    }

    public static bool TryReadExampleFile([NotNullWhen(true)] out JsonDocument? jsonDocument)
    {
        string path = "Examples\\Examples.json";
        if (!File.Exists(path))
        {
            jsonDocument = null;
            return false;
        }
        try
        {
            using StreamReader? streamReader = new(path);
            jsonDocument = JsonDocument.Parse(streamReader.BaseStream);
            return true;
        }
        catch
        {
            jsonDocument = null;
            return false;
        }
    }
}