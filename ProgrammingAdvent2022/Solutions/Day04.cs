// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using System.Text.RegularExpressions;
using ProgrammingAdvent2022.Common;

namespace ProgrammingAdvent2022.Solutions;

internal partial class Day04 : Day
{
    public static readonly Regex ValidInputLine = ValidInputLineRegex();

    [GeneratedRegex("^(?<a>[0-9]{1,2})-(?<b>[0-9]{1,2}),(?<c>[0-9]{1,2})-(?<d>[0-9]{1,2})$")]
    private static partial Regex ValidInputLineRegex();

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers result = new();

        int totalOverlaps = 0;
        foreach (string line in input)
        {
            Match match = ValidInputLine.Match(line);
            if (match.Success)
            {
                int a = int.Parse(match.Groups["a"].Value);
                int b = int.Parse(match.Groups["b"].Value);
                int c = int.Parse(match.Groups["c"].Value);
                int d = int.Parse(match.Groups["d"].Value);
                if ((a <= c && c <= b && a <= d && d <= b) ||
                    (c <= a && a <= d && c <= b && b <= d))
                {
                    totalOverlaps++;
                }
            }
            else
            {
                return result.WriteError("Invalid line in input.");
            }
        }

        return result.WriteAnswers(totalOverlaps, null);
    }
}
