// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using System.Text.RegularExpressions;
using ProgrammingAdvent2022.Common;

namespace ProgrammingAdvent2022.Solutions;

internal partial class Day18 : Day
{
    public static readonly Regex ValidInputLine = ValidInputLineRegex();

    [GeneratedRegex("^(?<x>[0-9]{1,2}),(?<y>[0-9]{1,2}),(?<z>[0-9]{1,2})$")]
    private static partial Regex ValidInputLineRegex();

    private static readonly Vector3Int[] _directions =
    [
        Vector3Int.UnitX, Vector3Int.UnitY, Vector3Int.UnitZ,
        -Vector3Int.UnitX, -Vector3Int.UnitY, -Vector3Int.UnitZ,
    ];

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers result = new();

        Vector3Int[] cubes = input.Select(line => ParseInputLine(line)).ToArray();

        Dictionary<Vector3Int, int> cubeSides = [];
        foreach (Vector3Int cube in cubes)
        {
            cubeSides[cube] = 6;
        }
        foreach (Vector3Int cube in cubes)
        {
            foreach (Vector3Int direction in _directions)
            {
                if (cubeSides.ContainsKey(cube + direction))
                {
                    cubeSides[cube]--;
                }
            }
        }
        int surfaceArea = cubeSides.Values.Sum();

        return result.WriteAnswers(surfaceArea, null);
    }

    private static Vector3Int ParseInputLine(string line)
    {
        Match match = ValidInputLine.Match(line);
        if (!match.Success)
        {
            throw new InvalidOperationException();
        }
        int x = int.Parse(match.Groups["x"].Value);
        int y = int.Parse(match.Groups["y"].Value);
        int z = int.Parse(match.Groups["z"].Value);
        return new Vector3Int(x, y, z);
    }
}
