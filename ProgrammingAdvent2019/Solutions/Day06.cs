// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Text.RegularExpressions;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day06 : Day
{
    private static readonly Regex _validLine = new("^(?<A>[0-9A-Z]{1,3})[)](?<B>[0-9A-Z]{1,3})$");

    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        if (inputLines.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        Dictionary<string, string> primaries = new();
        foreach (string line in inputLines)
        {
            Match match = _validLine.Match(line);
            if (!match.Success)
            {
                errorMessage = $"Invalid input line \"{line.Left(20, true)}\".";
                return false;
            }
            string primary = match.Groups["A"].Value;
            string satellite = match.Groups["B"].Value;
            if (primaries.ContainsKey(satellite))
            {
                errorMessage = $"\"{satellite}\" orbits multiple objects.";
                return false;
            }
            primaries.Add(satellite, primary);
        }
        if (!primaries.ContainsValue("COM"))
        {
            errorMessage = "Input does not contain COM.";
            return false;
        }
        if (primaries.ContainsKey("COM"))
        {
            errorMessage = "COM is not a satellite.";
            return false;
        }
        foreach (string satellite in primaries.Keys)
        {
            string trace = primaries[satellite];
            while (trace != "COM")
            {
                if (!primaries.ContainsKey(trace))
                {
                    errorMessage = $"\"{satellite}\" does not orbit COM.";
                    return false;
                }
                if (trace == satellite)
                {
                    errorMessage = $"\"{satellite}\" orbits itself.";
                    return false;
                }
                trace = primaries[trace];
            }
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines)
    {
        PuzzleAnswers output = new();
        Dictionary<string, string> primaries = new();
        foreach (string line in inputLines)
        {
            Match match = _validLine.Match(line);
            string primary = match.Groups["A"].Value;
            string satellite = match.Groups["B"].Value;
            primaries.Add(satellite, primary);
        }
        int orbitCount = 0;
        string trace;
        foreach (string satellite in primaries.Keys)
        {
            trace = primaries[satellite];
            orbitCount++;
            while (trace != "COM")
            {
                trace = primaries[trace];
                orbitCount++;
            }
        }
        if (!primaries.ContainsKey("YOU") || !primaries.ContainsKey("SAN"))
        {
            return output.WriteAnswers(orbitCount, "Input does not contain YOU and SAN.");
        }
        if (primaries["YOU"] == primaries["SAN"])
        {
            return output.WriteAnswers(orbitCount, 0);
        }
        Dictionary<string, int> santaDistance = new();
        trace = "SAN";
        int steps = 0;
        do
        {
            trace = primaries[trace];
            santaDistance.Add(trace, ++steps);
        } while (trace != "COM");
        trace = "YOU";
        steps = 0;
        do
        {
            trace = primaries[trace];
            steps++;
        } while (!santaDistance.ContainsKey(trace));
        int transfersRequired = steps + santaDistance[trace] - 2;
        return output.WriteAnswers(orbitCount, transfersRequired);
    }
}
